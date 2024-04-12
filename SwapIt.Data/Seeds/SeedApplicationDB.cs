using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SwapIt.Data.Constants;
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

                await userManager.CreateAsync(adminUser, "P@$$w0rd");
                await userManager.AddToRoleAsync(adminUser, RolesNames.Admin);
            }
        }
    }
}
