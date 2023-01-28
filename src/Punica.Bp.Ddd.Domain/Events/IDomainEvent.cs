namespace Punica.Bp.Ddd.Domain.Events
{
    public interface IDomainEvent
    {
        public DateTime EventTime { get; }
    }
}
