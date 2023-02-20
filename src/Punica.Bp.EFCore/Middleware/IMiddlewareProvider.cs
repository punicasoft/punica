namespace Punica.Bp.EFCore.Middleware
{
    public interface IMiddlewareProvider
    {
        bool Ignore<TEntity>();
        IEnumerable<IEntityTypeConfiguration> GetEntityTypesConfigurations();
        IEntityInterceptor GetAggregatedEntityInterceptors();
    }
}
