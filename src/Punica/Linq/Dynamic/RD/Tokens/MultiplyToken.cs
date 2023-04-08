using Punica.Linq.Dynamic.RD.Tokens.abstractions;
using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class MultiplyToken  : Operation
    {
        public override short Precedence => 12;
        public override ExpressionType ExpressionType => ExpressionType.Multiply; 
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();
            var tuple = ConvertExpressions(left, right);
            return Expression.Multiply(tuple.left, tuple.right);
        }
    }
}
