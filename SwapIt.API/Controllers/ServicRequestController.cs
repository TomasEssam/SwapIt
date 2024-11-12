using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices;
using SwapIt.BL.IServices.Identity;
using SwapIt.BL.Services;
using SwapIt.Data.Constants;
using SwapIt.Data.Helpers;

namespace SwapIt.API.Controllers
{
    [Route("api/serviceRequests")]
    [ApiController]
    [Authorize]
    public class ServicRequestController : ControllerBase
    {
        #region Fields & CTOR

        private readonly IServiceRequestService _serviceRequestService;
        private readonly IServiceService _serviceService;
        private readonly IUserService _userService;



        public ServicRequestController(IServiceRequestService serviceRequestService, IServiceService serviceService, IUserService userService)
        {
            _serviceRequestService = serviceRequestService;
            _serviceService = serviceService;
            _userService = userService;
        }
        #endregion


        #region User View


        #region Service Provider page
        [HttpGet("serviceProvider/Accepted")]
        [Authorize(Roles =RolesNames.Admin+","+RolesNames.SuperAdmin+","+RolesNames.ServiceProvider)]
        public async Task<IActionResult> GetAllAcceptedServiceProvider([FromQuery] int? serviceProviderId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                if (roles.Contains(RolesNames.ServiceProvider))
                    serviceProviderId = AppSecurityContext.UserId;

                return Ok(await _serviceService.GetAllAcceptedServiceProviderSideAsync((int)serviceProviderId!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin + "," + RolesNames.ServiceProvider)]
        [HttpGet("serviceProvider/finished")]
        public async Task<IActionResult> GetAllFinishedServiceProvider([FromQuery] int? serviceProviderId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                if (roles.Contains(RolesNames.ServiceProvider))
                    serviceProviderId = AppSecurityContext.UserId;

                return Ok(await _serviceService.GetAllFinshiedServiceProviderSideAsync((int)serviceProviderId!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin + "," + RolesNames.ServiceProvider)]
        [HttpGet("serviceProvider/pending")]
        public async Task<IActionResult> GetAllPendingServiceProvider([FromQuery] int? serviceProviderId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                if (roles.Contains(RolesNames.ServiceProvider))
                    serviceProviderId = AppSecurityContext.UserId;

                return Ok(await _serviceService.GetAllPendingServiceProviderSideAsync((int)serviceProviderId!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin + "," + RolesNames.ServiceProvider)]
        [HttpGet("serviceProvider/cancelled")]
        public async Task<IActionResult> GetAllCanceledServiceProvider([FromQuery] int? serviceProviderId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                if (roles.Contains(RolesNames.ServiceProvider))
                    serviceProviderId = AppSecurityContext.UserId;

                return Ok(await _serviceService.GetAllCanceledServiceProviderSideAsync((int)serviceProviderId!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Customer page
        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin + "," + RolesNames.ServiceConsumer)]
        [HttpGet("customer/pending")]
        public async Task<IActionResult> GetAllPendingCustomer([FromQuery] int? customerId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                if (roles.Contains(RolesNames.ServiceConsumer))
                    customerId = AppSecurityContext.UserId;

                return Ok(await _serviceService.GetAllPendingCustomerSideAsync((int)customerId!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin + "," + RolesNames.ServiceConsumer)]
        [HttpGet("customer/accepted")]
        public async Task<IActionResult> GetAllAcceptedCustomer([FromQuery] int? customerId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                if (roles.Contains(RolesNames.ServiceConsumer))
                    customerId = AppSecurityContext.UserId;

                return Ok(await _serviceService.GetAllAcceptedCustomerSideAsync((int)customerId!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin + "," + RolesNames.ServiceConsumer)]
        [HttpGet("customer/cancelled")]
        public async Task<IActionResult> GetAllCanceledCustomer([FromQuery] int? customerId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                if (roles.Contains(RolesNames.ServiceConsumer))
                    customerId = AppSecurityContext.UserId;

                return Ok(await _serviceService.GetAllCanceledCustomerSideAsync((int)customerId!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin + "," + RolesNames.ServiceConsumer)]
        [HttpGet("customer/finished")]
        public async Task<IActionResult> GetAllFinishedCustomer([FromQuery] int? customerId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                if (roles.Contains(RolesNames.ServiceConsumer))
                    customerId = AppSecurityContext.UserId;

                return Ok(await _serviceService.GetAllFinishiedCustomerSideAsync((int)customerId!));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion



        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin + "," + RolesNames.ServiceProvider)]
        [HttpPut("Accept/{serviceRequestId:int}")]
        public async Task<IActionResult> AcceptServiceRequestAsync(int serviceRequestId)
        {
            try
            {
                bool success = await _serviceRequestService.AcceptServiceRequestAsync(serviceRequestId,AppSecurityContext.UserId);
                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return new StatusCodeResult(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        #region Customer View
        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.SuperAdmin + "," + RolesNames.ServiceConsumer)]
        [HttpPut("Finish/{serviceRequestId:int}")]
        public async Task<IActionResult> FinishServiceRequestAsync(int serviceRequestId)
        {
            try
            {
                bool success = await _serviceRequestService.FinishServiceRequestAsync(serviceRequestId);

                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion


        [HttpPut("Cancel/{serviceRequestId:int}")]
        public async Task<IActionResult> CancelServiceRequestAsync([FromQuery] int userId, int serviceRequestId)
        {
            try
            {
                bool success = await _serviceRequestService.CancelServiceRequestAsync(userId, serviceRequestId);

                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return new StatusCodeResult(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ServiceRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));

                bool success = await _serviceRequestService.CreateAsync(dto);

                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return new StatusCodeResult(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetById/{requestId:int}")]
        public async Task<IActionResult> GetByIdAsync(int requestId)
        {
            try
            {
                var request = await _serviceRequestService.GetByIdAsync(requestId);
                if (request is null)
                    return BadRequest("no request with This Id");

                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(ServiceRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                bool success = await _serviceRequestService.UpdateAsync(dto);
                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Admin View
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int serviceRequestId)
        {
            try
            {
                bool success = await _serviceRequestService.DeleteAsync(serviceRequestId);

                if (!success)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);

                return new StatusCodeResult(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #endregion

        #region Super Admin View

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _serviceRequestService.GetAllAsync());
        }
        #endregion

    }
}
