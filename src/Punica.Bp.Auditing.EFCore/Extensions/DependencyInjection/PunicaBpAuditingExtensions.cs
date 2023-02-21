using Punica.Bp.Auditing.EFCore.Interceptors;
using Punica.Bp.EFCore.Middleware;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class PunicaBpAuditingExtensions
    {
        public static IServiceCollection AddAuditing(this IServiceCollection services)
        {
            services.AddScoped<IEntityInterceptor, EntityAddedInterceptor>();
            services.AddScoped<IEntityInterceptor, EntityModifiedInterceptor>();
            services.AddScoped<IEntityInterceptor, EntityDeletedInterceptor>();
            return services;
        }
    }
}
