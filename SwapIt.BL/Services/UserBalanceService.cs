using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.IRepository;
using SwapIt.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    public class UserBalanceService : IUserBalanceService
    {
        readonly IMapper _mapper;
        readonly IUserBalanceRepository _userBalanceRepository;
        readonly IPointsLoggerRepository _pointsLoggerRepository;
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserBalanceService(IMapper mapper, IUserBalanceRepository userBalanceRepository, IPointsLoggerRepository pointsLoggerRepository, INotificationService notificationService, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userBalanceRepository = userBalanceRepository;
            _pointsLoggerRepository = pointsLoggerRepository;
            _notificationService = notificationService;
            _userManager = userManager;
        }
        public async Task<bool> AddPointsAsync(int userId, int points)
        {
            return await _userBalanceRepository.AddPointsAsync(userId, points);
        }

        public async Task<bool> SubstractPointsAsync(int userId, int points)
        {
            return await _userBalanceRepository.SubstractPointsAsync(userId, points);
        }


        public async Task<bool> Deposite(int userId, int points)
        {
            bool successed = await _userBalanceRepository.AddPointsAsync(userId, points);

            if (!successed)
                throw new Exception("Could not add Points to account");

            PointsLogger logger = new PointsLogger()
            {
                UserId = userId,
                Points = points,
                Type = TransactionTypes.Deposit
            };
            successed = await _pointsLoggerRepository.AddAsync(logger);
            if (!successed)
                throw new Exception("Could not save logging info");

            //create notification 
            var user = await _userManager.FindByIdAsync(userId.ToString());


            successed = await _notificationService.CreateAsync(new NotificationDto()
            {
                Content = $"{user.UserName} has added {points}",
                NotificationType = NotificationTypes.Deposite
            }, userId);

            if(!successed)
                return false;

            return true;
        }
        public async Task<bool> Withdraw(int userId, int points)
        {
            bool successed = await _userBalanceRepository.SubstractPointsAsync(userId, points);

            if (!successed)
                throw new Exception("Could not add Points to account");

            PointsLogger logger = new PointsLogger()
            {
                UserId = userId,
                Points = points,
                Type = TransactionTypes.Withdraw
            };
            successed = await _pointsLoggerRepository.AddAsync(logger);
            if (!successed)
                throw new Exception("Could not save logging info");

            return true;
        }

        public async Task<int> GetMyPoints(int userId)
        {
            var userBalance = await _userBalanceRepository.GetByUserIdAsync(userId);
            return userBalance.Points;
        }
    }
}
