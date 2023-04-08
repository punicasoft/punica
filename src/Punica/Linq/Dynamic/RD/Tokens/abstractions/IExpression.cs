using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD.Tokens.abstractions
{
    public interface IExpression
    {
        Expression Evaluate();
    }
}
