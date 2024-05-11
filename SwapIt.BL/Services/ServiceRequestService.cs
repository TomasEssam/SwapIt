using SwapIt.BL.DTOs;
using SwapIt.BL.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    public class ServiceRequestService : IServiceRequestService
    {
        public Task<bool> AcceptServiceRequestAsync(int ServiceRequestId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelServiceRequestAsync(int ServiceRequestId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangeStateAsync(int ServiceRequestId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateAsync(ServiceRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int serviceRequestId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceRequestDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceRequestDto> UpdateAsync(ServiceRequestDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
