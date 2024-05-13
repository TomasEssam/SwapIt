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
        Task<List<SearchResultDto>> GetAllAcceptedAsync(int userId);
        Task<List<SearchResultDto>> GetAllFinshiedAsync(int userId);
        Task<List<ServiceDto>> GetAllAsync();
        Task<List<SearchResultDto>> SearchServiceAsync(ServiceFilterDto Filter);
        Task<List<string>> GetAllPreviousWorkImageUrlAsync(int userId);
        Task<ServiceDto> GetServiceByIdAsync(int serviceId);
        Task<List<DropDownDto>> GetServiceDDAsync();
        Task<List<RateDto>> GetRatesAsync(int serviceId);
    }
}
