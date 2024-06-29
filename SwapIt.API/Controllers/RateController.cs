using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.BL.IServices.Identity;
using SwapIt.BL.Services;
using SwapIt.Data.Constants;
using SwapIt.Data.Helpers;

namespace SwapIt.API.Controllers
{
    [Route("api/rates")]
    [ApiController]
    [Authorize]
    public class RateController : ControllerBase
    {
        #region fields and ctor
        private readonly IRateService _rateService;
        private readonly IUserService _userService;

        public RateController(IRateService rateService, IUserService userService)
        {
            _rateService = rateService;
            _userService = userService;
        }

        #endregion

        #region User View
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));

                dto.CustomerId = AppSecurityContext.UserId;

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
                var roles = await _userService.GetUserRole(AppSecurityContext.UserId);
                var rate = await _rateService.GetByIdAsync(rateId);

                if (!roles.Contains(RolesNames.SuperAdmin) && !roles.Contains(RolesNames.Admin))
                    if (AppSecurityContext.UserId != rate?.CustomerId)
                        return Unauthorized();

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

        #endregion

        #region Admin View

        #endregion

        #region Super Admin View

        [Authorize(Roles = RolesNames.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _rateService.GetAllAsync());
        }

        #endregion
    }
}
