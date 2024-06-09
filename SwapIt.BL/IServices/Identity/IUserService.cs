using Microsoft.AspNetCore.Http;
using SwapIt.BL.DTOs;
using SwapIt.BL.DTOs.Identity;

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
        Task<ProfileDto> GetUserAsync(int userId);
        Task<List<DropDownDto>> DropDownAsync();
        Task<bool> UploadIdImage(IFormFile idImage, int userId, string folderName);
        Task<bool> UploadProfileImage(IFormFile profileImage, int userId, string folderName);
        Task<ImageResultDto> GetProfileImage(int userId);
        Task<ImageResultDto> GetIdImage(int userId);
    }
}
