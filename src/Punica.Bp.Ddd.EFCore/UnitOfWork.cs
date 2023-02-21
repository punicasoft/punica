using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.Domain.Entities;
using Punica.Bp.Ddd.Domain.Repository;

namespace Punica.Bp.Ddd.EFCore
{
    public class UnitOfWork<TContext> : IUnitOfWork, IRepositoryResolver where TContext: DbContext
    {
        private readonly TContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;

        public UnitOfWork(TContext dbContext, IRepositoryFactory repositoryFactory)
        {
            this._dbContext = dbContext;
            _repositoryFactory = repositoryFactory;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            return _repositoryFactory.GetRepository<TEntity>(_dbContext);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
           return await _dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
