namespace Punica.Bp.MultiTenancy
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; }
    }
}
