namespace Punica.Bp.Ddd.Domain.Entities
{
    public abstract class Entity : IEntity
    {
    }

    public abstract class Entity<TKey> : IEntity<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
    }
}
