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
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            return Ok(await _categoryService.CreateAsync(dto));
        }


        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int categoryId)
        {
            return Ok(await _categoryService.DeleteAsync(categoryId));
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpGet]
        [Route("CategoryDropDown")]
        public async Task<IActionResult> GetCategoryDropDownAsync()
        {
            return Ok(await _categoryService.GetCategoryDDAsync());
        }

    }
}
