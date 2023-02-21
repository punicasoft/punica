using Punica.Bp.CQRS.Handlers;
using Punica.Bp.Ddd.Domain.Repository;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders.Commands
{
    public class CreateOrderRequestHandler : ICommandHandler<CreateOrderRequest, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateOrderRequest command, CancellationToken cancellationToken)
        {
            var buyer = new Buyer
            {
                Email = command.BuyerEmail,
                Name = command.BuyerName,
            };

            var order = new Order(buyer);

            foreach (var item in command.Items)
            {
                order.AddItem(new OrderItem(item.ProductId, item.ProductName, item.Price, item.Units));
            }

            var repository = _unitOfWork.GetRepository<Order>();

            await repository.InsertAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
