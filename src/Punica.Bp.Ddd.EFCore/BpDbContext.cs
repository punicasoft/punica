using Microsoft.EntityFrameworkCore;
using Punica.Bp.EFCore;

namespace Punica.Bp.Ddd.EFCore
{
    public abstract class BpDbContext : DbContextBase
    {
        protected BpDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
