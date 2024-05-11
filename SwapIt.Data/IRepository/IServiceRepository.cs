﻿using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.IRepository
{
    public interface IServiceRepository
    {
        Task<Service> GetByIdAsync(int id);
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service> AddAsync(Service entity);
        Task<Service> UpdateAsync(Service entity);
        Task<bool> DeleteAsync(Service entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}
