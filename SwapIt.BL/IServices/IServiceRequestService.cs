using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.IServices
{
    public interface IServiceRequestService
    {
        Task<bool> CreateAsync(ServiceRequestDto dto);
        Task<ServiceRequestDto> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int serviceRequestId);
        Task<bool> UpdateAsync(ServiceRequestDto dto);
        Task<bool> CancelServiceRequestAsync(int userId, int ServiceRequestId);
        Task<bool> AcceptServiceRequestAsync(int serviceRequestId);
        Task<bool> FinishServiceRequestAsync(int serviceRequestId);
        Task<ServiceRequest?> GetAsync(int serviceId, int userId);

        Task<List<ServiceRequestDto>> GetAllAsync();

    }
}
