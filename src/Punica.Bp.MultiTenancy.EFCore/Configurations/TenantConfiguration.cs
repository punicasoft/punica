using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Punica.Bp.EFCore.Extensions;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.MultiTenancy.EFCore.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration
    {
        private readonly DbContext _dbContext;

        public TenantConfiguration(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// https://github.com/dotnet/efcore/issues/14740
        /// If you inject <see cref="ITenantContext"/> to _tenantContext private field, what EF see in expression tree for that query filter is TenantConfiguration._tenantContext.TenandId.
        /// EF do not have a way to know how this _tenantContext's value is connected with DbContext.
        /// So there is no way to update the value, hence EF stick a constant. (parameterization happens only for things which can be changed in expression tree for better catching)
        /// If you want to configure QueryFilter with runtime value out side OnModelCreating, then pass DbContext in ctor to your entity configuration.
        /// So that EF can still inject current DbContext instance to calculate value. Basically EF will inject instance of DbContext if it referred in query filter
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        public void Configure<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            if (typeof(TEntity).IsAssignableTo(typeof(IMultiTenant)))
            {
                builder.Property(nameof(IMultiTenant.TenantId))
                    .IsRequired(false)
                    .HasColumnName(nameof(IMultiTenant.TenantId));

                //_dbContext.GetService<ITenantContext>().TenantId will work but if you assign _dbContext.GetService<ITenantContext>() to any variable and use variable.TenantId this will not work
                builder.AddQueryFilter<IMultiTenant>(e => e.TenantId == _dbContext.GetService<ITenantContext>().TenantId);
            }
        }

        public bool Ignore<TEntity>()
        {
            return false;
        }
    }
}
