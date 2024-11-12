using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices;
using SwapIt.BL.IServices.Identity;
using SwapIt.BL.Services;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities;
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
        private readonly IUserService _userService;

        public ServiceController(IServiceService serviceService, IUserService userService)
        {
            _serviceService = serviceService;
            _userService = userService;
        }
        #endregion

        #region User View

        [Authorize(Roles = RolesNames.ServiceProvider + "," + RolesNames.Admin + "," + RolesNames.SuperAdmin)]
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


        [Authorize(Roles = RolesNames.ServiceProvider + "," + RolesNames.Admin + "," + RolesNames.SuperAdmin)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int serviceId)
        {
            try
            {
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                var service = await _serviceService.GetServiceByIdAsync(serviceId);

                if (!roles.Contains(RolesNames.SuperAdmin) && !roles.Contains(RolesNames.Admin))
                    if (AppSecurityContext.UserId != service?.ServiceProviderId)
                        return Unauthorized();

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

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] ServiceFilterDto serviceFilterDto)
        {
            
            try
            {
                return Ok(await _serviceService.SearchServiceAsync(serviceFilterDto, AppSecurityContext.UserId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion

        #region Admin View
        #endregion

        #region Super Admin View
        [Authorize(Roles = RolesNames.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _serviceService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
