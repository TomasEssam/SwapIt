using SwapIt.BL.DTOs;
using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.IServices
{
    public interface INotificationService
    {
        Task<bool> CreateAsync(NotificationDto dto , int userId);
        Task<bool> DeleteAsync(int notificationId);
        Task<bool> UpdateAsync(NotificationDto dto);
        Task<List<UserNotificationDto>> GetByUserId(int userId);
        Task<bool> ReadNotification(int userNotificationId);
        Task<UserNotification> getById(int userNotificationId);
        Task<List<UserNotificationDto>> GetAllAsync();
    }
}
