namespace Punica
{
    public class BasicDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
