using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.IServices;

using SwapIt.Data.Entities;

namespace SwapIt.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/notification")]
    public class NotificationController : Controller
    {
        #region fileds & ctor
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        #endregion


        [HttpPut("read/{userNotificationId:int}")]
        public async Task<IActionResult> ReadNotification(int userNotificationId)
        {
            bool success = await _notificationService.ReadNotification(userNotificationId);
            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return NoContent();
        }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            return Ok(await _notificationService.GetAllAsync(userId));
        }
    }
}
