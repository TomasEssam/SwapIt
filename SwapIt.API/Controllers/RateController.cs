using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;

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
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] RateDto dto)
        {
            return Ok(await _rateService.CreateAsync(dto));
        }


        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int rateId)
        {
            return Ok(await _rateService.DeleteAsync(rateId));
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _rateService.GetAllAsync());
        }

        [HttpGet]
        [Route("GetByService")]
        public async Task<IActionResult> GetByService(int serviceId)
        {
            return Ok(await _rateService.GetByServiceIdAsync(serviceId));
        }
    }
}
