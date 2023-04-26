using Microsoft.Extensions.DependencyInjection;

namespace Punica.Extensions.DependencyInjection
{
    public class AssemblyScanOptions
    {
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
        public bool RegisterOpenGenerics { get; set; } = false;
    }
}
