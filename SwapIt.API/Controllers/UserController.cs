using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices.Identity;

namespace SwapIt.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Ctor
        public UserController(IUserService userService)
        {
            _userService = userService;
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

        #endregion
    }
}
