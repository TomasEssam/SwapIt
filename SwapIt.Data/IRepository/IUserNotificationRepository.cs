using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.IRepository
{
    public interface IUserNotificationRepository
    {
        Task<UserNotification> GetByIdAsync(int id);
        Task<IEnumerable<UserNotification>> GetAllAsync();
        Task<bool> AddAsync(UserNotification userNotification);
        Task<bool> UpdateAsync(UserNotification userNotification);
        Task<bool> DeleteAsync(UserNotification userNotification);
        Task<bool> DeleteByIdAsync(int id);
    }
}
