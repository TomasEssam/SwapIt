using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.BL.Services;
using SwapIt.Data.Constants;

namespace SwapIt.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        #region fields & ctor

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #endregion
        
        [Authorize(Roles = RolesNames.SuperAdmin + "," + RolesNames.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));

                bool success = await _categoryService.CreateAsync(dto);

                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return new StatusCodeResult(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RolesNames.SuperAdmin + "," + RolesNames.Admin)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int categoryId)
        {
            try
            {
                bool success = await _categoryService.DeleteAsync(categoryId);
                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpGet("CategoryDropDown")]
        public async Task<IActionResult> GetCategoryDropDown()
        {
            return Ok(await _categoryService.GetCategoryDDAsync());
        }

    }
}
