using Punica.Bp.CQRS.Messages;
using Punica.Bp.CQRS.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Punica.Bp.CQRS.Extensions;

namespace Punica.Bp.CQRS.Handlers
{
    public interface ICommandHandlerWrapper
    {
        Task Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    public class CommandHandlerWrapper<TCommand> : ICommandHandlerWrapper where TCommand : ICommand
    {
        public async Task Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
            await Handle((TCommand)request, serviceProvider, cancellationToken).ConfigureAwait(false);

        public Task Handle(TCommand command, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();

            Task<Unit> Func(TCommand cmd, CancellationToken token) => handler.Convert(cmd, token);

            var pipeline = serviceProvider
                .GetServices<ICommandPipelineBehavior<TCommand, Unit>>()
                .Reverse()
                .Aggregate(Func, (next, pipeline) => (cmd, ct) => pipeline.Handle(cmd, next, ct));


            return pipeline(command, cancellationToken);
        }
    }

    public interface ICommandHandlerWrapper<TResponse>
    {
        Task<TResponse> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }

    public class CommandHandlerWrapperWrapper<TCommand, TResponse> : ICommandHandlerWrapper<TResponse> where TCommand : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
            await Handle((TCommand)request, serviceProvider, cancellationToken).ConfigureAwait(false);

        public Task<TResponse> Handle(TCommand command, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();

            var pipeline = serviceProvider
                .GetServices<ICommandPipelineBehavior<TCommand, TResponse>>()
                .Reverse()
                .Aggregate(handler.Handle, (next, pipeline) => (cmd, ct) => pipeline.Handle(cmd, next, ct));


            return pipeline(command, cancellationToken);
        }
    }
}
