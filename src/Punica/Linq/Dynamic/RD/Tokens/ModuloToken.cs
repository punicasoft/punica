using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class ModuloToken : Operation
    {
        public override short Precedence => 12;
        public override ExpressionType ExpressionType => ExpressionType.Modulo;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();
            var tuple = ConvertExpressions(left, right);
            return Expression.Modulo(tuple.left, tuple.right);
        }
    }
}
