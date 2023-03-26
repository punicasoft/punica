using Microsoft.EntityFrameworkCore;
using Punica.Linq.Dynamic;

namespace Punica.Bp.EFCore.Query
{
    public class QueryBase
    {
        private readonly DbContext _dbContext;
       

        public QueryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // TODO: add where facility again
        protected List<dynamic> GetList<TEntity>(string columns, string filter) where TEntity : class
        {
            var query = _dbContext.Set<TEntity>().AsNoTracking();

            Evaluator evaluator = new Evaluator(typeof(IQueryable<TEntity>), null);
            var expression1 = TextParser.Evaluate(columns, evaluator);

            var resultExpression = evaluator.GetFilterExpression<IQueryable<TEntity>, IQueryable<dynamic>>(expression1[0]);

            var actual = resultExpression.Compile()(query).ToList();

            return actual;

        }

    }


}
