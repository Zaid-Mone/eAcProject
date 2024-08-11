using DataAccess.Helper;
using DataAccess.IRepository;
using DTOs.Enums;
using DTOs.Products;
using eShop.CustomAttributes;
using eShop.Middleware;
using eShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace eShop.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [CustomAuth(groupIds: [(int)UserGroups.Provider, (int)UserGroups.Admin, (int)UserGroups.SuperAdmin])]

    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerHelper _loggerHelper;


        public CategoriesController(ICategoryRepository categoryRepository, ILoggerHelper loggerHelper)
        {
            _categoryRepository = categoryRepository;
            _loggerHelper = loggerHelper;
        }

        //[HttpGet("GetCategories")]
        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            try
            {
                var categories = _categoryRepository.FindAllByCondition(x => x.IsDeleted == false);
                await _loggerHelper.AddLog(HttpContext);
                return Ok(categories);
            }
            catch (Exception)
            {
                await _loggerHelper.AddLog(HttpContext);
                return BadRequest("Something went wrong.");
            }
        }

        //[HttpGet("GetCategory")]
        [HttpGet]
        public async Task<ActionResult> GetCategory(Guid ID)
        {
            try
            {
                if (ID == Guid.Empty)
                {
                    return BadRequest();
                }
                var categories = _categoryRepository.FindByCondition(x => x.IsDeleted == false && x.ID == ID);
                await _loggerHelper.AddLog(HttpContext);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                await _loggerHelper.AddLog(HttpContext);
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet("FilterCategories")]
        [HttpGet]
        public async Task<ActionResult> FilterCategories(string filter = "")
        {
            try
            {
                if (string.IsNullOrEmpty(filter))
                {
                    var categories = _categoryRepository.FindAllByCondition(x => x.IsDeleted == false);
                    return Ok(categories);
                }
                else
                {
                    var categories = _categoryRepository.Search(filter);
                    await _loggerHelper.AddLog(HttpContext);
                    return Ok(categories);
                }
            }
            catch (Exception)
            {
                await _loggerHelper.AddLog(HttpContext);
                return BadRequest("Something went wrong.");
            }
        }

        //[HttpPost("AddCategory")]
        [HttpPost]
        public async Task<ActionResult> AddCategory(CategoryDTO Dto)
        {
            try
            {
                var categories = _categoryRepository.FindByCondition(x => x.EnglishTitle == Dto.EnglishTitle && x.IsDeleted == false);
                if (categories is not null)
                {
                    await _loggerHelper.AddLog(HttpContext, Dto);
                    return BadRequest("The category is already exist.");
                }
                if (string.IsNullOrEmpty(Dto.EnglishTitle))
                {
                    await _loggerHelper.AddLog(HttpContext, Dto);
                    return BadRequest("Please Enter Title");
                }
                _categoryRepository.Insert(new Category
                {
                    ArabicTitle = Dto.EnglishTitle,
                    EnglishTitle = Dto.EnglishTitle,
                    IsDeleted = false
                });
                _categoryRepository.Commit();
                await _loggerHelper.AddLog(HttpContext, Dto);
                return Ok("The category has been added Successfully.");

            }
            catch (Exception)
            {
                await _loggerHelper.AddLog(HttpContext, Dto);
                return BadRequest("Something went wrong.");
            }
        }

        //[HttpPost("UpdateCategory")]
        [HttpPost]
        public async Task<ActionResult> UpdateCategory(CategoryDTO Dto)
        {
            try
            {
                if (Dto.CategoryID != Guid.Empty)
                {
                    var categories = _categoryRepository.FindByCondition(x => x.ID == Dto.CategoryID && x.IsDeleted == false);
                    if (categories is not null)
                    {
                        categories.ArabicTitle = Dto.EnglishTitle;
                        categories.EnglishTitle = Dto.EnglishTitle;
                        _categoryRepository.Update(categories);
                        _categoryRepository.Commit();
                        await _loggerHelper.AddLog(HttpContext, Dto);
                        return Ok("Updated Successfully");
                    }
                }
                await _loggerHelper.AddLog(HttpContext, Dto);
                return BadRequest("Something went wrong.");
            }
            catch (Exception)
            {
                await _loggerHelper.AddLog(HttpContext, Dto);
                return BadRequest("Something went wrong.");
            }
        }

        //[HttpPost("DeleteCategory")]
        [HttpPost]
        public async Task<ActionResult> DeleteCategory(CategoryDTO Dto)
        {
            try
            {
                if (Dto.CategoryID != Guid.Empty)
                {
                    var categories = _categoryRepository.FindByCondition(x => x.ID == Dto.CategoryID && x.IsDeleted == false);
                    categories.IsDeleted = true;
                    _categoryRepository.Update(categories);
                    _categoryRepository.Commit();
                    await _loggerHelper.AddLog(HttpContext, Dto);
                    return Ok("Deleted Successfully");
                }
                await _loggerHelper.AddLog(HttpContext, Dto);
                return BadRequest("Something went wrong.");
            }
            catch (Exception)
            {
                await _loggerHelper.AddLog(HttpContext, Dto);
                return BadRequest("Something went wrong.");
            }
        }
    }
}
