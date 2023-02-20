using Punica.Bp.CQRS.Handlers;
using Punica.Bp.Ddd.Domain.Events;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders
{
    public class OrderCreatedEventHandler : IDomainEventHandler<EntityCreatedDomainEvent<Order>>
    {
        private readonly IOrderRepository _repository;

        public OrderCreatedEventHandler(IOrderRepository repository)
        {
            _repository = repository;
        }


        public async Task Handle(EntityCreatedDomainEvent<Order> notification, CancellationToken cancellationToken)
        {
            notification.Entity.Processing();

            await _repository.UpdateAsync(notification.Entity, cancellationToken);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
