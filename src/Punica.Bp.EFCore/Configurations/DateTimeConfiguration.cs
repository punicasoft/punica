using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.EFCore.Configurations
{
    public class DateTimeConfiguration : IEntityTypeConfiguration
    {
        public void Configure<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            foreach (var property in builder.Metadata.GetProperties().
                         Where(property => property.PropertyInfo != null &&
                                           (property.PropertyInfo.PropertyType == typeof(DateTime) || property.PropertyInfo.PropertyType == typeof(DateTime?)) &&
                                           property.PropertyInfo.CanWrite))
            {
                builder.Property(property.Name)
                    .HasConversion(property.ClrType == typeof(DateTime)
                        ? new DateTimeValueConverter()
                        : new NullableDateTimeValueConverter());
            }
        }

        public bool Ignore<TEntity>()
        {
            return false;
        }
    }
}
