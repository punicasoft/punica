using MediatR;
using Punica.Bp.Ddd.Domain.Events;

namespace Punica.Bp.EventBus
{
    public class Bus : IBus
    {
        private readonly IMediator _mediator;

        public Bus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await _mediator.Publish(new DomainEvent<IDomainEvent>(domainEvent), cancellationToken).ConfigureAwait(false);
        }
    }

    public interface IDomainEvent<out TEvent> : INotification where TEvent : IDomainEvent
    {
        TEvent Event { get; }
    }

    public class DomainEvent<TEvent> : IDomainEvent<TEvent> where TEvent : IDomainEvent
    {
        public TEvent Event { get; }

        public DomainEvent(TEvent @event)
        {
            Event = @event;
        }
    }


    public interface IDomainEventHandler<in TEvent> : INotificationHandler<IDomainEvent<TEvent>>
        where TEvent : IDomainEvent
    {
       
    }
    
}
