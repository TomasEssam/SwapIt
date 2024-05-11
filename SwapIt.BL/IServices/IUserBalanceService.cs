using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.IServices
{
    public interface IUserBalanceService
    {
        Task<bool> CreateAsync(UserBalanceDto dto);
        Task<bool> DeleteAsync(int userBalanceId);
        Task<UserBalanceDto> UpdateAsync(UserBalanceDto dto);
        Task<UserBalanceDto> GetByIdAsync(int userBalanceId);

    }
}
