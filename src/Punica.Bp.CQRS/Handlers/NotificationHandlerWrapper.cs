using Microsoft.Extensions.DependencyInjection;

namespace Punica.Bp.CQRS.Handlers
{
    public interface INotificationHandlerWrapper
    {
        Task Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
    
    public class NotificationHandlerWrapper<TNotification> : INotificationHandlerWrapper
    {
        public async Task Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
            await Handle((TNotification)request, serviceProvider, cancellationToken).ConfigureAwait(false);


        public async Task Handle(TNotification notification, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var handlers = serviceProvider.GetServices<INotificationHandler<TNotification>>();

            foreach (var handler in handlers)
            {
               await handler.Handle(notification, cancellationToken);
            }
           
        }
    }
}
