using Punica.Bp.CQRS.Handlers;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders
{
    public class CreateOrderRequestHandler : ICommandHandler<CreateOrderRequest, Guid>
    {


        public Task<Guid> Handle(CreateOrderRequest command, CancellationToken cancellationToken)
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


            return Task.FromResult(Guid.NewGuid());
        }
    }
}
