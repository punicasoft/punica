using Microsoft.Extensions.DependencyInjection;

namespace Punica.Bp.EFCore.Middleware
{
    public class MiddlewareProvider : IMiddlewareProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public MiddlewareProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public bool Ignore<TEntity>()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEntityTypeConfiguration> GetEntityTypesConfigurations()
        {
            return _serviceProvider.GetServices<IEntityTypeConfiguration>();
        }

        public IEntityInterceptor GetAggregatedEntityInterceptors()
        {
            return new EntityInterceptorAggregator(_serviceProvider.GetServices<IEntityInterceptor>());
        }
    }
}
