using SwapIt.BL.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.IServices.Identity
{
    public interface IUserService
    {
        Task<LoginResultDto> Authenticate(LoginDto dto);
        Task CreateUserAsync(UserDto dto);
        Task DeleteUserAsync(int userId);
        Task UpdateUserAsync(UserDto dto);
        Task<bool> IsUserNameExists(string userName, int? userId = null);
        Task<string> GetUserRole(int userId);
        Task ResetPasswordAsync(ResetPasswordDto dto);
        Task<LoginResultDto> RefreshToken(string authenticationToken, string refreshToken);
    }
}
