using SwapIt.BL.DTOs;
using SwapIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.IServices
{
    public interface IRateService
    {
        Task<bool> CreateAsync(RateDto newRate);
        Task<bool> DeleteAsync(int rateId);
        Task<bool> UpdateAsync(RateDto dto);
        Task<RateDto> GetByIdAsync(int rateId);
        Task<List<RateDto>> GetAllAsync();
        Task<List<RateDto>> GetByServiceIdAsync(int serviceId);
    }
}
