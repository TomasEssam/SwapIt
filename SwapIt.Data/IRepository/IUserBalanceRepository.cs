using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.IRepository
{
    public interface IUserBalanceRepository
    {
        Task<UserBalance> GetByIdAsync(int id);
        Task<IEnumerable<UserBalance>> GetAllAsync();
        Task<bool> AddAsync(UserBalance entity);
        Task<bool> UpdateAsync(UserBalance entity);
        Task<bool> DeleteAsync(UserBalance entity);
        Task<bool> DeleteByIdAsync(int id);
        Task<bool> AddPointsAsync(int userId, int points);
        Task<bool> SubstractPointsAsync(int userId, int points);
        Task<UserBalance> GetByUserIdAsync(int id);
        Task<UserBalance> GetByUserAsync(ApplicationUser user);
    }
}
