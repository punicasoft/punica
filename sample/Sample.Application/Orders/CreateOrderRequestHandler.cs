using Punica.Bp.CQRS.Handlers;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders
{
    public class CreateOrderRequestHandler : ICommandHandler<CreateOrderRequest, Guid>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderRequestHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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

            await _orderRepository.InsertAsync(order, cancellationToken);

            await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
