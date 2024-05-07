using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SwapIt.BL.DTOs.Identity;
using SwapIt.BL.Helpers;
using SwapIt.BL.IServices.Identity;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.IPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.Services
{
    public class RoleService :  IRoleService 
    {
        SwapItDbContext context;
        public RoleService(IServiceProvider serviceProvider)
        {
            context = serviceProvider.GetRequiredService<SwapItDbContext>();
        }

        public async Task<bool> AddUserRoleRow(UserRolesDto userRole)
        {
            var user = context.Users.FirstOrDefault(u => u.UserName == userRole.UserName);
            var UserId = user.Id;
            int? RoleId = null;
            if (userRole.RoleName != null)
            {
                RoleId = context.Roles.Where(r => r.Name == userRole.RoleName).FirstOrDefault().Id;
            }

            if (UserId != null && RoleId != null)
            {
                var UserRoles = new ApplicationUserRole()
                {
                    RoleId = (int)RoleId,
                    UserId = UserId,
                };

                context.UserRoles.Add(UserRoles);
                context.SaveChanges();
                return await Task.FromResult(true);
            }
            else if(UserId != null)
            {
                var ServiceProviderUserRoles = new ApplicationUserRole()
                {
                    
                    RoleId = context.Roles.Where(r => r.Name == "Service Provider").FirstOrDefault().Id,
                    UserId = UserId,
                };
                var ServiceConsumerUserRoles = new ApplicationUserRole()
                {

                    RoleId = context.Roles.Where(r => r.Name == "Service Consumer").FirstOrDefault().Id,
                    UserId = UserId,
                };

                context.UserRoles.Add(ServiceProviderUserRoles);
                context.UserRoles.Add(ServiceConsumerUserRoles);
                context.SaveChanges();
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public List<string> GetRoles()
        {
            return new List<string>() { RolesNames.Admin, RolesNames.SuperAdmin,
                RolesNames.ServiceProvider , RolesNames.ServiceConsumer};
        }


    }
}
