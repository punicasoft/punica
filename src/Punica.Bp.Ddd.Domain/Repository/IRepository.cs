using System.Linq.Expressions;

namespace Punica.Bp.Ddd.Domain.Repository
{
    public interface IRepository<TEntity> where TEntity : class//, IAggregateRoot
    {
        Task<TEntity?> FindAsync(object key, CancellationToken cancellationToken = default);

        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
