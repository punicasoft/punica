using Microsoft.Extensions.DependencyInjection;
using Punica.Bp.CQRS.Messages;
using Punica.Bp.CQRS.Pipeline;

namespace Punica.Bp.CQRS.Handlers
{
    public interface IQueryHandlerWrapper<TResponse>
    {
        Task<TResponse> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    public class QueryHandlerWrapper<TQuery, TResponse> : IQueryHandlerWrapper<TResponse> where TQuery : IQuery<TResponse>
    {
        public async Task<TResponse> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
            await Handle((TQuery)request, serviceProvider, cancellationToken).ConfigureAwait(false);

        public Task<TResponse> Handle(TQuery query, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();

            var pipeline = serviceProvider
                .GetServices<IQueryPipelineBehavior<TQuery, TResponse>>()
                .Reverse()
                .Aggregate(handler.Handle, (next, pipeline) => (cmd, ct) => pipeline.Handle(cmd, next, ct));


            return pipeline(query, cancellationToken);
        }
    }
}
