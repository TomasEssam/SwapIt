﻿using Microsoft.AspNetCore.Authorization;
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
        [HttpGet]
        [Route("AcceptServiceRequestAsync")]
        public async Task<IActionResult> AcceptServiceRequestAsync(int ServiceRequestId)
        {
            return Ok(await _serviceRequestService.AcceptServiceRequestAsync(ServiceRequestId));
        }

        [HttpGet]
        [Route("CancelServiceRequestAsync")]
        public async Task<IActionResult> CancelServiceRequestAsync(int userId, int ServiceRequestId)
        {
            return Ok(await _serviceRequestService.CancelServiceRequestAsync(userId, ServiceRequestId));
        }

        [HttpPost]
        [Route("CreateAsync")]
        public async Task<IActionResult> CreateAsync(ServiceRequestDto dto)
        {
            return Ok(await _serviceRequestService.CreateAsync(dto));
        }

        [HttpGet]
        [Route("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync(int serviceRequestId)
        {
            return Ok(await _serviceRequestService.DeleteAsync(serviceRequestId));
        }

        [HttpGet]
        [Route("GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _serviceRequestService.GetByIdAsync(id));
        }
        [HttpPost]
        [Route("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync(ServiceRequestDto dto)
        {
            return Ok(await _serviceRequestService.UpdateAsync(dto));
        }
        #endregion
    }
}
