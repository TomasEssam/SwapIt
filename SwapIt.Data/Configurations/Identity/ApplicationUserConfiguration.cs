using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Configurations.Identity
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable(name: "Users");
            builder.HasQueryFilter(e => !e.IsDeleted);
            //builder.HasOne(x => x.UserBalance)
            //     .WithOne(ub => ub.User);
            //builder.HasMany(x => x.services)
            //    .WithOne(s => s.ServiceProvider);
            //builder.HasMany(x => x.UserNotifications)
            //    .WithMany(un => un.Users);
            //builder.HasMany(x => x.ServiceRequests)
            //    .WithMany(sr => sr.Customers);
            //builder.HasMany(x => x.Rates)
            //    .WithMany(r => r.Customers);
        }
    }
}
