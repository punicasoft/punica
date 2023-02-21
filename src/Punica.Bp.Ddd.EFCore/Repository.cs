using Punica.Bp.Ddd.Domain.Repository;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.Domain.Entities;

namespace Punica.Bp.Ddd.EFCore
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _dbContext;

        protected DbSet<TEntity> Entities => _dbContext.Set<TEntity>();

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity?> FindAsync(object key, CancellationToken cancellationToken = default)
        {
            var savedEntity = await Entities.FindAsync(new object?[] { key }, cancellationToken: cancellationToken);
            return savedEntity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var savedEntity = await Entities.AddAsync(entity, cancellationToken);
            return savedEntity.Entity;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            TEntity? entity = await Entities
                .Where(predicate)
                .FirstOrDefaultAsync(cancellationToken);

            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbContext.Attach(entity);

            var updated = Entities.Update(entity).Entity;

            return Task.FromResult(updated);
        }
    }
}
