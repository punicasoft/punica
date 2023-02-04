using Punica.Bp.Ddd.Domain.Repository;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.Domain.Entities;
using Punica.Bp.EFCore;

namespace Punica.Bp.Ddd.EFCore
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContextBase _dbContextBase;

        protected abstract IQueryable<TEntity> IncludeAll(IQueryable<TEntity> query);

        public IUnitOfWork UnitOfWork => null;

        protected Repository(DbContextBase dbContextBase)
        {
            _dbContextBase = dbContextBase;
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var savedEntity = await _dbContextBase.Set<TEntity>().AddAsync(entity, cancellationToken);
            return savedEntity.Entity;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            TEntity? entity = await IncludeAll(_dbContextBase.Set<TEntity>())
                .Where(predicate)
                .FirstOrDefaultAsync(cancellationToken);

            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbContextBase.Attach(entity);

            var updated = _dbContextBase.Update(entity).Entity;

            return Task.FromResult(updated);
        }
    }
}
