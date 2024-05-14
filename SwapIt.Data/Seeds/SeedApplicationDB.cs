using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SwapIt.Data.Constants;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Context;
using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Seeds
{
    public class SeedApplicationDB
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<SwapItDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            context.Database.Migrate(); // apply all migrations 
            context.Database.EnsureCreated();


            if (!context.Roles.Any())
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = RolesNames.Admin, NormalizedName = RolesNames.Admin });
                await roleManager.CreateAsync(new ApplicationRole { Name = RolesNames.ServiceProvider, NormalizedName = RolesNames.ServiceProvider });
                await roleManager.CreateAsync(new ApplicationRole { Name = RolesNames.ServiceConsumer, NormalizedName = RolesNames.ServiceConsumer });
                await roleManager.CreateAsync(new ApplicationRole { Name = RolesNames.SuperAdmin, NormalizedName = RolesNames.SuperAdmin });
            }
            if (!context.Users.Any())
            {
                var adminUser = new ApplicationUser()
                {
                    ApplicationUserId = Guid.NewGuid(),
                    Email = "TomasEssam@Tomas.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "TomasEssam",
                    IsActive = true,
                };

                var serviceProviderUser = new ApplicationUser()
                {
                    ApplicationUserId = Guid.NewGuid(),
                    Email = "David@Gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "DavidAmir",
                    IsActive = true,
                };

                var serviceConsumerUser = new ApplicationUser()
                {
                    ApplicationUserId = Guid.NewGuid(),
                    Email = "George@Gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "GeorgeMagdy",
                    IsActive = true,
                };
                var superAdminUser = new ApplicationUser()
                {
                    ApplicationUserId = Guid.NewGuid(),
                    Email = "George2@Gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "GeorgeSobhy",
                    IsActive = true,
                };

                await userManager.CreateAsync(adminUser, "P@$$w0rd");
                await userManager.AddToRoleAsync(adminUser, RolesNames.Admin);

                await userManager.CreateAsync(serviceProviderUser, "P@$$w0rd");
                await userManager.AddToRoleAsync(serviceProviderUser, RolesNames.ServiceProvider);

                await userManager.CreateAsync(serviceConsumerUser, "P@$$w0rd");
                await userManager.AddToRoleAsync(serviceConsumerUser, RolesNames.ServiceConsumer);

                await userManager.CreateAsync(superAdminUser, "P@$$w0rd");
                await userManager.AddToRoleAsync(superAdminUser, RolesNames.SuperAdmin);
               
                UserBalance userBalance = new UserBalance()
                {
                    Amount = 150,
                    Points = 30,
                    UserId = serviceConsumerUser.Id
                };
                context.UserBalances.Add(userBalance);
                context.SaveChanges();
            }

            //this part is for the UserRoles table to assign User Id with Role Id

            //if (!context.UserRoles.Any())
            //{
            //    var AdminUserRoles = new ApplicationUserRole()
            //    {
            //        RoleId = context.Roles.Where(r => r.Name == "Admin").FirstOrDefault().Id,
            //        UserId = context.Users.Where(u => u.UserName == "TomasEssam").FirstOrDefault().Id,
            //    };
            //    context.UserRoles.Add(AdminUserRoles);
            //    context.SaveChanges();
            //}
        }
    }
}
