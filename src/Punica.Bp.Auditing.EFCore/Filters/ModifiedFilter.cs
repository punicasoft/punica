﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Punica.Bp.Core;
using Punica.Bp.EFCore.Extensions;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.Auditing.EFCore.Filters
{
    public class ModifiedFilter:ITrackingFilter
    {
        private readonly IUserContext _userContext;
        private readonly IDateTime _dateTime;

        public ModifiedFilter(IUserContext userContext, IDateTime dateTime)
        {
            _userContext = userContext;
            _dateTime = dateTime;
        }

        public Task BeforeSave(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            if (entry.State == EntityState.Modified)
            {
                var type = entry.Metadata.ClrType;

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
