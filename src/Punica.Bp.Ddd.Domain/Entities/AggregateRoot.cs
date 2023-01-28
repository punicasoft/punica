using Punica.Bp.Ddd.Domain.Events;

namespace Punica.Bp.Ddd.Domain.Entities
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public virtual IEnumerable<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents;
        }

        public virtual void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected virtual void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        protected virtual void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
    }

    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey> where TKey : struct
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        public virtual IEnumerable<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents;
        }

        public virtual void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        protected virtual void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        protected virtual void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
    }
}
