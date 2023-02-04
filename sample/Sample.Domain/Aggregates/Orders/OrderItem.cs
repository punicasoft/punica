using Punica.Bp.Ddd.Domain.Entities;

namespace Sample.Domain.Aggregates.Orders
{
    public class OrderItem : Entity<Guid>
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set;}

        public decimal UnitPrice { get; private set;}
        public int Units { get; private set;}

        public OrderItem(Guid productId, string productName, decimal unitPrice, int units)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Units = units;
        }

        public void AddUnit(int count)
        {
            Units += count;
        }

        public void RemoveUnit(int count)
        {
            Units -= count;
        }
    }
}
