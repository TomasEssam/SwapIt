using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.IRepository
{
    public interface IPointsLoggerRepository
    {
        Task<PointsLogger> GetByIdAsync(int id);
        Task<IEnumerable<PointsLogger>> GetAllAsync();
        Task<bool> AddAsync(PointsLogger entity);
        Task<bool> UpdateAsync(PointsLogger entity);
        Task<bool> DeleteAsync(PointsLogger entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}
