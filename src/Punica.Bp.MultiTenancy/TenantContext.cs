using Microsoft.AspNetCore.Http;

namespace Punica.Bp.MultiTenancy
{
    public class TenantContext : ITenantContext
    {
        public Guid? TenantId { get; }

        public TenantContext(IHttpContextAccessor accessor)
        {
            var tid = (string)accessor.HttpContext.Request.Headers["TenantId"];
            if (string.IsNullOrEmpty(tid))
            {
                tid = "AFBE4C05-8561-4BFD-959A-C25C900D883E";
            }

            TenantId  = Guid.Parse(tid);
        }
    }
}
