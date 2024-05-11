using SwapIt.Data.Entities;
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
        Task<UserBalance> AddAsync(UserBalance entity);
        Task<UserBalance> UpdateAsync(UserBalance entity);
        Task<bool> DeleteAsync(UserBalance entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}
