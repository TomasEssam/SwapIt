using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
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
        public NotificationService(INotificationRepository notificationRepository, IUserNotificationRepository userNotificationRepository)
        {
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
        }



        public Task<bool> CreateAsync(NotificationDto dto , int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetByIdAsync(int notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(NotificationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
