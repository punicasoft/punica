using Punica.Bp.Auditing.EFCore.Filters;
using Punica.Bp.EFCore.Middleware;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class PunicaBpAuditingExtensions
    {
        public static IServiceCollection AddAuditing(this IServiceCollection services)
        {
            services.AddScoped<ITrackingFilter, AddedFilter>();
            services.AddScoped<ITrackingFilter, ModifiedFilter>();
            services.AddScoped<ITrackingFilter, DeletedFilter>();
            return services;
        }
    }
}
