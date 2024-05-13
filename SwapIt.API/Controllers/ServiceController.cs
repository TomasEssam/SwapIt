using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices;

namespace SwapIt.API.Controllers
{
    [Route("api/services")]
    [ApiController]
    [AllowAnonymous]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] ServiceDto dto)
        {
            return Ok(await _serviceService.CreateAsync(dto));
        }


        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery]int serviceId)
        {
            return Ok(await _serviceService.DeleteAsync(serviceId));
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _serviceService.GetAllAsync());
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search([FromBody] ServiceFilterDto serviceFilterDto )
        {
            return Ok(await _serviceService.SearchServiceAsync(serviceFilterDto));
        }

        [HttpGet]
        [Route("GetAllAccepted")]
        public async Task<IActionResult> GetAllAccepted([FromQuery] int userId)
        {
            return Ok(await _serviceService.GetAllAcceptedAsync(userId));
        }

        [HttpGet]
        [Route("GetAllFinished")]
        public async Task<IActionResult> GetAllFinished([FromQuery] int userId)
        {
            return Ok(await _serviceService.GetAllFinshiedAsync(userId));
        }


    }
}
