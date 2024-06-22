using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.BL.Services;

namespace SwapIt.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [AllowAnonymous]
    public class CategoryController : ControllerBase
    {
        #region fields & ctor

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(m=>m.Errors).Select(e=>e.ErrorMessage));

            bool success = await _categoryService.CreateAsync(dto);

            if(!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int categoryId)
        {
            bool success = await _categoryService.DeleteAsync(categoryId);
            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return new StatusCodeResult(StatusCodes.Status204NoContent);
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
