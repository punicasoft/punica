using Punica.Bp.CQRS.Messages;
using Punica.Bp.CQRS.Pipeline;
using System;
using Microsoft.Extensions.DependencyInjection;
using Punica.Bp.CQRS.Handlers;

namespace Punica.Bp.CQRS
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Send(ICommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var type = command.GetType();
            var handler = (ICommandHandlerWrapper)Activator.CreateInstance(typeof(CommandHandlerWrapper<>).MakeGenericType(type))!;
            return handler.Handle(command, _serviceProvider, cancellationToken);

            ////TODO: handle types
            //// var handler = _serviceProvider.GetRequiredService(command.GetType());
            //var handler = _serviceProvider.GetRequiredService<ICommandHandler<ICommand>>();

            //Task<Unit> Func(ICommand cmd, CancellationToken token) => handler.Convert(cmd, token);

            //var pipeline = _serviceProvider
            //    .GetServices<ICommandPipelineBehavior<ICommand, Unit>>()
            //    .Reverse()
            //    .Aggregate(Func, (next, pipeline) => (cmd, ct) => pipeline.Handle(cmd, next, ct));


            //return pipeline(command, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }


            var type = command.GetType();
            var handler = (ICommandHandlerWrapper<TResponse>)Activator.CreateInstance(typeof(CommandHandlerWrapper<,>).MakeGenericType(type, typeof(TResponse)))!;
            return handler.Handle(command, _serviceProvider, cancellationToken);
            ////TODO: handle types
            //var handler = _serviceProvider.GetRequiredService<ICommandHandler<ICommand<TResponse>, TResponse>>();

            //var pipeline = _serviceProvider
            //    .GetServices<ICommandPipelineBehavior<ICommand<TResponse>, TResponse>>()
            //    .Reverse()
            //    .Aggregate(handler.Handle, (next, pipeline) => (cmd, ct) => pipeline.Handle(cmd, next, ct));


            //return pipeline(command, cancellationToken);
        }



        public Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
