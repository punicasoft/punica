﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Punica.Bp.Core;
using Punica.Bp.EFCore.Extensions;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.Auditing.EFCore.Filters
{
    public class AddedFilter : ITrackingFilter
    {
        private readonly IUserContext _userContext;
        private readonly IDateTime _dateTime;

        public AddedFilter(IUserContext userContext, IDateTime dateTime)
        {
            _userContext = userContext;
            _dateTime = dateTime;
        }

        public Task BeforeSave(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            if (entry.State == EntityState.Added)
            {
              
                var type = entry.Metadata.ClrType;

                if (type.IsAssignableTo(typeof(ICreatedDate)))
                {
                    entry.As<ICreatedDate>().Property(p => p.CreatedOn).CurrentValue = _dateTime.UtcNow;
                }

                if (type.IsAssignableTo(typeof(ICreatedBy)))
                {
                    entry.As<ICreatedBy>().Property(p => p.CreatedBy).CurrentValue = _userContext.UserId;
                }

                //var implementedType = type.GetImplementedType(typeof(ICreatedBy<IEntity<Guid>>));

                //if (implementedType != null)
                //{
                //    dynamic entity = Activator.CreateInstance(implementedType);
                //    entity.Id = _userContext.UserId;
                //    //PropertyInfo idProp = genericType.GetProperty("Id");
                //    //idProp.SetValue(entity, _userContext.UserId);

                //    setter.SetNavigation(nameof(ICreatedBy<object>.CreatedBy), entity);

                //}


                if (type.IsAssignableTo(typeof(IModifiedDate)))
                {
                    entry.As<IModifiedDate>().Property(p => p.ModifiedOn).CurrentValue = _dateTime.UtcNow;
                }

                if (type.IsAssignableTo(typeof(IModifiedBy)))
                {
                    entry.As<IModifiedBy>().Property(p => p.ModifiedBy).CurrentValue = _userContext.UserId;
                }
            }

            return Task.CompletedTask;
        }

        public Task<int> AfterSave(int result, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(result);
        }
    }
}
