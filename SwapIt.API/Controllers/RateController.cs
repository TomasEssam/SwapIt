using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.BL.Services;

namespace SwapIt.API.Controllers
{
    [Route("api/rates")]
    [ApiController]
    [AllowAnonymous]
    public class RateController : ControllerBase
    {
        private readonly IRateService _rateService;

        public RateController(IRateService rateService)
        {
            _rateService = rateService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));

            bool success = await _rateService.CreateAsync(dto);

            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return NoContent();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int rateId)
        {


            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));

            bool success = await _rateService.DeleteAsync(rateId);

            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _rateService.GetAllAsync());
        }

        [HttpGet]
        [Route("GetByService/{serviceId:int}")]
        public async Task<IActionResult> GetByService(int serviceId)
        {
            return Ok(await _rateService.GetByServiceIdAsync(serviceId));
        }
    }
}
