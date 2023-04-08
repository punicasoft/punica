using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class RootToken : Operation, ITokenList
    {
        public IExpression? Parameter { get; set; }
        public List<IToken> Tokens { get; }

        public override short Precedence => 0;
        public override ExpressionType ExpressionType => ExpressionType.Lambda;


        public RootToken(IExpression? argExpression, List<IToken> tokens)
        {
            Parameter = argExpression;
            Tokens = tokens;
        }

        public override Expression Evaluate(Stack<Expression> stack)
        {
            var expression = Process(Tokens);

            if (Parameter == null)
            {
                return Expression.Lambda(expression);
            }
           
            return Expression.Lambda(expression, (ParameterExpression)Parameter.Evaluate());
        }


        public void AddToken(IToken token)
        {
            throw new NotImplementedException();
        }
    }
}
