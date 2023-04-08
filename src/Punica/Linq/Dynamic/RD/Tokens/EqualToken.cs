using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class EqualToken :Operation
    {
        public override short Precedence => 8;
        public override ExpressionType ExpressionType => ExpressionType.Equal;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();
            return Expression.Equal(left, right);
        }
    }
}
