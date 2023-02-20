using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Punica.Bp.EFCore.Middleware
{
    public interface IEntityTypeConfiguration
    {
        public void Configure<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class;

        public bool Ignore<TEntity>();
    }
}
