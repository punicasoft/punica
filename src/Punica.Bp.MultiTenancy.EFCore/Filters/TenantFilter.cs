using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Punica.Bp.EFCore.Extensions;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.MultiTenancy.EFCore.Filters
{
    public class TenantFilter : ITrackingFilter
    {
        private readonly ITenantContext _tenantContext;

        public TenantFilter(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public Task BeforeSave(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            if (!_tenantContext.TenantId.HasValue)
            {
                return Task.CompletedTask;
            }

            var type = entry.Metadata.ClrType;

            if (entry.State == EntityState.Added && type.IsAssignableTo(typeof(IMultiTenant)))
            {
                entry.As<IMultiTenant>().Property(p => p.TenantId).CurrentValue = _tenantContext.TenantId;
            }

            return Task.CompletedTask;
        }

        public Task<int> AfterSave(int result, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(result);
        }
    }
}
