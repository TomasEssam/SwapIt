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
        readonly SwapItDbContext _context;

        public ServiceRequestService(IMapper mapper, IServiceRequestRepository serviceRequestRepository, IPointsLoggerRepository pointsLoggerRepository, IServiceRepository serviceRepository, UserManager<ApplicationUser> userManager, IUserBalanceRepository userBalanceRepository, SwapItDbContext swapItDbContext)
        {
            _mapper = mapper;
            _serviceRequestRepository = serviceRequestRepository;
            _pointsLoggerRepository = pointsLoggerRepository;
            _serviceRepository = serviceRepository;
            _userManager = userManager;
            _UserBalanceRepository = userBalanceRepository;
            _context = swapItDbContext;
        }
        [Authorize(Roles = RolesNames.Admin + "," + RolesNames.ServiceProvider)]
        public async Task<bool> AcceptServiceRequestAsync(int ServiceRequestId)
        {
            var serviceRequest = await _serviceRequestRepository.GetByIdAsync(ServiceRequestId);

            if (serviceRequest is null)
                return false;

            if (serviceRequest.RequestState != RequestStateNames.Pending)
                return false;

            serviceRequest.RequestState = RequestStateNames.Accepted;
            await _serviceRequestRepository.UpdateAsync(serviceRequest);
            return true;

        }


        public async Task<bool> CancelServiceRequestAsync(int userId, int ServiceRequestId)
        {
            //state>pending =======> return money
            //state=>accepted customer ==>return money - commession
            //state=>accepted provider ==> return money 

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return false;

            var serviceRequest = await _serviceRequestRepository.GetByIdAsync(ServiceRequestId);

            if (serviceRequest is null)
                return false;

            if (serviceRequest.RequestState == RequestStateNames.Pending)
            {

                serviceRequest.RequestState = RequestStateNames.Canceled;

                //return money
                var userBalance = await _UserBalanceRepository.GetByUserAsync(user);
                var holdAmount = _context.PointsLoggers
                    .Where(x => x.UserId == user.Id && x.ServiceRequestId == serviceRequest.Id)
                    .FirstOrDefault();

                if (holdAmount is null)
                    return false;

                holdAmount.IsDeleted = true;
                userBalance.Points += holdAmount.Points;
                _context.SaveChanges();
            }
            if (serviceRequest.RequestState == RequestStateNames.Accepted)
            {
                serviceRequest.RequestState = RequestStateNames.Canceled;

                var userBalance = await _UserBalanceRepository.GetByUserAsync(user);

                var holdAmount = _context.PointsLoggers
                    .Where(x => x.UserId == user.Id && x.ServiceRequestId == serviceRequest.Id)
                    .FirstOrDefault();

                holdAmount.IsDeleted = true;

                if (userBalance.Points < 5)
                    return false;

                userBalance.Points += holdAmount.Points - 5;
                //To Do create table for system balance auditing  -- Adding 5 points to our system
                _context.SaveChanges();
            }
            return true;
        }



        public async Task<bool> CreateAsync(ServiceRequestDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(dto.ServiceId);

            if (service is null)
                return false;

            var user = await _userManager.FindByIdAsync(dto.CustomerId.ToString());

            if (user is null)
                return false;

            bool substracted = await _UserBalanceRepository.SubstractPointsAsync(await _UserBalanceRepository.GetByUserAsync(user), service.Price);
            if (!substracted)
                return false;


            dto.RequestState = RequestStateNames.Pending;


            var serviceRequest = await _serviceRequestRepository.AddAndReturnAsync(_mapper.Map<ServiceRequest>(dto));

            //track points transaction
            var pointsLogger = new PointsLogger();
            pointsLogger.Points = service.Price;
            pointsLogger.Type = TransactionTypes.Hold;
            pointsLogger.UserId = user.Id;
            pointsLogger.ServiceRequestId = serviceRequest.Id;
            await _pointsLoggerRepository.AddAsync(pointsLogger);

            return true;
        }

        public async Task<bool> DeleteAsync(int serviceRequestId)
        {
            return await _serviceRequestRepository.DeleteByIdAsync(serviceRequestId);
        }

        public async Task<ServiceRequestDto> GetByIdAsync(int id)
        {
            return _mapper.Map<ServiceRequestDto>(await _serviceRequestRepository.GetByIdAsync(id));
        }

        public async Task<bool> UpdateAsync(ServiceRequestDto dto)
        {
            return await _serviceRequestRepository.UpdateAsync(_mapper.Map<ServiceRequest>(dto));

        }
    }
}
