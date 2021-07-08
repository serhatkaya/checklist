using System;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SerhatKaya.CheckList.Context;
using SerhatKaya.CheckList.Entities;
using SerhatKaya.CheckList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SerhatKaya.CheckList.Helper;

namespace SerhatKaya.CheckList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly CheckListContext _clContext;
        private readonly IConfiguration _configuration;

        public AuthController(CheckListContext clContext, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _clContext = clContext;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel userModel)
        {
            var apiResult = new ApiResult
            {
                Succeed = false,
                Message = "Login Failed!"
            };


            var loginUser = await _clContext.Users.Where(x => x.DeletedDateTime == null && x.Email == userModel.email)
                .FirstOrDefaultAsync();

            if (loginUser == null) return StatusCode(400, apiResult);

            if (!VerifyPasswordHash(userModel.password, loginUser.PasswordHash, loginUser.PasswordSalt))
                return StatusCode(400, apiResult);

            loginUser.Token = GenerateToken(loginUser);

            return StatusCode(200, loginUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var userList = await _clContext.Users.ToListAsync();
            return StatusCode(200, userList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> AddUser(User user)
        {
            var apiResult = new ApiResult
            {
                Message = "Add succeed",
                Succeed = true
            };
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _clContext.Users.Add(user);
            await _clContext.SaveChangesAsync();
            return StatusCode(200, apiResult);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser(User entity)
        {
            var updatedUser = await _clContext.Users.Where(x => x.Id == entity.Id && x.DeletedDateTime == null)
                .FirstOrDefaultAsync();

            if (updatedUser != null)
            {
                if (entity.NewPassword != null)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(entity.NewPassword, out passwordHash, out passwordSalt);
                    updatedUser.PasswordHash = passwordHash;
                    updatedUser.PasswordSalt = passwordSalt;
                }

                if (entity.UserFullName != null) updatedUser.UserFullName = entity.UserFullName;
                if (entity.Email != null) updatedUser.Email = entity.Email;
                if (entity.Role != updatedUser.Role) updatedUser.Role = entity.Role;

                await _clContext.SaveChangesAsync();
                return StatusCode(200, new ApiResult { Message = "Update succeed!", Succeed = true });
            }

            return StatusCode(200, new ApiResult { Message = "No changes!", Succeed = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveUser(User user)
        {
            var apiResult = new ApiResult
            {
                Message = "Remove success!",
                Succeed = true
            };
            var userToDelete = await _clContext.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();
            if (userToDelete != null)
            {
                _clContext.Users.Remove(userToDelete);
                await _clContext.SaveChangesAsync();
            }

            return StatusCode(200, apiResult);
        }

        #region PrivateMethods

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hMac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hMac.Key;
                passwordHash = hMac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            using (var hMac = new System.Security.Cryptography.HMACSHA512(userPasswordSalt))
            {
                var computeHash = hMac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != userPasswordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string GenerateToken(User user)
        {
            try
            {
                var appSettingsSection = _configuration.GetSection("AppSettings");
                var appSettings = appSettingsSection.Get<AppSettings>();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.Now.AddHours(8),
                    SigningCredentials =
                        new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }

        #endregion
    }
}

public class AuthenticateResponse
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string UserFullName { get; set; }
    public string Token { get; set; }

    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        Email = user.Email;
        UserFullName = user.UserFullName;
        Token = token;
    }
}