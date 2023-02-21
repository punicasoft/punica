using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Punica.Bp.EFCore.Middleware;
using Punica.Bp.EFCore.Extensions;

namespace Punica.Bp.Auditing.EFCore.Configurations
{
    public class AuditEntityConfigurations : IEntityTypeConfiguration
    {
        public void Configure<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            var type = typeof(TEntity);

            if (type.IsAssignableTo(typeof(ICreatedDate)))
            {
                builder.Property(nameof(ICreatedDate.CreatedOn))
                    .IsRequired()
                    .HasColumnName(nameof(ICreatedDate.CreatedOn));
            }

            if (type.IsAssignableTo(typeof(ICreatedBy)))
            {
                builder.Property(nameof(ICreatedBy.CreatedBy))
                    .IsRequired()
                    .HasColumnName(nameof(ICreatedBy.CreatedBy));
            }

            if (type.IsAssignableTo(typeof(IModifiedDate)))
            {
                builder.Property(nameof(IModifiedDate.ModifiedOn))
                    .IsRequired()
                    .HasColumnName(nameof(IModifiedDate.ModifiedOn));
            }

            if (type.IsAssignableTo(typeof(IModifiedBy)))
            {
                builder.Property(nameof(IModifiedBy.ModifiedBy))
                    .IsRequired()
                    .HasColumnName(nameof(IModifiedBy.ModifiedBy));
            }

            if (type.IsAssignableTo(typeof(IDeletedDate)))
            {
                builder.Property(nameof(IDeletedDate.DeletedOn))
                    .IsRequired(false)
                    .HasColumnName(nameof(IDeletedDate.DeletedOn));
            }

            if (type.IsAssignableTo(typeof(IDeletedBy)))
            {
                builder.Property(nameof(IDeletedBy.DeletedBy))
                    .IsRequired()
                    .HasColumnName(nameof(IDeletedBy.DeletedBy));
            }

            if (type.IsAssignableTo(typeof(ISoftDeletable)))
            {
                builder.Property(nameof(ISoftDeletable.Deleted))
                    .IsRequired()
                    .HasColumnName(nameof(ISoftDeletable.Deleted));

                builder.AddQueryFilter<ISoftDeletable>(e =>  e.Deleted == false);

                //Expression<Func<TEntity, bool>> expression = e => !EF.Property<bool>(e, nameof(ISoftDeletable.Deleted));

               // builder.HasQueryFilter(expression);//TODO fix queries
            }

        }

        public bool Ignore<TEntity>()
        {
            return false;
        }
    }
}
