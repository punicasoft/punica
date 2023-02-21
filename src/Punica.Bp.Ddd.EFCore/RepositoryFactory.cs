using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Punica.Bp.Ddd.Domain.Entities;
using Punica.Bp.Ddd.Domain.Repository;

namespace Punica.Bp.Ddd.EFCore
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public IRepository<TEntity> GetRepository<TEntity>(DbContext context) where TEntity : class, IEntity
        {
            return context.GetService<IRepository<TEntity>>();
        }
    }
}
