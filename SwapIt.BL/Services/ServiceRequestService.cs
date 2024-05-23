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
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPointsLoggerRepository _pointsLoggerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserBalanceRepository _UserBalanceRepository;
        private readonly SwapItDbContext _context;
        private readonly INotificationService _notificationService;


        public ServiceRequestService(IMapper mapper, IServiceRequestRepository serviceRequestRepository, IPointsLoggerRepository pointsLoggerRepository, IServiceRepository serviceRepository, UserManager<ApplicationUser> userManager, IUserBalanceRepository userBalanceRepository, SwapItDbContext swapItDbContext, INotificationService notificationService)
        {
            _mapper = mapper;
            _serviceRequestRepository = serviceRequestRepository;
            _pointsLoggerRepository = pointsLoggerRepository;
            _serviceRepository = serviceRepository;
            _userManager = userManager;
            _UserBalanceRepository = userBalanceRepository;
            _context = swapItDbContext;
            _notificationService = notificationService;
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

            var customer = await _userManager.FindByIdAsync(serviceRequest.CustomerId.ToString());
            var service = await _serviceRepository.GetByIdAsync(serviceRequest.ServiceId);
            //for Notification
            await _notificationService.CreateAsync(new NotificationDto
            {
                Content = $"You have been accepted a request from user {customer.UserName}.",
                NotificationType = NotificationTypes.AcceptRequest
            }, service.ServiceProviderId);

            await _notificationService.CreateAsync(new NotificationDto
            {
                Content = $"Your request for a service {service.Name} has been accepted.",
                NotificationType = NotificationTypes.AcceptRequest
            }, customer.Id);

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

            var service = await _serviceRepository.GetByIdAsync(serviceRequest.ServiceId);

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

                //for Notification
                await _notificationService.CreateAsync(new NotificationDto
                {
                    Content = $"The request has been canceled",
                    NotificationType = NotificationTypes.RequestCanceled
                }, serviceRequest.CustomerId);

                await _notificationService.CreateAsync(new NotificationDto
                {
                    Content = $"The request has been canceled",
                    NotificationType = NotificationTypes.RequestCanceled
                },service.ServiceProviderId);

            }
            if (serviceRequest.RequestState == RequestStateNames.Accepted)
            {
                serviceRequest.RequestState = RequestStateNames.Canceled;

                var userBalance = await _UserBalanceRepository.GetByUserAsync(user);

                var holdAmount = _context.PointsLoggers
                    .Where(x => x.UserId == user.Id && x.ServiceRequestId == serviceRequest.Id)
                    .FirstOrDefault();

                holdAmount.IsDeleted = true;


                if (userId == service.ServiceProviderId)
                {
                    if (userBalance.Points < 5)
                        return false;

                    userBalance.Points -= 5;

                    //for Notification
                    await _notificationService.CreateAsync(new NotificationDto
                    {
                        Content = $"The request has been canceled",
                        NotificationType = NotificationTypes.RequestCanceled
                    }, serviceRequest.CustomerId);

                    await _notificationService.CreateAsync(new NotificationDto
                    {
                        Content = $"The request has been canceled /n" +
                        $"And Unfortunatly there is 5 points shortage in your account",
                        NotificationType = NotificationTypes.RequestCanceled
                    }, service.ServiceProviderId);
                }
                else
                {
                    userBalance.Points += holdAmount.Points - 5;
                    //for Notification
                    await _notificationService.CreateAsync(new NotificationDto
                    {
                        Content = $"The request has been canceled /n" +
                       $"And your money has been retrivied to your account with shortage 5 points",
                        NotificationType = NotificationTypes.RequestCanceled
                    }, serviceRequest.CustomerId);

                    await _notificationService.CreateAsync(new NotificationDto
                    {
                        Content = $"The request has been canceled",
                        NotificationType = NotificationTypes.RequestCanceled
                    }, service.ServiceProviderId);
                }

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

            bool substracted = await _UserBalanceRepository.SubstractPointsAsync(user.Id, service.Price);
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

            //for Notification
            await _notificationService.CreateAsync(new NotificationDto
            {
                Content = $"your Service {service.Name} has been requested by {user.UserName}",
                NotificationType = NotificationTypes.Request
            }, service.ServiceProviderId);

            await _notificationService.CreateAsync(new NotificationDto
            {
                Content = $"you have been create a request to service {service.Name}/n" +
                $"and the money of the service is in the hold phase waiting for the request to be accepted",
                NotificationType = NotificationTypes.RequestCreated
            }, user.Id);

            return true;
        }

        public async Task<bool> DeleteAsync(int serviceRequestId)
        {
            return await _serviceRequestRepository.DeleteByIdAsync(serviceRequestId);
        }

        public async Task<bool> FinishServiceRequestAsync(int serviceRequestId)
        {
            var serviceRequest = await _serviceRequestRepository.GetByIdAsync(serviceRequestId);

            if (serviceRequest is null)
                return false;

            if (serviceRequest.RequestState == RequestStateNames.Accepted)
            {
                var service = await _serviceRepository.GetByIdAsync(serviceRequest.ServiceId);

                var userBalance = await _UserBalanceRepository.GetByUserIdAsync(service.ServiceProviderId);

                var pointsLogger = await _pointsLoggerRepository.GetByServiceRequestIdAsync(serviceRequestId);

                await _UserBalanceRepository.AddPointsAsync(service.ServiceProviderId, pointsLogger.Points);

                await _pointsLoggerRepository.AddAsync(new PointsLogger()
                {
                    ServiceRequestId = serviceRequestId,
                    UserId = service.ServiceProviderId,
                    Type = TransactionTypes.Transfared,
                    Points = pointsLogger.Points
                });

                serviceRequest.RequestState = RequestStateNames.Finished;
                
                await _serviceRequestRepository.UpdateAsync(serviceRequest);

                var customer = await _userManager.FindByIdAsync(serviceRequest.CustomerId.ToString());
                
                //for Notification
                await _notificationService.CreateAsync(new NotificationDto
                {
                    Content = $"You finished the service {service.Name} for {customer.UserName} /n" +
                    $"and the money has been added successfully to your account.",
                    NotificationType = NotificationTypes.RequestFinished
                }, service.ServiceProviderId);

                await _notificationService.CreateAsync(new NotificationDto
                {
                    Content = $"Your request for the service {service.Name} has been completed successfully.",
                    NotificationType = NotificationTypes.RequestFinished
                }, customer.Id);

                return true;
            }
            return false;
        }

        public async Task<List<ServiceRequestDto>> GetAllAsync()
        {
            var requests = await _serviceRequestRepository.GetAllAsync();
            return  _mapper.Map<List<ServiceRequestDto>>(requests);
        }

        public async Task<ServiceRequest?> GetAsync(int serviceId, int userId)
        {
            var serviceRequest = await _serviceRequestRepository.GetAllAsync();

            return serviceRequest.Where(s=>s.ServiceId==serviceId &&s.CustomerId==userId).FirstOrDefault();
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
