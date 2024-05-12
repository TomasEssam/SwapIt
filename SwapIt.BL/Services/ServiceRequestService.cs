using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IMapper _mapper;
        readonly IServiceRequestRepository _serviceRequestRepository;
        readonly IServiceRepository _serviceRepository;
        readonly IPointsLoggerRepository _pointsLoggerRepository;
        readonly UserManager<ApplicationUser> _userManager;
        readonly IUserBalanceRepository _UserBalanceRepository;

        public ServiceRequestService(IMapper mapper, IServiceRequestRepository serviceRequestRepository, IPointsLoggerRepository pointsLoggerRepository, IServiceRepository serviceRepository, UserManager<ApplicationUser> userManager, IUserBalanceRepository userBalanceRepository)
        {
            _mapper = mapper;
            _serviceRequestRepository = serviceRequestRepository;
            _pointsLoggerRepository = pointsLoggerRepository;
            _serviceRepository = serviceRepository;
            _userManager = userManager;
            _UserBalanceRepository = userBalanceRepository;
        }
        [Authorize(Roles =RolesNames.Admin+ ","+RolesNames.ServiceProvider)]
        public async Task<bool> AcceptServiceRequestAsync(int ServiceRequestId)
        {
            var serviceRequest = await _serviceRequestRepository.GetByIdAsync(ServiceRequestId);
            
            if(serviceRequest is null)
                return false;

            if (serviceRequest.RequestState != RequestStateNames.Pending)
                return false;

            serviceRequest.RequestState = RequestStateNames.Accepted;

            return true;

        }


        public async Task<bool> CancelServiceRequestAsync(int ServiceRequestId)
        {
            throw new NotImplementedException();
        }



        public async Task<bool> CreateAsync(ServiceRequestDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(dto.ServiceId);

            if (service is null)
                return false;

            var user = await _userManager.FindByIdAsync(dto.CustomerId.ToString());

            if (user is null)
                return false;
            
            //var userBalance = await _UserBalanceRepository.GetByIdAsync(user.user;


            //if(_userBalanceService(UserBalance))

            ////track points
            //var pointsLogger = new PointsLogger();
            //pointsLogger.Amount = points;
            //pointsLogger.Type = TransactionTypes.Deposit;
            //pointsLogger.UserId = dto.UserId;
            //await _pointsLoggerRepository.AddAsync(pointsLogger);


            dto.RequestState = RequestStateNames.Pending;
            
            await _serviceRequestRepository.AddAsync(_mapper.Map<ServiceRequest>(dto));
            return true;
        }

        public async Task<bool> DeleteAsync(int serviceRequestId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceRequestDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceRequestDto> UpdateAsync(ServiceRequestDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
