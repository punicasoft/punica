namespace Punica.Bp.CQRS.Handlers
{
    public interface INotificationHandler<in TNotification>
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}
