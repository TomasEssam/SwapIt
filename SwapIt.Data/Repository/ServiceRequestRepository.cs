using Microsoft.EntityFrameworkCore;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwapIt.Data.Repository
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly SwapItDbContext Context;

        public ServiceRequestRepository(SwapItDbContext swapItDbContext)
        {
            Context = swapItDbContext ?? throw new ArgumentNullException(nameof(swapItDbContext));
        }
        public async Task<bool> AddAsync(ServiceRequest serviceRequest)
        {
            Context.ServiceRequests.Add(serviceRequest);
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<ServiceRequest> AddAndReturnAsync(ServiceRequest serviceRequest)
        {
            Context.ServiceRequests.Add(serviceRequest);
            await Context.SaveChangesAsync();
            return serviceRequest;
        }

        public async Task<bool> DeleteAsync(ServiceRequest serviceRequest)
        {
            serviceRequest.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var serviceRequest = await GetByIdAsync(id);
            if (serviceRequest == null)
                return false;

            serviceRequest.IsDeleted = true;
            return await Context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllAsync()
        {
            return await Context.ServiceRequests.ToListAsync();
        }

        public async Task<ServiceRequest> GetByIdAsync(int id)
        {
            return await Context.ServiceRequests.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(ServiceRequest newServiceRequest)
        {
            var serviceRequest = await GetByIdAsync(newServiceRequest.Id);
            if (serviceRequest == null)
            {
                return false;
            }
            else
            {
                serviceRequest.Notes = newServiceRequest.Notes;
                serviceRequest.RequestState = newServiceRequest.RequestState;
                serviceRequest.ExecutionTime = newServiceRequest.ExecutionTime;
                serviceRequest.ModificationDate = DateTime.Now;

                await Context.SaveChangesAsync();
                return true;
            }
        }
    }
}
