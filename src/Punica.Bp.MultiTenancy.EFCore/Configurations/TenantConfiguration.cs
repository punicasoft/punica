using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.MultiTenancy.EFCore.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration
    {
        public void Configure<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            if (typeof(TEntity).IsAssignableTo(typeof(IMultiTenant)))
            {
                builder.Property(nameof(IMultiTenant.TenantId))
                    .IsRequired(false)
                    .HasColumnName(nameof(IMultiTenant.TenantId));
            }
        }
    }
}
