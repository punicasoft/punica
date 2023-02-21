using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.Domain.Entities;
using Punica.Bp.Ddd.Domain.Repository;

namespace Punica.Bp.Ddd.EFCore
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepository<TEntity>(DbContext context) where TEntity : class, IEntity;
    }
}
