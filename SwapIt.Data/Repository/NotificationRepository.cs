using Microsoft.EntityFrameworkCore;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly SwapItDbContext Context;

        public NotificationRepository(SwapItDbContext swapItDbContext)
        {
            Context = swapItDbContext ?? throw new ArgumentNullException(nameof(swapItDbContext));
        }

        public async Task<bool> AddAsync(Notification notification)
        {
            Context.Notifications.Add(notification);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Notification notification)
        {
            notification.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var notification = await GetByIdAsync(id);
            if (notification == null)
                return false;

            notification.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await Context.Notifications.ToListAsync();
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await Context.Notifications.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateAsync(Notification newNotification)
        {
            var notification = await GetByIdAsync(newNotification.Id);
            if (notification == null)
            {
                return false;
            }
            else
            {
                //we need to change itttt
                throw new Exception();
                notification = newService;
                await Context.SaveChangesAsync();
                return true;
            }
        }
    }
}
