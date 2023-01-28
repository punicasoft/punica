using Punica.Bp.Ddd.Domain.Entities;

namespace Punica.Bp.Ddd.Domain.Events
{
    public class EntityCreatedDomainEvent<TAggregate> : IDomainEvent where TAggregate : IAggregateRoot
    {
        public TAggregate Entity { get; init; }
        public DateTime EventTime { get; }

        public EntityCreatedDomainEvent(TAggregate entity, DateTime eventTime)
        {
            Entity = entity;
            EventTime = eventTime;
        }

    }
}
