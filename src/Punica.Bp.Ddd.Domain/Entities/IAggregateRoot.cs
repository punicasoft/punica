using Punica.Bp.Ddd.Domain.Events;

namespace Punica.Bp.Ddd.Domain.Entities
{
    public interface IAggregateRoot : IEntity
    {
        IEnumerable<IDomainEvent> GetDomainEvents();
        void ClearDomainEvents();
    }

    public interface IAggregateRoot<TKey> : IEntity<TKey>, IAggregateRoot where TKey : struct
    {
        
    }
}
