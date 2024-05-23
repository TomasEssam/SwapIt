using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    public class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceRequestService _serviceRequestService;
        private readonly SwapItDbContext _context;
        private readonly INotificationService _notificationService;
        public RateService(IRateRepository rateRepository, IMapper mapper, UserManager<ApplicationUser> userManager, IServiceRepository serviceRepository, IServiceRequestService serviceRequestService, SwapItDbContext context, INotificationService notificationService)
        {
            _rateRepository = rateRepository;
            _mapper = mapper;
            _userManager = userManager;
            _serviceRepository = serviceRepository;
            _serviceRequestService = serviceRequestService;
            _context = context;
            _notificationService = notificationService;
        }
        public async Task<bool> CreateAsync(RateDto newRate)
        {
            var user = await _userManager.FindByIdAsync(newRate.CustomerId.ToString());
            if (user is null)
                return false;

            var service = await _serviceRepository.GetByIdAsync(newRate.ServiceId);
            if (service is null)
                return false;

            var serviceRequest = await _serviceRequestService.GetAsync(newRate.ServiceId, newRate.CustomerId);
            if (serviceRequest is null)
                return false;
            //for Notification 
            await _notificationService.CreateAsync(new NotificationDto
            {
                Content = $"Your Service {service.Name} has been rated {newRate.RateValue} by {user.UserName}",
                NotificationType = NotificationTypes.Rate
            }, service.ServiceProviderId);

            await _notificationService.CreateAsync(new NotificationDto
            {
                Content = $"You have rated service {service.Name} with rate value {newRate.RateValue}",
                NotificationType = NotificationTypes.Rate
            }, serviceRequest.CustomerId);

            return await _rateRepository.AddAsync(_mapper.Map<Rate>(newRate));
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
        public async Task<List<RateDto>> GetByServiceIdAsync(int serviceId)
        {
            var rates = await _rateRepository.GetAllAsync();
            return _mapper.Map<List<RateDto>>(rates.Where(r => r.ServiceId == serviceId));
        }

        public async Task<int> GetTotalRateForUser(int userId)
        {
            
            var totalRateList = 
            await _context.Services
                .Include(x => x.Rates)
                .Where(x => x.ServiceProviderId == userId)
                .Select(x => new ProfileDto
                {
                   TotalRate  = (x.Rates.Count() == 0) ? 0 : x.Rates.Select(x => x.RateValue).Sum() / x.Rates.Count()
                }).ToListAsync();
            return (totalRateList.Count == 0) ? 0 : (int)totalRateList.Select(x => x.TotalRate).Average();
             
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

    }
}
