using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Punica.Bp.Ddd.Domain.Entities;
using Punica.Bp.Ddd.Domain.Repository;
using Punica.Bp.EFCore.Extensions;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.EFCore
{
    public class BpContext : DbContext, IUnitOfWork
    {
        public BpContext(DbContextOptions options)
            : base(options)
        {

        }

        private void ChangeTrackerOnStateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            // e.Entry
            new EntityEntry<IAggregateRoot>(e.Entry.GetInfrastructure());
            throw new NotImplementedException();
        }

        private void ChangeTrackerOnTracked(object? sender, EntityTrackedEventArgs e)
        {
            throw new NotImplementedException();
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
