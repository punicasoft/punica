using Punica.Bp.CQRS.Handlers;
using Punica.Bp.Ddd.Domain.Repository;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders.Commands
{
    public class AddItemToOrderCommandHandler : ICommandHandler<AddItemToOrderCommand>
    {

        private readonly IUnitOfWork _unitOfWork;

        public AddItemToOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddItemToOrderCommand command, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Order>();

            var order = await repository.FindAsync(command.OrderId, cancellationToken);

            if (order == null)
            {
                throw new Exception("Order Not Found");
            }

            order.AddItem(new OrderItem(command.ProductId, command.ProductName, command.Price, command.Units));

            await repository.UpdateAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
