using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.IRepository
{
    public interface IRateRepository
    {
        Task<Rate> GetByIdAsync(int id);
        Task<IEnumerable<Rate>> GetAllAsync();
        Task<bool> AddAsync(Rate rate);
        Task<bool> UpdateAsync(Rate rate);
        Task<bool> DeleteAsync(Rate rate);
        Task<bool> DeleteByIdAsync(int id);
    }
}
