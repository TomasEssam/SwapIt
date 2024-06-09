using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.IServices.Identity;
using SwapIt.Data.Entities.Identity;

namespace SwapIt.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [AllowAnonymous]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        [Route("ActivateAccount")]
        public async Task<IActionResult> ActivateAccount(int userId)
        { 
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return NotFound("user can't be found");

            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("couldn't activate");

            return Ok();
        }
    }
}
