namespace Punica
{
    public interface IDateTime
    {
        DateTime Now { get;}
        DateTime UtcNow { get; }
    }
}
