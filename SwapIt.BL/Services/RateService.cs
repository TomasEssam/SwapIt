using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    internal class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceRequestService _serviceRequestService;
        public RateService(IRateRepository rateRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IServiceRepository serviceRepository, IServiceRequestService serviceRequestService)
        {
            _rateRepository = rateRepository;
            _mapper = mapper;
            _userManager = userManager;
            _serviceRepository = serviceRepository;
            _serviceRequestService = serviceRequestService;
        }
        public async Task<bool> CreateAsync(RateDto dto)
        {
           return await _rateRepository.AddAsync(_mapper.Map<Rate>(dto));
        }

        public async Task<bool> DeleteAsync(int rateId)
        {
            return await _rateRepository.DeleteByIdAsync(rateId);
        }

        public async Task<List<RateDto>> GetAllAsync()
        {
            var rates = await _rateRepository.GetAllAsync();
            return _mapper.Map<List<RateDto>>(rates);
        }

        public async Task<RateDto> GetByIdAsync(int rateId)
        {
            var rate = await _rateRepository.GetByIdAsync(rateId);
            return _mapper.Map<RateDto>(rate);
        }

        public async Task<bool> UpdateAsync(RateDto dto)
        {
            return await _rateRepository.UpdateAsync(_mapper.Map<Rate>(dto));
        }

        public async Task<bool> Rate(RateDto newRate)
        {
            var user = await _userManager.FindByIdAsync(newRate.CustomerId.ToString());
            if (user is null)
                return false;

            var service = await _serviceRepository.GetByIdAsync(user.Id);
            if (service is null)
                return false;

            var serviceRequest =  _serviceRequestService.GetAsync(newRate.ServiceId, newRate.CustomerId);
            if (serviceRequest is null)
                return false;

            return await _rateRepository.AddAsync(_mapper.Map<Rate>(newRate));
        }

    }
}
