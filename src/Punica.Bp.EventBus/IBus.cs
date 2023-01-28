using Punica.Bp.Ddd.Domain.Events;

namespace Punica.Bp.EventBus
{
    public interface IBus
    {
        Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
