using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using SwapIt.Data.Configurations.Entities;
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
        
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceRequest> Requests { get; set; }
        public DbSet<UserBalance> Balances { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Category> Categories { get; set; }

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
                entity.Entity.CreationUser = "Tomas";
                entity.Entity.CreationDate = DateTime.UtcNow;
                entity.Entity.ModificationUser = "Tomas";
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
                entity.Entity.ModificationUser = "Tomas";
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

            #region Entities 
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceRequestConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new RateConfiguration());
            modelBuilder.ApplyConfiguration(new UserBalanceConfiguration());
            modelBuilder.ApplyConfiguration(new UserNotificationConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            #endregion

        }

    }
}
