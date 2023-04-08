using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class SubtractToken : Operation
    {
        public override short Precedence => 11;
        public override ExpressionType ExpressionType => ExpressionType.Subtract;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();

            if (stack.Count == 0)
            {
                return Expression.Negate(right);
            }

            var left = stack.Pop();
            return Expression.Subtract(left, right);
        }
    }
}
