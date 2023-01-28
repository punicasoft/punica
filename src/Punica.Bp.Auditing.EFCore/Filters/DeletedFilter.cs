using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Punica.Bp.Core;
using Punica.Bp.EFCore.Extensions;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.Auditing.EFCore.Filters
{
    public class DeletedFilter :ITrackingFilter
    {
        private readonly IUserContext _userContext;
        private readonly IDateTime _dateTime;

        public DeletedFilter(IUserContext userContext, IDateTime dateTime)
        {
            _userContext = userContext;
            _dateTime = dateTime;
        }

        public Task BeforeSave(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            if (entry.State == EntityState.Deleted)
            {
                var type = entry.Metadata.ClrType;

                if (!type.IsAssignableTo(typeof(ISoftDeletable)))
                {
                    return Task.CompletedTask; 
                }

                if (type.IsAssignableTo(typeof(ISoftDeletable)))
                {
                    entry.As<ISoftDeletable>().Property(p => p.Deleted).CurrentValue = true;
                }

                if (type.IsAssignableTo(typeof(IDeletedDate)))
                {
                    entry.As<IDeletedDate>().Property(p => p.DeletedOn).CurrentValue = _dateTime.UtcNow;
                }

                if (type.IsAssignableTo(typeof(IDeletedBy)))
                {
                    entry.As<IDeletedBy>().Property(p => p.DeletedBy).CurrentValue = _userContext.UserId;
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
