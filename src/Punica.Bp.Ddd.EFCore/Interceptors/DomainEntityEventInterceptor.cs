using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Punica.Bp.CQRS;
using Punica.Bp.Ddd.Domain.Entities;
using Punica.Bp.Ddd.Domain.Events;
using Punica.Bp.Ddd.EFCore.Interceptors.Events;
using Punica.Bp.EFCore.Extensions;
using Punica.Bp.EFCore.Middleware;

namespace Punica.Bp.Ddd.EFCore.Interceptors
{
    public class DomainEntityEventInterceptor : IEntityInterceptor
    {
        private readonly List<IDomainEvent> _events;
        private readonly IEventTriggerCache _triggerCache;
        private readonly IDateTime _dateTime;
        private readonly IPublisher _publisher;
        
        public DomainEntityEventInterceptor(IEventTriggerCache triggerCache, IDateTime dateTime, IPublisher publisher)
        {
            _triggerCache = triggerCache;
            _dateTime = dateTime;
            _publisher = publisher;
            _events = new List<IDomainEvent>();
        }

        public Task BeforeSavingAsync(EntityEntry entry, CancellationToken cancellationToken = default)
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
            else if (entry.State == EntityState.Modified && _triggerCache.IsModifiedEventEnabled(type)) // TODO: could lead looping if on the event entity modified and saved again
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

        public async Task<int> AfterSavingAsync(int result, CancellationToken cancellationToken = default)
        {
            foreach (var domainEvent in _events)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }

            return result;
        }

        public Task SavedFailedAsync(Exception exception, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
