using Punica.Bp.CQRS.Messages;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders.Queries
{
    public class GetOrderQuery : IQuery<List<object>>
    {
        public Guid OrderId { get; set; }
        public string Select { get; set; }
        public string Filter { get; set; }
    }
}
