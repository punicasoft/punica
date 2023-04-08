using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD
{
    public class TokenContext
    {
        public string Text { get; }

        public int Depth { get; }

        public Expression? ParameterInstance { get; }

        public IExpression? Parameter { get;}


        internal TokenContext(string text, int depth, IExpression? parameter = null, Expression? parameterInstance = null)
        {
            Text = text;
            Depth = depth;
            ParameterInstance = parameterInstance;
            Parameter = parameter;
        }

        public TokenContext(string text, IExpression? parameter = null, Expression? parameterInstance = null) : this(text, 0, parameter, parameterInstance)
        {
        }

        public TokenContext(string text, ParameterExpression parameter, Expression? parameterInstance = null) : this(text, 0, new ParameterToken(parameter), parameterInstance)
        {
        }

        //public TokenContext(TokenContext context)
        //{
        //    Text = context.Text;
        //    Depth = context.Depth + 1;
        //    ParameterInstance = context.ParameterInstance;
        //    Parameter = context.Parameter;
        //}

        //public TokenContext(TokenContext context, ParameterExpression? arg)
        //{
        //    Text = context.Text;
        //    Depth = context.Depth + 1;
        //    ParameterInstance = context.ParameterInstance;
        //    Parameter = arg;
        //}

    }
}
