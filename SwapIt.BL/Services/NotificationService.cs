using AutoMapper;
using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using SwapIt.Data.Entities;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IMapper _mapper;
        public NotificationService(INotificationRepository notificationRepository, IUserNotificationRepository userNotificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(NotificationDto dto, int userId)
        {
            //Create general notification using Notification obj
            var notification = _mapper.Map<Notification>(dto);
            bool added = await _notificationRepository.AddAsync(notification);

            if (!added)
                throw new Exception("Could Not save the notification");

            //attach notification to user
            UserNotification userNotification = new UserNotification()
            {
                ApplicationUserId = userId,
                NotificationId = notification.Id,
                IsRead = false

            };
            added = await _userNotificationRepository.AddAsync(userNotification);

            if (!added)
                throw new Exception("Could Not attach the notification to the user");

            return true;
        }

        public async Task<bool> DeleteAsync(int notificationId)
        {
            //delete usernotification that has general notification
            var userN = await _userNotificationRepository.GetAllAsync();
            var relatedUserNotifications = userN.Where(un => un.NotificationId == notificationId);


            foreach (var n in relatedUserNotifications)
            {
                await _userNotificationRepository.DeleteAsync(n);
            }

            //delete general notification
            return await _notificationRepository.DeleteByIdAsync(notificationId);
        }

        public async Task<List<UserNotificationDto>> GetByUserId(int userId)
        {
            var result = new List<UserNotificationDto>();


            //get notifications related to user
            var notifications = await _userNotificationRepository.GetAllAsync();
            var notificationsForUser = notifications.Where(u => u.ApplicationUserId == userId);

            //fill result list
            var relatedNotification = new Notification();
            foreach (UserNotification n in notificationsForUser)
            {
                relatedNotification = await _notificationRepository.GetByIdAsync(n.NotificationId);

                result.Add(new UserNotificationDto()
                {
                    UserNotificationId = n.Id,
                    ApplicationUserId = userId,
                    Content = relatedNotification.Content,
                    IsRead = n.IsRead,
                    NotificationType = relatedNotification.NotificationType
                });
            }
            return result;
        }
        public async Task<List<UserNotificationDto>> GetAllAsync()
        {
            var notifications = await _userNotificationRepository.GetAllAsync();
            return _mapper.Map<List<UserNotificationDto>>(notifications);
        }

        public async Task<bool> ReadNotification(int userNotificationId)
        {
            var userNotification = await _userNotificationRepository.GetByIdAsync(userNotificationId);
            userNotification.IsRead = true;
            return await _userNotificationRepository.UpdateAsync(userNotification);
        }

        public async Task<bool> UpdateAsync(NotificationDto dto)
        {
            return await _notificationRepository.UpdateAsync(_mapper.Map<Notification>(dto));
        }

        public async Task<UserNotification> getById(int userNotificationId)
        {
            return await _userNotificationRepository.GetByIdAsync(userNotificationId);
        }
    }
}
