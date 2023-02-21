using Punica.Bp.CQRS.Messages;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders.Queries
{
    public class GetOrderQuery : IQuery<Order?>
    {
        public Guid OrderId { get; set; }
    }
}
