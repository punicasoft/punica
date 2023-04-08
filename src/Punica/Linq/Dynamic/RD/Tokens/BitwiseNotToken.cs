using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class BitwiseNotToken : Operation
    {
        public override bool IsLeftAssociative => false;
        public override short Precedence => 13;
        public override ExpressionType ExpressionType => ExpressionType.Not;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            return Expression.Not(stack.Pop());
        }
    }
}
