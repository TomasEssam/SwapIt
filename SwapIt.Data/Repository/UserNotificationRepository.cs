using SwapIt.Data.Entities.Context;
using SwapIt.Data.Entities;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SwapIt.Data.Repository
{
    public class UserNotificationRepository : IUserNotificationRepository
    {
        private readonly SwapItDbContext Context;

        public UserNotificationRepository(SwapItDbContext swapItDbContext)
        {
            Context = swapItDbContext ?? throw new ArgumentNullException(nameof(swapItDbContext));
        }
        public async Task<bool> AddAsync(UserNotification service)
        {
            Context.UserNotifications.Add(service);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(UserNotification service)
        {
            service.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var service = await GetByIdAsync(id);
            if (service == null)
                return false;

            service.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<UserNotification>> GetAllAsync()
        {
            return await Context.UserNotifications.ToListAsync();
        }

        public async Task<UserNotification> GetByIdAsync(int id)
        {
            return await Context.UserNotifications.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateAsync(UserNotification newUserNotification)
        {
            var service = await GetByIdAsync(newUserNotification.Id);
            if (service == null)
            {
                return false;
            }
            else
            {
                //we need to change itttt
                throw new Exception();
                service = newUserNotification;
                await Context.SaveChangesAsync();
                return true;
            }
        }
    }
}
