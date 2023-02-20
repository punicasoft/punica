using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.Domain.Repository;
using Punica.Bp.EFCore;

namespace Punica.Bp.Ddd.EFCore
{
    public abstract class BpDbContext : DbContextBase, IUnitOfWork
    {
        protected BpDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
