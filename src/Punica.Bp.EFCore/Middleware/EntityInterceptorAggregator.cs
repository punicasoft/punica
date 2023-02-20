using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Punica.Bp.EFCore.Middleware
{
    public class EntityInterceptorAggregator : IEntityInterceptor
    {
        private readonly IEntityInterceptor[] _interceptors;

        public EntityInterceptorAggregator(IEnumerable<IEntityInterceptor> interceptors)
        {
            _interceptors = interceptors.ToArray();
        }

        public async Task BeforeSavingAsync(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            for (var i = 0; i < _interceptors.Length; i++)
            {
                var interceptor = _interceptors[i];
                await interceptor.BeforeSavingAsync(entry, cancellationToken);
            }
        }

        public async Task<int> AfterSavingAsync(int result, CancellationToken cancellationToken = default)
        {
            for (var i = 0; i < _interceptors.Length; i++)
            {
                var interceptor = _interceptors[i];
                result = await interceptor.AfterSavingAsync(result, cancellationToken);
            }

            return result;
        }

        public async Task SavedFailedAsync(Exception exception, CancellationToken cancellationToken = default)
        {
            for (var i = 0; i < _interceptors.Length; i++)
            {
                var interceptor = _interceptors[i];
                await interceptor.SavedFailedAsync(exception, cancellationToken);
            }
        }
    }
}
