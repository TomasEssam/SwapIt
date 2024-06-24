using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.BL.Services;
using SwapIt.Data.Constants;

namespace SwapIt.API.Controllers
{
    [Route("api/rates")]
    [ApiController]
    [Authorize]
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
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));

                bool success = await _rateService.CreateAsync(dto);

                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int rateId)
        {
            try
            {
                bool success = await _rateService.DeleteAsync(rateId);

                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = RolesNames.SuperAdmin + "," + RolesNames.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _rateService.GetAllAsync());
        }

        [HttpGet]
        [Route("GetByService/{serviceId:int}")]
        public async Task<IActionResult> GetByService(int serviceId)
        {
            try
            {
                return Ok(await _rateService.GetByServiceIdAsync(serviceId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
