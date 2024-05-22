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
        Task<bool> AddPointsAsync(int userId, int points);
        Task<bool> SubstractPointsAsync(int userId, int points);
        Task<bool> Deposite(int userId, int points);
        Task<bool> Withdraw(int userId, int points);
        Task<int> GetMyPoints(int userId);

    }
}
