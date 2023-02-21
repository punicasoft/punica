using Punica.Bp.Auditing;
using Punica.Bp.Ddd.Domain.Entities;
using Punica.Bp.Ddd.Domain.Events;
using Punica.Bp.MultiTenancy;

namespace Sample.Domain.Aggregates.Orders
{
    [EnableCreatedEvent]
    public class Order : AggregateRoot<Guid>, IAuditableEntity, IMultiTenant
    {
        public string Status { get; private set; }
        public DateTime OrderDate { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public Buyer Buyer { get; private set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid DeletedBy { get; set; }
        public Guid? TenantId { get; private set; }

        private Order()
        {
            Items = new List<OrderItem>();
            Status = "New";
        }

        public Order(Buyer buyer): this()
        {
            Buyer = buyer;
            OrderDate = DateTime.UtcNow;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }

        public void Processing()
        {
            Status = "Processing";
        }

    }
}
