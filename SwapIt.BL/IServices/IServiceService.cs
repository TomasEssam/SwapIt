using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
using SwapIt.Data.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.IServices
{
    public interface IServiceService
    {
        Task<bool> CreateAsync(ServiceDto dto);
        Task<bool> DeleteAsync(int serviceId);
        Task<bool> UpdateAsync(ServiceDto dto);
        Task<List<ServiceDto>> GetAllAcceptedAsync();
        Task<List<ServiceDto>> GetAllFinshiedAsync();
        Task<List<ServiceDto>> GetAllAsync();
        Task<List<ServiceDto>> SearchServiceAsync(ServiceFilterDto Filter);
        Task<List<string>> GetAllPreviousWorkImageUrlAsync(int userId);
        Task<ServiceDto> GetServiceByIdAsync(int serviceId);
        Task<List<DropDownDto>> GetServiceDDAsync();
    }
}
