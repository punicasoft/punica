namespace Punica.Bp.CQRS.Handlers
{
    public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    {
    }
}
