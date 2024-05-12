﻿using Microsoft.EntityFrameworkCore;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Repository
{
    
    public class UserBalanceRepository : IUserBalanceRepository
    {
        private readonly SwapItDbContext Context;
        

        public UserBalanceRepository(SwapItDbContext swapItDbContext)
        {
            Context = swapItDbContext ?? throw new ArgumentNullException(nameof(swapItDbContext));
        }
        public async Task<bool> AddAsync(UserBalance userBalance)
        {
            Context.UserBalances.Add(userBalance);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(UserBalance userBalance)
        {
            userBalance.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var userBalance = await GetByIdAsync(id);
            if (userBalance == null)
                return false;

            userBalance.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<UserBalance>> GetAllAsync()
        {
            return await Context.UserBalances.ToListAsync();
        }

        public async Task<UserBalance> GetByIdAsync(int id)
        {
            return await Context.UserBalances.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateAsync(UserBalance newUserBalance)
        {
            var userBalnce = await GetByIdAsync(newUserBalance.Id);
            if (userBalnce == null)
            {
                return false;
            }
            else
            {
                //we need to use Auto mapper
                throw new Exception();
                userBalnce = newUserBalance;
                await Context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> AddPointsAsync(UserBalance userBalance, int points)
        {
            if (points > 0)
            {
                userBalance.Points += points;
                return await UpdateAsync(userBalance);
            }
            return false;
        }

        public async Task<bool> SubstractPointsAsync(UserBalance userBalance, int points)
        {
            if (points > 0 && userBalance.Points >= points)
            {
                userBalance.Points -= points;
                return await UpdateAsync(userBalance);

            }
            return false;
        }

        public async Task<UserBalance> GetByUserIdAsync(int id)
        {
            return await Context.UserBalances.FirstOrDefaultAsync(c => c.UserId == id);
        }
        public async Task<UserBalance> GetByUserAsync(ApplicationUser user)
        {
            return await Context.UserBalances.FirstOrDefaultAsync(c => c.UserId == user.Id);
        }
    }
}
