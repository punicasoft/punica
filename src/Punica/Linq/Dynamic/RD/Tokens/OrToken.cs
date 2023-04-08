using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class OrToken : Operation
    {
        public override short Precedence => 3;
        public override ExpressionType ExpressionType => ExpressionType.OrElse;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();
            return Expression.OrElse(left, right);
        }
    }
}
