using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.EFCore;
using Sample.Domain.Aggregates.Orders;

namespace Sample.Infrastructure
{
    public class OrderDbContext : BpDbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
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
                b.OwnsOne(o => o.Buyer,
                    a =>
                    {
                        a.WithOwner();
                        a.Property(e => e.Name).HasMaxLength(30);
                        a.Property(e => e.Email).HasMaxLength(100);
                    });
                  

                b.Navigation(q => q.Items).UsePropertyAccessMode(PropertyAccessMode.Property);

                b.Property(q => q.Status)
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.ToTable("OrderItems", Schema);
                
                b.Property(q => q.ProductId).IsRequired();
                b.Property(q => q.ProductName).IsRequired().HasMaxLength(200);
                b.Property(q => q.UnitPrice).IsRequired().HasPrecision(10,2);
                b.Property(q => q.Units).IsRequired();
            });
        }
    }
}
