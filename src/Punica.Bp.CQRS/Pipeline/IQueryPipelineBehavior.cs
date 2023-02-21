namespace Punica.Bp.CQRS.Pipeline
{
    public interface IQueryPipelineBehavior<TQuery, TResponse> : IPipelineBehavior<TQuery, TResponse> where TQuery : notnull
    {
    }
}
