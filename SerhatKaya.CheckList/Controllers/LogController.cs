using System;
using System.Threading.Tasks;
using SerhatKaya.CheckList.Context;
using SerhatKaya.CheckList.Entities;
using SerhatKaya.CheckList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SerhatKaya.CheckList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly CheckListContext _clContext;

        public LogController(CheckListContext clContext)
        {
            _clContext = clContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var resp = await _clContext.Logs
                .Include(x => x.User)
                .Include(x => x.CheckList)
                .ToListAsync();
            return StatusCode(200, resp);
        }

        [Authorize]
        [HttpPost("confirm")]
        public async Task<IActionResult> AddLog(Logs log)
        {
            var newLog = log;
            _clContext.Logs.Add(newLog);
            await _clContext.SaveChangesAsync();
            return StatusCode(200, new ApiResult { Succeed = true, Message = "Log created!" });
        }
    }
}