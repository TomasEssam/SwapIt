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
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet]
        [Route("read")]
        public async Task<IActionResult> ReadNotification(int userNotificationId)
        {
           
            return Ok(await _notificationService.ReadNotification(userNotificationId));
        }
        [HttpGet]
        [Route("getall")]

        public async Task<IActionResult> GetAll(int userId)
        {
            return Ok(await _notificationService.GetAllAsync(userId));
        }
    }
}
