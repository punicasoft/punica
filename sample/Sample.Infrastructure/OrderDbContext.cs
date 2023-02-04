using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.EFCore;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Infrastructure
{
    public class OrderDbContext : BpDbContext
    {
        public OrderDbContext(DbContextOptions<BpDbContext> options) : base(options)
        {
            Schema = "sample";
        }

        public string Schema { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Order>(b =>
            {
                b.ToTable(nameof(Order)+"s", Schema);
                b.OwnsOne(o => o.Buyer, a => { a.WithOwner(); });

                b.Navigation(q => q.Items).UsePropertyAccessMode(PropertyAccessMode.Property);

                b.Property(q => q.Status);
            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.ToTable("OrderItems", Schema);
                
                b.Property(q => q.ProductId).IsRequired();
                b.Property(q => q.ProductName).IsRequired();
                b.Property(q => q.UnitPrice).IsRequired();
                b.Property(q => q.Units).IsRequired();
            });
        }
    }
}
