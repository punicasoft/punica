using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Punica.Bp.EFCore.Middleware
{
    public interface IEntityInterceptor
    {
        Task BeforeSavingAsync(EntityEntry entry, CancellationToken cancellationToken = default);
        Task<int> AfterSavingAsync(int result, CancellationToken cancellationToken = default);
        Task SavedFailedAsync(Exception exception, CancellationToken cancellationToken = default);
    }
}
