using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices;
using SwapIt.BL.IServices.Identity;
using SwapIt.BL.Services;
using SwapIt.Data.Constants;

namespace SwapIt.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IServiceService _serviceService;

        #endregion

        #region Ctor
        public UserController(IUserService userService, IServiceService serviceService)
        {
            _userService = userService;
            _serviceService = serviceService;
        }
        #endregion

        #region  Actions

        [HttpPost]
        [Route("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto dto)
        {
            return Ok(await _userService.Authenticate(dto));
        }

        [HttpGet]
        [Route("IsUserNameExists")]
        public async Task<IActionResult> IsUserNameExists([FromQuery] UsernameDto dto)
        {
            return Ok(await _userService.IsUserNameExists(dto.Username, null));
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _userService.ResetPasswordAsync(dto);
            return Ok();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync([FromForm] UserDto dto)
        {
            try
            {
                await _userService.CreateUserAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {
            return Ok(await _userService.GetUserAsync(userId));
        }

        [HttpGet]
        [Route("GetServicesImages")]
        public async Task<IActionResult> GetServicesImages(int userId)
        {
            var imageResults = await _serviceService.GetAllPreviousWorkImageUrlAsync(userId);

            if (imageResults == null || !imageResults.Any())
            {
                return NotFound("No images found for the specified service provider.");
            }

            var base64Images = imageResults
                .Where(r => r.Success)
                .Select(r => new
                {
                    r.Base64Image,
                    r.ContentType
                })
                .ToList();

            return Ok(base64Images);
        }

        [HttpGet]
        [Route("ServiceProvidersDropDown")]
        public async Task<IActionResult> DropDownAsync()
        {
            return Ok(await _userService.DropDownAsync());
        }

        [HttpPost]
        [Route("UploadProfileImage")]
        public async Task<IActionResult> UploadProfileImage( IFormFile image,int userId)
        {
            try
            {
                return Ok(await _userService.UploadProfileImage(image, userId, FolderName.profileImages));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //[Route("UploadIdImage")]
        //public async Task<IActionResult> UploadIdImage([FromForm] IFormFile image, [FromForm] int userId)
        //{
        //    try
        //    {
        //        return Ok(await _userService.UploadIdImage(image, userId, FolderName.IdImages));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet]
        [Route("GetProfileImage")]
        public async Task<IActionResult> GetProfileImage(int userId)
        {
            var result = await _userService.GetProfileImage(userId);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return File(result.ImageStream, result.ContentType);
        }

        [HttpGet]
        [Route("GetIdImage")]
        public async Task<IActionResult> GetIdImage(int userId)
        {
            var result = await _userService.GetIdImage(userId);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return File(result.ImageStream, result.ContentType);
        }

        #endregion
    }
}
