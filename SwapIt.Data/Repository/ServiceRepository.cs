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
        public async Task<Service> AddAsync(Service service)
        {
            Context.Services.Add(service);
            await Context.SaveChangesAsync();
            return service;
        }

        public async Task<bool> DeleteAsync(Service service)
        {
            Context.Services.Remove(service);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var service = await GetByIdAsync(id);
            if (service == null)
                return false;

            Context.Services.Remove(service);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
          return await Context.Services.ToListAsync();
        }

        public async Task<Service> GetByIdAsync(int id)
        {
            return await Context.Services.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Service> UpdateAsync(Service newService)
        {
            var service = await GetByIdAsync(newService.Id);
            if (service == null)
            {
                return null;
            }
            else
            {
                //we need to change itttt
                throw new Exception();
                service = newService;
                await Context.SaveChangesAsync();
                return newService;
            }
        }
    }
}
