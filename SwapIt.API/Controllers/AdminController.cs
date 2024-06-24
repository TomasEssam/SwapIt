using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.IServices.Identity;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities.Identity;

namespace SwapIt.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = RolesNames.SuperAdmin + "," + RolesNames.Admin)]
    public class AdminController : Controller
    {
        #region fields & ctor
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        #endregion

        [HttpPut("ActivateAccount")]
        public async Task<IActionResult> ActivateAccount(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user is null)
                    return NotFound("user can't be found");

                user.IsActive = true;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
