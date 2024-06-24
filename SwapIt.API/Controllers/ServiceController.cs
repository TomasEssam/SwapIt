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
    [Authorize]
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
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

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

        [Authorize(Roles = RolesNames.SuperAdmin + "," + RolesNames.Admin)]
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #region serviceProvider view endPoints
        [HttpGet("serviceProvider/Accepted/{serviceProviderId:int}")]
        public async Task<IActionResult> GetAllAcceptedServiceProvider(int serviceProviderId)
        {
            try
            {
                return Ok(await _serviceService.GetAllAcceptedServiceProviderSideAsync(serviceProviderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("serviceProvider/finished/{serviceProviderId:int}")]
        public async Task<IActionResult> GetAllFinishedServiceProvider(int serviceProviderId)
        {
            try
            {
                return Ok(await _serviceService.GetAllFinshiedServiceProviderSideAsync(serviceProviderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("serviceProvider/pending/{serviceProviderId:int}")]
        public async Task<IActionResult> GetAllPendingServiceProvider(int serviceProviderId)
        {
            try
            {
                return Ok(await _serviceService.GetAllPendingServiceProviderSideAsync(serviceProviderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("serviceProvider/cancelled/{serviceProviderId:int}")]
        public async Task<IActionResult> GetAllCanceledServiceProvider(int serviceProviderId)
        {
            try
            {
                return Ok(await _serviceService.GetAllCanceledServiceProviderSideAsync(serviceProviderId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Customers view endPoints
        [HttpGet("customer/pending/{customerId:int}")]
        public async Task<IActionResult> GetAllPendingCustomer(int customerId)
        {
            try
            {
                return Ok(await _serviceService.GetAllPendingCustomerSideAsync(customerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer/accepted/{customerId:int}")]
        public async Task<IActionResult> GetAllAcceptedCustomer(int customerId)
        {
            try
            {


                return Ok(await _serviceService.GetAllAcceptedCustomerSideAsync(customerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer/cancelled/{customerId:int}")]
        public async Task<IActionResult> GetAllCanceledCustomer(int customerId)
        {
            try
            {
                return Ok(await _serviceService.GetAllCanceledCustomerSideAsync(customerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer/finished/{customerId:int}")]
        public async Task<IActionResult> GetAllFinishedCustomer(int customerId)
        {
            try
            {
                return Ok(await _serviceService.GetAllFinishiedCustomerSideAsync(customerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
