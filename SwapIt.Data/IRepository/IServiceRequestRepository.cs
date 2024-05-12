using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.IRepository
{
    public interface IServiceRequestRepository
    {
        Task<ServiceRequest> GetByIdAsync(int id);
        Task<IEnumerable<ServiceRequest>> GetAllAsync();
        Task<bool> AddAsync(ServiceRequest entity);
        Task<bool> UpdateAsync(ServiceRequest entity);
        Task<bool> DeleteAsync(ServiceRequest entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}
