using Punica.Bp.Ddd.Domain.Entities;

namespace Punica.Bp.Ddd.Domain.Repository
{
    public static class RepositoryExtensions
    {
        public static IRepository<TEntity> GetRepository<TEntity>(this IUnitOfWork unitOfWork) where TEntity : class, IEntity
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            if (unitOfWork is IRepositoryResolver resolver)
            {
                return resolver.GetRepository<TEntity>();
            }

            throw new InvalidOperationException(
                $"{nameof(unitOfWork)} is not implements type of {typeof(IRepositoryResolver)}");
        }
    }
}
