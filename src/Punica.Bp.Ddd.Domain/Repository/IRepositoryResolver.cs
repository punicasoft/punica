using Punica.Bp.Ddd.Domain.Entities;

namespace Punica.Bp.Ddd.Domain.Repository
{
    public interface IRepositoryResolver
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
    }
}
