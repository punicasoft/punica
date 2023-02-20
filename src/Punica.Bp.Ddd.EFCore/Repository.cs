using Punica.Bp.Ddd.Domain.Repository;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.Domain.Entities;

namespace Punica.Bp.Ddd.EFCore
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly BpDbContext _dbContext;

        protected abstract IQueryable<TEntity> IncludeAll(IQueryable<TEntity> query);

        public IUnitOfWork UnitOfWork => _dbContext;

        protected Repository(BpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var savedEntity = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            return savedEntity.Entity;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            TEntity? entity = await IncludeAll(_dbContext.Set<TEntity>())
                .Where(predicate)
                .FirstOrDefaultAsync(cancellationToken);

            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Attach(entity);

            var updated = _dbContext.Update(entity).Entity;

            return Task.FromResult(updated);
        }
    }
}
