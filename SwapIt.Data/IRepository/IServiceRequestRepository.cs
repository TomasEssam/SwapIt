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
        Task<ServiceRequest> AddAsync(ServiceRequest entity);
        Task<ServiceRequest> UpdateAsync(ServiceRequest entity);
        Task<bool> DeleteAsync(ServiceRequest entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}
