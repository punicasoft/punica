using Punica.Bp.Ddd.Domain.Entities;

namespace Punica.Bp.Ddd.Domain.Events
{
    public class EntityDeletedDomainEvent<TAggregate> : IDomainEvent where TAggregate : IAggregateRoot
    {
        public TAggregate Entity { get; init; }
        public DateTime EventTime { get; }

        public EntityDeletedDomainEvent(TAggregate entity, DateTime eventTime)
        {
            Entity = entity;
            EventTime = eventTime;
        }

    }
}
