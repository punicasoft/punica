namespace Punica.Bp.MultiTenancy
{
    public interface ITenantContext
    {
        Guid? TenantId { get; }
    }
}
