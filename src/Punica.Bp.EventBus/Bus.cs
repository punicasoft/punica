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
            await _mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
        }
    }
}
