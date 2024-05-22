using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.IServices;



namespace SwapIt.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    [AllowAnonymous]
    public class PaymentController : ControllerBase
    {
        private readonly IUserBalanceService _userBalanceService;

        public PaymentController(IUserBalanceService userBalanceService)
        {
            _userBalanceService = userBalanceService;
        }
        
        [HttpPost]
        [Route("Deposite")]
        public async Task<IActionResult> Deposite(int userId, int points)
        {
            return Ok(await _userBalanceService.Deposite(userId, points));
        }
        [HttpPost]
        [Route("Withdraw")]
        public async Task<IActionResult> Withdraw(int userId, int points)
        {
            return Ok(await _userBalanceService.SubstractPointsAsync(userId,points));
        }
    }
}
