using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Punica.Bp.Ddd.Domain.Repository;

namespace Punica.Bp.Ddd.EFCore.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories<TContext>(this IServiceCollection services) where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.TryAddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<DbContext>(sp=> sp.GetRequiredService<TContext>());

            return services;
        }
    }
}
