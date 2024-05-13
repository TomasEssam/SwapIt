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
    public class RateRepository : IRateRepository
    {
        private readonly SwapItDbContext Context;

        public RateRepository(SwapItDbContext swapItDbContext)
        {
            Context = swapItDbContext ?? throw new ArgumentNullException(nameof(swapItDbContext));
        }
        public async Task<bool> AddAsync(Rate rate)
        {
            Context.Rates.Add(rate);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Rate rate)
        {
            rate.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var rate = await GetByIdAsync(id);
            if (rate == null)
                return false;

            rate.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Rate>> GetAllAsync()
        {
            return await Context.Rates.ToListAsync();
        }

        public async Task<Rate> GetByIdAsync(int id)
        {
            return await Context.Rates.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateAsync(Rate newService)
        {
            var rate = await GetByIdAsync(newService.Id);
            if (rate == null)
            {
                return false;
            }
            else
            {
                //we need to change itttt
                throw new Exception();
                rate = newService;
                await Context.SaveChangesAsync();
                return true;
            }
        }
    }
}
