using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Punica.Bp.EFCore.Middleware;
using System.Diagnostics.CodeAnalysis;

namespace Punica.Bp.EFCore
{
    public class DbContextBase : DbContext
    {
        public DbContextBase(DbContextOptions options)
            : base(options)
        {
        }

        private void ChangeTrackerOnStateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            // e.Entry
            //new EntityEntry<IAggregateRoot>(e.Entry.GetInfrastructure());
            throw new NotImplementedException();
        }

        private void ChangeTrackerOnTracked(object? sender, EntityTrackedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes = modelBuilder.Model.GetEntityTypes();

            var methodInfo = typeof(DbContextBase)
                .GetMethod(nameof(ConfigureBaseProperties), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            
            var configurations = this.GetService<IMiddlewareProvider>().GetEntityTypesConfigurations();

            foreach (var entityType in entityTypes)
            {
                methodInfo?.MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType, configurations });
            }
        }

        protected virtual void ConfigureBaseProperties<TEntity>([NotNull] ModelBuilder modelBuilder, [NotNull] IMutableEntityType mutableEntityType, [NotNull] IEnumerable<IEntityTypeConfiguration> configurations)
            where TEntity : class
        {
            if (mutableEntityType.IsOwned())
            {
                return;
            }
            
            // ReSharper disable once PossibleMultipleEnumeration
            if (configurations.Any(configuration => configuration.Ignore<TEntity>()))
            {
                return;
            }

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var configuration in configurations)
            {
                configuration.Configure(modelBuilder.Entity<TEntity>());
            }


        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry> entries = ChangeTracker
                .Entries();

            var interceptor = this.GetService<IMiddlewareProvider>().GetAggregatedEntityInterceptors();

            int result;

            foreach (EntityEntry entry in entries)
            {
                await interceptor.BeforeSavingAsync(entry, cancellationToken);
            }

            try
            {
                result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                await interceptor.AfterSavingAsync(result, cancellationToken);
            }
            catch (Exception e)
            {
                await interceptor.SavedFailedAsync(e, cancellationToken);
                throw;
            }

            return result;
        }

        //private LambdaExpression CombineQueryFilters(Type entityType, LambdaExpression baseFilter, IEnumerable<LambdaExpression> andAlsoExpressions)
        //{
        //    var newParam = Expression.Parameter(entityType);

        //    var andAlsoExprBase = (Expression<Func<IEntity, bool>>)(_ => true);
        //    var andAlsoExpr = ReplacingExpressionVisitor.Replace(andAlsoExprBase.Parameters.Single(), newParam, andAlsoExprBase.Body);
        //    foreach (var expressionBase in andAlsoExpressions)
        //    {
        //        var expression = ReplacingExpressionVisitor.Replace(expressionBase.Parameters.Single(), newParam, expressionBase.Body);
        //        andAlsoExpr = Expression.AndAlso(andAlsoExpr, expression);
        //    }

        //    var baseExp = ReplacingExpressionVisitor.Replace(baseFilter.Parameters.Single(), newParam, baseFilter.Body);
        //    var exp = Expression.OrElse(baseExp, andAlsoExpr);

        //    return Expression.Lambda(exp, newParam);
        //}

    }
}
