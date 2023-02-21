using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Punica.Bp.Ddd.Domain.Entities;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.Ddd.EFCore.Configurations
{
    public class DomainConfiguration : IEntityTypeConfiguration
    {
        public void Configure<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            var type = typeof(TEntity);

            if (type.IsAssignableTo(typeof(IEntity)))
            {
                var navigations = builder.Metadata.GetNavigations();

                foreach (var navigation in navigations)
                {
                    navigation.SetIsEagerLoaded(true);
                }
            }
        }

        public bool Ignore<TEntity>()
        {
            if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return false;
        }
    }
}
