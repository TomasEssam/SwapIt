using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices;
using SwapIt.Data.Constants;
using SwapIt.Data.Helpers;

namespace SwapIt.API.Controllers
{
    [Route("api/services")]
    [ApiController]
    [AllowAnonymous]
    public class ServiceController : ControllerBase
    {
        #region fileds & ctor
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ServiceDto dto)
        {
            try
            {
                if(!ModelState.IsValid) 
                    return BadRequest(ModelState.Values.SelectMany(v=>v.Errors).Select(e=>e.ErrorMessage));

                bool success = await _serviceService.CreateAsync(dto);
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
        public async Task<IActionResult> Delete([FromQuery] int serviceId)
        {
            try
            {
                bool success = await _serviceService.DeleteAsync(serviceId);
                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //string s = AppSecurityContext.UserName;
            try
            {
                return Ok(await _serviceService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] ServiceFilterDto serviceFilterDto)
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                return Ok(await _serviceService.SearchServiceAsync(serviceFilterDto));
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #region serviceProvider view endPoints
        [HttpGet("serviceProvider/Accepted/{serviceProviderId:int}")]
        public async Task<IActionResult> GetAllAcceptedServiceProvider( int serviceProviderId)
        {
            return Ok(await _serviceService.GetAllAcceptedServiceProviderSideAsync(serviceProviderId));
        }

        [HttpGet("serviceProvider/finished/{serviceProviderId:int}")]
        public async Task<IActionResult> GetAllFinishedServiceProvider(int serviceProviderId)
        { 
            return Ok(await _serviceService.GetAllFinshiedServiceProviderSideAsync(serviceProviderId));
        }

        [HttpGet("serviceProvider/pending/{serviceProviderId:int}")]
        public async Task<IActionResult> GetAllPendingServiceProvider(int serviceProviderId)
        {
            return Ok(await _serviceService.GetAllPendingServiceProviderSideAsync(serviceProviderId));
        }

        [HttpGet("serviceProvider/cancelled/{serviceProviderId:int}")]
        public async Task<IActionResult> GetAllCanceledServiceProvider(int serviceProviderId)
        {
            return Ok(await _serviceService.GetAllCanceledServiceProviderSideAsync(serviceProviderId));
        }
        #endregion

        #region Customers view endPoints
        [HttpGet("customer/pending/{customerId:int}")]
        public async Task<IActionResult> GetAllPendingCustomer(int customerId)
        {
            return Ok(await _serviceService.GetAllPendingCustomerSideAsync(customerId));
        }

        [HttpGet("customer/accepted/{customerId:int}")]
        public async Task<IActionResult> GetAllAcceptedCustomer( int customerId)
        {
            return Ok(await _serviceService.GetAllAcceptedCustomerSideAsync(customerId));
        }

        [HttpGet("customer/cancelled/{customerId:int}")]
        public async Task<IActionResult> GetAllCanceledCustomer(int customerId)
        {
            return Ok(await _serviceService.GetAllCanceledCustomerSideAsync(customerId));
        }

        [HttpGet("customer/finished/{customerId:int}")]
        public async Task<IActionResult> GetAllFinishedCustomer(int customerId)
        {
            return Ok(await _serviceService.GetAllFinishiedCustomerSideAsync(customerId));
        }
        #endregion

        /*
        //[HttpPost]
        //[Route("UploadServiceImage")]
        //public async Task<IActionResult> UploadServiceImage([FromForm] IFormFile image, [FromForm] int serviceId)
        //{
        //    try
        //    {
        //        return Ok(await _serviceService.UploadServiceImage(image, serviceId, FolderName.servicesImages));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("GetServiceImage")]
        //public async Task<IActionResult> GetServiceImage(int serviceId)
        //{
        //    var result = await _serviceService.GetServiceImage(serviceId);

        //    if (!result.Success)
        //    {
        //        return NotFound(result.ErrorMessage);
        //    }

        //    return File(result.ImageStream, result.ContentType);
        //}
        */
    }
}
