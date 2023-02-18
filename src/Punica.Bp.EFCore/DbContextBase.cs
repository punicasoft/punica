using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Punica.Bp.EFCore.Middleware;
using System.Diagnostics.CodeAnalysis;

namespace Punica.Bp.EFCore
{
    public class DbContextBase : DbContext
    {
        public DbContextBase(DbContextOptions options)
            : base(options)
        {
        }

        private void ChangeTrackerOnStateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            // e.Entry
            //new EntityEntry<IAggregateRoot>(e.Entry.GetInfrastructure());
            throw new NotImplementedException();
        }

        private void ChangeTrackerOnTracked(object? sender, EntityTrackedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes = modelBuilder.Model.GetEntityTypes();

            var methodInfo = typeof(DbContextBase)
                .GetMethod(nameof(ConfigureBaseProperties), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            //foreach (var entityType in entityTypes)
            //{
            //    methodInfo?.MakeGenericMethod(entityType.ClrType)
            //        .Invoke(this, new object[] { modelBuilder, entityType });
            //}

            var configurations = this.GetService<IEnumerable<IEntityTypeConfiguration>>();

            foreach (var entityType in entityTypes)
            {
                methodInfo?.MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType, configurations });
            }
        }

        protected virtual void ConfigureBaseProperties<TEntity>([NotNull] ModelBuilder modelBuilder, [NotNull] IMutableEntityType mutableEntityType, IEnumerable<IEntityTypeConfiguration> configurations)
            where TEntity : class
        {
            if (mutableEntityType.IsOwned())
            {
                return;
            }

            //if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
            //{
            //    return;
            //}

            foreach (var configuration in configurations)
            {
                configuration.Configure(modelBuilder.Entity<TEntity>());
            }

          
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> entries = ChangeTracker
                .Entries();

            IEnumerable<ITrackingFilter> filters = this.GetService<IEnumerable<ITrackingFilter>>();

            var result = 0;

            foreach (EntityEntry entry in entries)
            {
                var trackingFilters = filters == null ? new List<ITrackingFilter>() : filters.ToList();

                foreach (ITrackingFilter filter in trackingFilters)
                {
                    await filter.BeforeSave(entry, cancellationToken);
                }

                result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                foreach (ITrackingFilter filter in trackingFilters)
                {
                    result = await filter.AfterSave(result, cancellationToken);
                }

            }

            return result;
        }
        
    }
}
