using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.IServices;



namespace SwapIt.API.Controllers
{
    [Route("api/[controller]")]
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
            var userBalance =await _userBalanceService.GetByUser(userId);
            return Ok(await _userBalanceService.AddPointsAsync(userBalance, points));
        }
        [HttpPost]
        [Route("Withdraw")]
        public async Task<IActionResult> Withdraw(int userId, int points)
        {
            var userBalance = await _userBalanceService.GetByUser(userId);
            return Ok(await _userBalanceService.SubstractPointsAsync(userBalance, points));
        }
    }
}
