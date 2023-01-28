using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Punica.Bp.EFCore.Middleware
{
    public interface ITrackingFilter
    {
        Task BeforeSave(EntityEntry entry, CancellationToken cancellationToken = default);
        Task<int> AfterSave(int result, CancellationToken cancellationToken = default);
    }
}
