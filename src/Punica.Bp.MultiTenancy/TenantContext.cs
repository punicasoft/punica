namespace Punica.Bp.MultiTenancy
{
    public class TenantContext : ITenantContext
    {
        public Guid? TenantId { get; } = Guid.Parse("AFBE4C05-8561-4BFD-959A-C25C900D883E");
    }
}
