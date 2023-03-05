using Punica.Bp.CQRS.Messages;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Application.Orders.Queries
{
    public class GetOrderQuery : IQuery<List<object>>
    {
        public Guid OrderId { get; set; }
        public List<string> Columns { get; set; }
        public string Filter { get; set; }
    }
}
