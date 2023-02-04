using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Punica.Bp.EFCore.Configurations
{
    public class DateTimeValueConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeValueConverter() : base(x => x.ToUniversalTime(), x => x.ToUniversalTime())
        {
        }
    }
}
