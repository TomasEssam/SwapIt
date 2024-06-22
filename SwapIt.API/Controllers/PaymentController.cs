﻿using Microsoft.AspNetCore.Authorization;
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
        #region fields & ctor
        private readonly IUserBalanceService _userBalanceService;

        public PaymentController(IUserBalanceService userBalanceService)
        {
            _userBalanceService = userBalanceService;
        }
        #endregion


        [HttpPost("Deposite")]
        public async Task<IActionResult> Deposite(int userId, int points)
        {
            bool success = await _userBalanceService.Deposite(userId, points);

            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            
            return NoContent();
        }

        [HttpPost("Withdraw")]
        public async Task<IActionResult> Withdraw(int userId, int points)
        {
            bool success = await _userBalanceService.SubstractPointsAsync(userId, points);

            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return NoContent();
        }

        [HttpGet("MyPoints")]
        public async Task<IActionResult> GetMyPoints(int userId)
        {
            return Ok(await _userBalanceService.GetMyPoints(userId));
        }
    }
}
