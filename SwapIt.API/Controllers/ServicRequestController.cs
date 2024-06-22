using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices;
using SwapIt.BL.IServices.Identity;
using SwapIt.BL.Services;

namespace SwapIt.API.Controllers
{
    [Route("api/serviceRequests")]
    [ApiController]
    [AllowAnonymous]
    public class ServicRequestController : ControllerBase
    {
        #region Fields

        private readonly IServiceRequestService _serviceRequestService;

        #endregion

        #region Ctor

        public ServicRequestController(IServiceRequestService serviceRequestService)
        {
            _serviceRequestService = serviceRequestService;
        }
        #endregion

        #region Actions
        [HttpPut("Accept/{serviceRequestId:int}")]
        public async Task<IActionResult> AcceptServiceRequestAsync(int serviceRequestId)
        {
            bool success = await _serviceRequestService.AcceptServiceRequestAsync(serviceRequestId);

            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        [HttpPut("Cancel/{serviceRequestId:int}")]
        public async Task<IActionResult> CancelServiceRequestAsync([FromQuery]int userId, int serviceRequestId)
        {
            bool success = await _serviceRequestService.CancelServiceRequestAsync(userId, serviceRequestId);

            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        [HttpPut("Finish/{serviceRequestId:int}")]
        public async Task<IActionResult> FinishServiceRequestAsync( int serviceRequestId)
        {
            bool success = await _serviceRequestService.FinishServiceRequestAsync(serviceRequestId);

            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]ServiceRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));

            bool success = await _serviceRequestService.CreateAsync(dto);

            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int serviceRequestId)
        {

            bool success = await _serviceRequestService.DeleteAsync(serviceRequestId);

            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        [HttpGet("GetById/{requestId:int}")]
        public async Task<IActionResult> GetByIdAsync(int requestId)
        {
            var request = await _serviceRequestService.GetByIdAsync(requestId);
            if (request is null)
                return BadRequest("no request with This Id");

            return Ok(request);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(ServiceRequestDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(v=>v.Errors).Select(e => e.ErrorMessage));
            bool success = await _serviceRequestService.UpdateAsync(dto);
            if (!success)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _serviceRequestService.GetAllAsync());
        }

        #endregion
    }
}
