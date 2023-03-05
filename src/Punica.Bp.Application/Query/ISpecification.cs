using System.Linq.Expressions;

namespace Punica.Bp.Application.Query
{
    public interface ISpecification<TEntity>
    {
       public Expression<Func<TEntity, bool>> Expression { get; set; }
    }
}
