using Punica.Bp.CQRS.Messages;
using Punica.Bp.CQRS.Handlers;
using System.Collections.Concurrent;

namespace Punica.Bp.CQRS
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, ICommandHandlerWrapper> CommandHandlerWrappers = new();
        private static readonly ConcurrentDictionary<Type, object> CommandResponseHandlersWrappers = new();
        private static readonly ConcurrentDictionary<Type, object> QueryHandlerWrappers = new();
        private static readonly ConcurrentDictionary<Type, INotificationHandlerWrapper> NotificationHandlerWrappers = new();

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var type = notification.GetType();
            var handler = NotificationHandlerWrappers.GetOrAdd(type,
                t => (INotificationHandlerWrapper)Activator.CreateInstance(
                    typeof(NotificationHandlerWrapper<>).MakeGenericType(type))!);
           return handler.Handle(notification, _serviceProvider, cancellationToken);
        }

        public Task Send(ICommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var type = command.GetType();
            var handler = CommandHandlerWrappers.GetOrAdd(type,
                t => (ICommandHandlerWrapper)Activator.CreateInstance(
                    typeof(CommandHandlerWrapper<>).MakeGenericType(type))!);

            return handler.Handle(command, _serviceProvider, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var type = command.GetType();
            var handler = (ICommandHandlerWrapper<TResponse>)CommandResponseHandlersWrappers.GetOrAdd(type,
                t => Activator.CreateInstance(
                    typeof(CommandHandlerWrapperWrapper<,>).MakeGenericType(type, typeof(TResponse)))!);
            return handler.Handle(command, _serviceProvider, cancellationToken);
        }



        public Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var type = query.GetType();
            var handler = (IQueryHandlerWrapper<TResponse>)QueryHandlerWrappers.GetOrAdd(type,
                t => Activator.CreateInstance(
                    typeof(QueryHandlerWrapper<,>).MakeGenericType(type, typeof(TResponse)))!);
            return handler.Handle(query, _serviceProvider, cancellationToken);
        }
    }
}
