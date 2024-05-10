using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwapIt.Data.Entities;
using SwapIt.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Configurations.Entities
{
    public class UserBalanceConfiguration : IEntityTypeConfiguration<UserBalance>
    {
        public void Configure(EntityTypeBuilder<UserBalance> builder)
        {
            builder.ToTable(name: "UserBalance");
            builder.HasQueryFilter(e => !e.IsDeleted);
        
        }
    }
}
