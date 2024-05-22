using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices;
using SwapIt.BL.IServices.Identity;
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
        public async Task<IActionResult> CreateAsync([FromBody] UserDto dto)
        {
            await _userService.CreateUserAsync(dto);
            return Ok();
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
            return Ok(await _serviceService.GetAllPreviousWorkImageUrlAsync(userId));
        }

        #endregion
    }
}
