using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Punica.Bp.EFCore.Configurations
{
    public class NullableDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
    {
        public NullableDateTimeValueConverter() : base( x => x.HasValue ? x.Value.ToUniversalTime() : x, x => x.HasValue ? x.Value.ToUniversalTime() : x)
        {
        }
    }
}
