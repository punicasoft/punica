using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Punica.Bp.Ddd.Domain.Entities;
using Punica.Bp.Ddd.Domain.Events;
using Punica.Bp.Ddd.EFCore.Filters.Events;
using Punica.Bp.EFCore.Extensions;
using Punica.Bp.EFCore.Middleware;
using Punica.Bp.EventBus;

namespace Punica.Bp.Ddd.EFCore.Filters
{
    public class DomainEventFilter : ITrackingFilter
    {
        private readonly List<IDomainEvent> _events;
        private readonly IEventTriggerCache _triggerCache;
        private readonly IDateTime _dateTime;
        private readonly IBus _bus;
        
        public DomainEventFilter(IEventTriggerCache triggerCache, IDateTime dateTime, IBus bus)
        {
            _triggerCache = triggerCache;
            _dateTime = dateTime;
            _bus = bus;
            _events = new List<IDomainEvent>();
        }

        public Task BeforeSave(EntityEntry entry, CancellationToken cancellationToken = default)
        {
            var type = entry.Metadata.ClrType;

            if (!type.IsAssignableTo(typeof(IAggregateRoot)))
            {
                return Task.CompletedTask;
            }

            if (entry.State == EntityState.Added && _triggerCache.IsCreatedEventEnabled(type))
            {
                var constructed = typeof(EntityCreatedDomainEvent<>).MakeGenericType(entry.Metadata.ClrType);
                object? o = Activator.CreateInstance(constructed, entry.Entity, _dateTime.UtcNow);
                if (o != null)
                {
                    _events.Add((IDomainEvent)o);
                }

            }
            else if (entry.State == EntityState.Modified && _triggerCache.IsModifiedEventEnabled(type))
            {
                var constructed = typeof(EntityModifiedDomainEvent<>).MakeGenericType(entry.Metadata.ClrType);
                object? o = Activator.CreateInstance(constructed, entry.Entity, _dateTime.UtcNow);
                if (o != null)
                {
                    _events.Add((IDomainEvent)o);
                }
            }
            else if (entry.State == EntityState.Deleted && _triggerCache.IsDeletedEventEnabled(type))
            {
                var constructed = typeof(EntityDeletedDomainEvent<>).MakeGenericType(entry.Metadata.ClrType);
                object? o = Activator.CreateInstance(constructed, entry.Entity, _dateTime.UtcNow);
                if (o != null)
                {
                    _events.Add((IDomainEvent)o);
                }
            }

            var entityEntry = entry.As<IAggregateRoot>();

            var events = entityEntry.Entity.GetDomainEvents();
            entityEntry.Entity.ClearDomainEvents();
            _events.AddRange(events);


            return Task.CompletedTask;
        }

        public async Task<int> AfterSave(int result, CancellationToken cancellationToken = default)
        {
            foreach (var domainEvent in _events)
            {
                await _bus.Publish(domainEvent, cancellationToken);
            }

            return result;
        }
    }
}
