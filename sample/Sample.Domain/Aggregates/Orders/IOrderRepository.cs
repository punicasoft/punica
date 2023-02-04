using Punica.Bp.Ddd.Domain.Repository;

namespace Sample.Domain.Aggregates.Orders
{
    public interface IOrderRepository : IRepository<Order>
    {
    }
}
