using Punica.Bp.Auditing.EFCore.Filters;
using Punica.Bp.EFCore.Middleware;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class PunicaBpAuditingExtensions
    {
        public static IServiceCollection AddAuditing(this IServiceCollection services)
        {
            services.AddScoped<IEntityInterceptor, AddedFilter>();
            services.AddScoped<IEntityInterceptor, ModifiedFilter>();
            services.AddScoped<IEntityInterceptor, DeletedFilter>();
            return services;
        }
    }
}
