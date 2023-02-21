using System.Linq.Expressions;

namespace Punica.Bp.EFCore.Middleware
{
    public interface IQueryFilter<TEntity>
    {
        Expression<Func<TEntity, bool>> Expression { get; }
    }
}
