using System;
using System.Linq;
using System.Threading.Tasks;
using SerhatKaya.CheckList.Context;
using SerhatKaya.CheckList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SerhatKaya.CheckList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckListController : ControllerBase
    {
        private readonly ILogger<CheckListController> _logger;
        private readonly CheckListContext _clContext;

        public CheckListController(CheckListContext clContext, ILogger<CheckListController> logger)
        {
            _clContext = clContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetIndex()
        {
            var resp = await _clContext.CheckLists.Where(x => x.DeletedDateTime == null)
                .Include(x => x.CheckListItems)
                .Include(x => x.Category)
                .ThenInclude(x => x.TagItems)
                .ThenInclude(x => x.Tag)
                .ToListAsync();
            return Ok(resp);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var resp = await _clContext.CheckLists.Where(x => x.DeletedDateTime == null && x.Id == id)
                .Include(x => x.CheckListItems)
                .Include(x => x.Category)
                .ThenInclude(x => x.TagItems)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync();
            return StatusCode(200, resp);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCheckList(Entities.CheckList entity)
        {
            var apiResult = new ApiResult
            {
                Message = "Successfully added!",
                Succeed = true
            };
            try
            {
                var newCl = entity;
                _clContext.CheckLists.Add(newCl);
                await _clContext.SaveChangesAsync();
                return StatusCode(200, apiResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                apiResult.Succeed = false;
                apiResult.Message = "Add Failed!";
                apiResult.Data = ex.Message;
                return StatusCode(400, apiResult);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveCheckList([FromBody] int id)
        {
            var apiResult = new ApiResult
            {
                Message = "Successfully removed!",
                Succeed = true
            };
            try
            {
                var cl = await _clContext.CheckLists.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (cl != null)
                {
                    cl.DeletedDateTime = DateTime.Now;
                }

                await _clContext.SaveChangesAsync();
                return StatusCode(200, apiResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                apiResult.Message = "Remove Failed!";
                apiResult.Succeed = false;
                return StatusCode(400, apiResult);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> EditCheckList(Entities.CheckList entity)
        {
            var apiResult = new ApiResult
            {
                Message = "Update succeed!",
                Succeed = true
            };
            try
            {
                var cList = await _clContext.CheckLists.Where(x => x.Id == entity.Id).Include(x => x.CheckListItems)
                    .FirstOrDefaultAsync();
                cList.CategoryId = entity.CategoryId;
                cList.Description = entity.Description;
                cList.Header = entity.Header;
                cList.CheckListItems = entity.CheckListItems;
                cList.Theme = entity.Theme;

                await _clContext.SaveChangesAsync();
                return StatusCode(200, apiResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                apiResult.Message = "Update Failed!";
                apiResult.Succeed = false;
                return StatusCode(400, apiResult);
            }
        }
    }
}