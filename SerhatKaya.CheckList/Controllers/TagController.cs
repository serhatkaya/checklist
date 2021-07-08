using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SerhatKaya.CheckList.Context;
using SerhatKaya.CheckList.Entities;
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
    public class TagController : ControllerBase
    {
        private ILogger<TagController> _logger;
        private readonly CheckListContext _clContext;

        public TagController(CheckListContext clContext, ILogger<TagController> logger)
        {
            _clContext = clContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            return StatusCode(200, await _clContext.Tags.Where(x => x.DeletedDateTime == null).ToListAsync());
        }

        [HttpGet("getCategories/{id}")]
        public async Task<IActionResult> GetCategoriesByTag(int id)
        {
            var data = await _clContext.Tags.Where(x => x.Id == id).Include(x => x.TagItems)
                .ThenInclude(x => x.Category).ToListAsync();

            return StatusCode(200, data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTag(Tag entity)
        {
            var apiResult = new ApiResult
            {
                Message = "Add succeed!",
                Succeed = true
            };
            try
            {
                var newTag = entity;
                _clContext.Tags.Add(newTag);
                await _clContext.SaveChangesAsync();
                return StatusCode(200, apiResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                apiResult.Message = "Add Failed!";
                apiResult.Succeed = false;
                return StatusCode(400, apiResult);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveTag([FromBody] int id)
        {
            var apiResult = new ApiResult
            {
                Message = "Remove succeed!",
                Succeed = true
            };
            try
            {
                var category = await _clContext.Tags.Where(x => x.Id == id).FirstOrDefaultAsync();
                category.DeletedDateTime = DateTime.Now;
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
        [HttpPost("edit")]
        public async Task<IActionResult> EditTag(Tag entity)
        {
            var apiResult = new ApiResult
            {
                Message = "Update succeed!",
                Succeed = true
            };
            try
            {
                var tag = await _clContext.Tags.Where(x => x.Id == entity.Id).FirstOrDefaultAsync();
                if (entity.TagName != tag.TagName) tag.TagName = entity.TagName;
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