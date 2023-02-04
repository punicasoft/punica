using Microsoft.EntityFrameworkCore;
using Punica.Bp.Ddd.Domain.Repository;
using Punica.Bp.EFCore;

namespace Punica.Bp.Ddd.EFCore
{
    public class BpDbContext : DbContextBase, IUnitOfWork
    {
        public BpDbContext(DbContextOptions<BpDbContext> options) : base(options)
        {
        }
    }
}
