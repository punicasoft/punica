using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Punica.Bp.EFCore.Extensions
{
    public static class EfCoreExtensions
    {
        public static EntityEntry<TEntity> As<TEntity>(this EntityEntry entry) where TEntity : class
            => entry.Context.Entry((TEntity)entry.Entity);
    }
}
