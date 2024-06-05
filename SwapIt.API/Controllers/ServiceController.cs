using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Delete([FromQuery] int serviceId)
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
        public async Task<IActionResult> Search([FromBody] ServiceFilterDto serviceFilterDto)
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _serviceService.SearchServiceAsync(serviceFilterDto));
        }

        [HttpGet]
        [Route("GetAllAcceptedServiceProvider")]
        public async Task<IActionResult> GetAllAcceptedServiceProvider([FromQuery] int serviceProviderId)
        {
            return Ok(await _serviceService.GetAllAcceptedServiceProviderSideAsync(serviceProviderId));
        }

        [HttpGet]
        [Route("GetAllFinishedServiceProvider")]
        public async Task<IActionResult> GetAllFinishedServiceProvider([FromQuery] int serviceProviderId)
        {
            return Ok(await _serviceService.GetAllFinshiedServiceProviderSideAsync(serviceProviderId));
        }

        [HttpGet]
        [Route("GetAllPendingServiceProvider")]
        public async Task<IActionResult> GetAllPendingServiceProvider([FromQuery] int serviceProviderId)
        {
            return Ok(await _serviceService.GetAllPendingServiceProviderSideAsync(serviceProviderId));
        }

        [HttpGet]
        [Route("GetAllCanceledServiceProvider")]
        public async Task<IActionResult> GetAllCanceledServiceProvider([FromQuery] int serviceProviderId)
        {
            return Ok(await _serviceService.GetAllCanceledServiceProviderSideAsync(serviceProviderId));
        }

        [HttpGet]
        [Route("GetAllPendingCustomer")]
        public async Task<IActionResult> GetAllPendingCustomer([FromQuery] int customerId)
        {
            return Ok(await _serviceService.GetAllPendingCustomerSideAsync(customerId));
        }

        [HttpGet]
        [Route("GetAllAcceptedCustomer")]
        public async Task<IActionResult> GetAllAcceptedCustomer([FromQuery] int customerId)
        {
            return Ok(await _serviceService.GetAllAcceptedCustomerSideAsync(customerId));
        }

        [HttpGet]
        [Route("GetAllCanceledCustomer")]
        public async Task<IActionResult> GetAllCanceledCustomer([FromQuery] int customerId)
        {
            return Ok(await _serviceService.GetAllCanceledCustomerSideAsync(customerId));
        }

        [HttpGet]
        [Route("GetAllFinishedCustomer")]
        public async Task<IActionResult> GetAllFinishedCustomer([FromQuery] int customerId)
        {
            return Ok(await _serviceService.GetAllFinishiedCustomerSideAsync(customerId));
        }

        [HttpPost]
        [Route("UploadServiceImage")]
        public async Task<IActionResult> UploadServiceImage([FromForm] IFormFile image, [FromForm] int serviceId)
        {

            string folderName = @"servicesImages";
            return Ok(await _serviceService.UploadServiceImage(image, serviceId, folderName));
        }

        [HttpGet]
        [Route("GetServiceImage")]
        public async Task<IActionResult> GetServiceImage(int serviceId)
        {
            var result = await _serviceService.GetServiceImage(serviceId);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return File(result.ImageStream, result.ContentType);
        }
    }
}
