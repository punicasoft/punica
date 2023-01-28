using Punica.Bp.Ddd.Domain.Entities;
using System.Linq.Expressions;

namespace Punica.Bp.Ddd.Domain.Repository
{
    public interface IRepository<TAggregate> where TAggregate : class, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }

        Task<TAggregate?> GetAsync(Expression<Func<TAggregate, bool>> predicate, CancellationToken cancellationToken = default);

        Task<TAggregate> InsertAsync(TAggregate entity, CancellationToken cancellationToken = default);

        Task<TAggregate> UpdateAsync(TAggregate entity, CancellationToken cancellationToken = default);
    }
}
