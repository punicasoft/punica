using Punica.Bp.CQRS.Handlers;
using Punica.Bp.Ddd.Domain.Events;
using Punica.Bp.Ddd.Domain.Repository;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders.Commands
{
    public class OrderCreatedEventHandler : IDomainEventHandler<EntityCreatedDomainEvent<Order>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderCreatedEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(EntityCreatedDomainEvent<Order> notification, CancellationToken cancellationToken)
        {
            notification.Entity.Processing();

            var repository = _unitOfWork.GetRepository<Order>();
            await repository.UpdateAsync(notification.Entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
