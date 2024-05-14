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
    public class ServiceRepository : IServiceRepository
    {
        private readonly SwapItDbContext Context;

        public ServiceRepository(SwapItDbContext swapItDbContext)
        {
            Context = swapItDbContext ?? throw new ArgumentNullException(nameof(swapItDbContext));
        }
        public async Task<bool> AddAsync(Service service)
        {
            Context.Services.Add(service);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Service service)
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

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await Context.Services.ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetAllWithServiceProviderAsync()
        {
            return await Context.Services.Include(s => s.ServiceProvider).ToListAsync();
        }

        public async Task<Service> GetByIdAsync(int id)
        {
            return await Context.Services.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Service newService)
        {
            var service = await GetByIdAsync(newService.Id);
            if (service == null)
            {
                return false;
            }
            else
            {
                service.Name = newService.Name;
                service.Price = newService.Price;
                service.Description = newService.Description;
                service.CategoryId = newService.CategoryId;
                service.PreviousworkImagesUrl = newService.PreviousworkImagesUrl;
                service.TimeToExecute = newService.TimeToExecute;
                service.ModificationDate = DateTime.Now;

                await Context.SaveChangesAsync();
                return true;
            }
        }
    }
}
