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

        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<UserBalance> UserBalances { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PointsLogger> PointsLoggers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyConfigurationsToModel(modelBuilder);
            modelBuilder.Entity<Rate>().HasOne(c => c.Customer)
                .WithMany(x => x.Rates).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Rate>().HasOne(c => c.Service)
               .WithMany(x => x.Rates).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Service>().HasOne(s => s.ServiceProvider)
               .WithMany(x => x.Services).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Service>().HasOne(c => c.Category)
               .WithMany(x => x.Services).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ServiceRequest>().HasOne(c => c.Customer)
               .WithMany(x => x.ServiceRequests).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ServiceRequest>().HasOne(c => c.Service)
               .WithMany(x => x.ServiceRequests).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserBalance>().HasOne(c => c.User)
               .WithOne(x => x.UserBalance).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserNotification>().HasOne(c => c.Notification)
               .WithMany(x => x.UserNotifications).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UserNotification>().HasOne(c => c.User)
               .WithMany(x => x.UserNotifications).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PointsLogger>().HasOne(p => p.User)
                .WithMany(x => x.PointsLoggers).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Notification>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Rate>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Service>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<ServiceRequest>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<UserBalance>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<UserNotification>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PointsLogger>().HasQueryFilter(e => !e.IsDeleted);


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
            //modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            //modelBuilder.ApplyConfiguration(new ServiceRequestConfiguration());
            //modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            //modelBuilder.ApplyConfiguration(new RateConfiguration());
            //modelBuilder.ApplyConfiguration(new UserBalanceConfiguration());
            //modelBuilder.ApplyConfiguration(new UserNotificationConfiguration());
            //modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            #endregion

        }

    }
}
