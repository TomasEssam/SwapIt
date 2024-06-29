using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.IServices;
using SwapIt.BL.IServices.Identity;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities;
using SwapIt.Data.Helpers;
using System.Security;
using System.Security.Permissions;

namespace SwapIt.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/notification")]
    public class NotificationController : Controller
    {
        #region fileds & ctor
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public NotificationController(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }
        #endregion

        #region User View
        [HttpPut("read/{userNotificationId:int}")]
        public async Task<IActionResult> ReadNotification(int userNotificationId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);


                if (!roles.Contains(RolesNames.SuperAdmin) && !roles.Contains(RolesNames.Admin))
                {
                    //make sure that notification is related to user
                    var notification = await _notificationService.getById(userNotificationId);
                    if (notification.ApplicationUserId != AppSecurityContext.UserId)
                        return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }

                bool success = await _notificationService.ReadNotification(userNotificationId);
                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("MyNofications")]
        public async Task<IActionResult> MyNofications()
        {
            try
            {
                return Ok(await _notificationService.GetByUserId(AppSecurityContext.UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Admin View

        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin)]
        [HttpGet("GetByUserId/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);

                if (!roles.Contains(RolesNames.SuperAdmin))
                    if (userId != AppSecurityContext.UserId)
                        return Unauthorized();

                return Ok(await _notificationService.GetByUserId(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Super Admin View

        [Authorize(Roles = RolesNames.SuperAdmin)]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _notificationService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
