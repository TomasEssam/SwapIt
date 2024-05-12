﻿using Microsoft.EntityFrameworkCore;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<ServiceRequest> AddAsync(ServiceRequest serviceRequest)
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
            return await Context.ServiceRequests.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ServiceRequest> UpdateAsync(ServiceRequest newServiceRequest)
        {
            var serviceRequest = await GetByIdAsync(newServiceRequest.Id);
            if (serviceRequest == null)
            {
                return null;
            }
            else
            {
                //we need to use Auto mapper
                throw new Exception();
                serviceRequest = newServiceRequest;
                await Context.SaveChangesAsync();
                return newServiceRequest;
            }
        }
    }
}
