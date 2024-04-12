using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using SwapIt.Data.Configurations.Identity;
using SwapIt.Data.Entities.Common;
using SwapIt.Data.Entities.Identity;
using SwapIt.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwapIt.Data.Entities.Context
{
    public class SwapItDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserClaim,
        ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public SwapItDbContext(DbContextOptions<SwapItDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyConfigurationsToModel(modelBuilder);
        }
        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyAuditInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            //When adding entities
            var addedEntities = ChangeTracker.Entries<IAuditEntity>()
                .Where(e => e.State == EntityState.Added);
            foreach (var entity in addedEntities)
            {
                entity.Entity.CreationUser = AppSecurityContext.UserName;
                entity.Entity.CreationDate = DateTime.UtcNow;
                entity.Entity.ModificationUser = AppSecurityContext.UserName;
                entity.Entity.ModificationDate = DateTime.UtcNow;
            }

            //When adding and edititng entities
            var addEditEntities = ChangeTracker.Entries<IDeletedEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var entity in addEditEntities)
            {
                entity.Entity.IsDeleted = false;
            }

            //When editing entities
            var modifiedEntities = ChangeTracker.Entries<IAuditEntity>()
                .Where(e => e.State == EntityState.Modified);
            foreach (var entity in modifiedEntities)
            {
                entity.Entity.ModificationUser = AppSecurityContext.UserName;
                entity.Entity.ModificationDate = DateTime.UtcNow;
            }

            //When deleting entities
            var deletedEntities = ChangeTracker.Entries<IDeletedEntity>()
                .Where(e => e.State == EntityState.Deleted);
            foreach (var entity in deletedEntities)
            {
                if (!entity.Entity.IsDeleted)
                {
                    entity.Entity.DeletionDate = DateTime.UtcNow;
                    entity.Entity.IsDeleted = true;
                    entity.State = EntityState.Modified;
                }
            }
        }

        private void ApplyConfigurationsToModel(ModelBuilder modelBuilder)
        {
            #region Identity

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserTokenConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserLoginConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationRoleClaimConfiguration());

            #endregion

        }

    }
}
