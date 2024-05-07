using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.IServices.Identity;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SwapIt.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
   

        public UserService(IConfiguration configuration,
    SignInManager<ApplicationUser> signinManager,
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signinManager = signinManager;
            _httpContextAccessor = httpContextAccessor;
           
        }
        public async Task<LoginResultDto> Authenticate(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user != null && user.IsActive)
            {
                var result = await _signinManager.CheckPasswordSignInAsync(user, dto.Password, false);

                if (result.Succeeded)
                {
                    var claims = await GetUserClaims(user);
                    var token = GenerateAccessToken(claims);
                    var refreshToken = GenerateRefreshToken();
                    user.RefreshToken = refreshToken;
                    SetClaimsInHttpContextManualy(claims);
                    await _userManager.UpdateAsync(user);
                    var userRoles = await _userManager.GetRolesAsync(user);
                    return new LoginResultDto()
                    {
                        Token = token,
                        RefreshToken = refreshToken,
                        UserName = user.UserName,
                        UserId = user.Id,
                        Roles = userRoles,
 

                    };
                }
                throw new Exception("Invalid User name or password");
            }
            throw new Exception("Invalid User name or password");
        }
        public async Task CreateUserAsync(UserDto dto)
        {
            var user = new ApplicationUser()
            {
                BirthDate = dto.DateOfBirth,
                Email = dto.Email,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString(),
                ApplicationUserId = Guid.NewGuid(),
                UserName = dto.Username,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(x => x.Description));
                throw new Exception(errors);
            }
            
            dto.UserId = user.Id;
            try
            {
                if (string.IsNullOrEmpty(dto.RoleId))
                {
                    await _userManager.AddToRoleAsync(user, RolesNames.ServiceProvider);
                    await _userManager.AddToRoleAsync(user, RolesNames.ServiceConsumer);

                }
                else if (dto.RoleId == RolesNames.Admin || dto.RoleId == RolesNames.SuperAdmin)
                {
                    await _userManager.AddToRoleAsync(user, dto.RoleId); 
                }
                else
                {
                    throw new Exception("You provided a Role name that doesn't exist");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            await _userManager.DeleteAsync(user);
            await Task.CompletedTask;
        }
        public async Task UpdateUserAsync(UserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            user.UserName = dto.Username;
            user.Email = dto.Username;
            await _userManager.UpdateAsync(user);
            var inRole = await _userManager.IsInRoleAsync(user, dto.RoleId);
            if (!inRole)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRoleAsync(user, dto.RoleId);
            }
            await Task.CompletedTask;
        }
        public async Task<bool> IsUserNameExists(string userName, int? userId = null)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return (user != null || (userId.HasValue && userId.Value == user.Id));
        }
        public async Task<string> GetUserRole(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.FirstOrDefault();
        }
        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, dto.Password);
            await Task.CompletedTask;
        }
        public async Task<LoginResultDto> RefreshToken(string authenticationToken, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(authenticationToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != refreshToken) { throw new Exception("Invalid token"); };
            var newJwtToken = GenerateAccessToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            SetClaimsInHttpContextManualy(principal.Claims.ToList());
            await _userManager.UpdateAsync(user);

            return new LoginResultDto() { Token = newJwtToken, RefreshToken = newRefreshToken, UserName = user.UserName, UserId = user.Id };
        }
        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var nowUtc = DateTime.UtcNow;

            var expiryDuration = double.Parse(_configuration["Token:ExpiryMinutes"]);

            var expires = nowUtc.AddMinutes(expiryDuration);

            var jwt = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt); //the method is called WriteToken but returns a string
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Token:Issuer"],
                ValidAudience = _configuration["Token:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"])),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        private async Task<IEnumerable<Claim>> GetUserClaims(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
           

            var identity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ApplicationClaims.UserId, user.Id.ToString()),
                new Claim(ApplicationClaims.UserName, user.UserName.ToString()),
                new Claim(ApplicationClaims.Roles, string.Join(",", userRoles)), // used for frontend
               
            }, "Token");

            // used for identity 
            identity.AddClaims(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            return identity.Claims;
        }
        private void SetClaimsInHttpContextManualy(IEnumerable<Claim> claims)
        {
            // to make user in httpcontext not empty so when call savechanges 
            // can find username claim to use it in audit information
            var claimsIdentity = new ClaimsIdentity(claims, "Token");
            _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
            AppSecurityContext.Configure(_httpContextAccessor);
        }

    }
}
