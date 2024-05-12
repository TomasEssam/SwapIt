using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
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


        public async Task<bool> CancelServiceRequestAsync(int userId,  int ServiceRequestId)
        {
            //case state>pending =======> return money
            //case state=>accepted customer ==>return money - commession
            //case state=>accepted provider ==> return money 
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if(user is null) 
                return false;

            var serviceRequest = await _serviceRequestRepository.GetByIdAsync(ServiceRequestId);
           
            if (serviceRequest is null)
                return false;

            if (serviceRequest.RequestState == RequestStateNames.Pending)
            {
                
                serviceRequest.RequestState = RequestStateNames.Canceled;
                //return money
                var userBalance = await _UserBalanceRepository.GetByUserAsync(user);


                //_UserBalanceRepository.AddPointsAsync()

                //add notification

            }
            throw new Exception();
        }



        public async Task<bool> CreateAsync(ServiceRequestDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(dto.ServiceId);

            if (service is null)
                return false;

            var user = await _userManager.FindByIdAsync(dto.CustomerId.ToString());

            if (user is null)
                return false;

            bool substracted = await _UserBalanceRepository.SubstractPointsAsync(await _UserBalanceRepository.GetByUserAsync(user), (int)service.Price);
            if(!substracted) 
                return false;


            //track points transaction
            var pointsLogger = new PointsLogger();
            pointsLogger.Amount = service.Price;
            pointsLogger.Type = TransactionTypes.Hold;
            pointsLogger.UserId = user.Id;
            pointsLogger.ServiceRequestId = dto.ServiceRequestId; 
            await _pointsLoggerRepository.AddAsync(pointsLogger);


            dto.RequestState = RequestStateNames.Pending;
            return await _serviceRequestRepository.AddAsync(_mapper.Map<ServiceRequest>(dto));
        }

        public async Task<bool> DeleteAsync(int serviceRequestId)
        {
            return await _serviceRequestRepository.DeleteByIdAsync(serviceRequestId);
        }

        public async Task<ServiceRequestDto> GetByIdAsync(int id)
        {
            return  _mapper.Map<ServiceRequestDto>(await _serviceRequestRepository.GetByIdAsync(id));
        }

        public async Task<bool> UpdateAsync(ServiceRequestDto dto)
        {
            return await _serviceRequestRepository.UpdateAsync(_mapper.Map<ServiceRequest>(dto));

        }
    }
}
