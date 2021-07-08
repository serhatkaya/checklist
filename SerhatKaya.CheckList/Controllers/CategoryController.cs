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
    public class CategoryController : ControllerBase
    {
        private ILogger<CategoryController> _logger;
        private readonly CheckListContext _clContext;

        public CategoryController(CheckListContext clContext, ILogger<CategoryController> logger)
        {
            _clContext = clContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var resp = await _clContext.Categories.Where(x => x.DeletedDateTime == null)
                .Include(x => x.TagItems)
                .ThenInclude(x => x.Tag)
                .ToListAsync();
            foreach (var res in resp)
            {
                var clists = await _clContext.CheckLists.Where(x => x.CategoryId == res.Id && x.DeletedDateTime == null)
                    .ToListAsync();
                res.Count = clists.Count;
                foreach (var tags in res.TagItems)
                {
                    if (tags.Tag.TagItems != null)
                    {
                        tags.Tag.TagItems = null;
                    }
                }
            }

            return StatusCode(200, resp);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category entity)
        {
            var apiResult = new ApiResult
            {
                Message = "Add Succeed!",
                Succeed = true
            };
            try
            {
                var newCategory = entity;
                _clContext.Categories.Add(newCategory);
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

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetChecklistByCategory(int id)
        {
            var resp = new CheckListInCategory
            {
                Category = await _clContext.Categories.Where(x => x.Id == id && x.DeletedDateTime == null)
                    .Include(x => x.TagItems)
                    .ThenInclude(x => x.Tag)
                    .FirstOrDefaultAsync(),
                CheckLists = await _clContext.CheckLists.Where(x => x.CategoryId == id && x.DeletedDateTime == null)
                    .ToListAsync(),
            };

            return StatusCode(200, resp);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateCategory(Category entity)
        {
            var apiResult = new ApiResult
            {
                Message = "Update succeed!",
                Succeed = true
            };
            try
            {
                var cat = await _clContext.Categories.Where(x => x.Id == entity.Id).Include(x => x.TagItems)
                    .FirstOrDefaultAsync();
                cat.Description = entity.Description;
                cat.CategoryName = entity.CategoryName;
                cat.TagItems = entity.TagItems;
                cat.Theme = entity.Theme;
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

        [Authorize(Roles = "Admin")]
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveCategory(Category entity)
        {
            var category = await _clContext.Categories.Where(x => x.Id == entity.Id)
                .Include(x => x.CheckLists)
                .ThenInclude(x => x.CheckListItems)
                .Include(x => x.CheckLists)
                .ThenInclude(x => x.Logs)
                .Include(x => x.TagItems)
                .FirstOrDefaultAsync();

            foreach (var checkList in category.CheckLists)
            {
                foreach (var listItem in checkList.CheckListItems)
                {
                    _clContext.CheckListItems.Remove(listItem);
                }

                foreach (var log in checkList.Logs)
                {
                    _clContext.Logs.Remove(log);
                }

                _clContext.CheckLists.Remove(checkList);
            }

            foreach (var tagItem in category.TagItems)
            {
                _clContext.TagItems.Remove(tagItem);
            }

            _clContext.Categories.Remove(category);
            await _clContext.SaveChangesAsync();

            return Ok();
        }
    }
}

public class CheckListInCategory
{
    public List<CheckList> CheckLists { get; set; }
    public Category Category { get; set; }
}