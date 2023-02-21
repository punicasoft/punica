using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Punica.Bp.EFCore.Extensions;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.MultiTenancy.EFCore.Interceptor
{
    public class TenantInterceptor : IEntityInterceptor
    {
        private readonly ITenantContext _tenantContext;

        public TenantInterceptor(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public Task BeforeSavingAsync(EntityEntry entry, CancellationToken cancellationToken = default)
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

        public Task<int> AfterSavingAsync(int result, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(result);
        }

        public Task SavedFailedAsync(Exception exception, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
