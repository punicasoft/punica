using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD
{
    public class TokenContext
    {
        public string Text { get; }

        public int Depth { get; }

        public Expression? ParameterInstance { get; set; }

        public ParameterExpression? Parameter { get; set; }

        public TokenContext(string text) : this(text, 0)
        {
        }

        public TokenContext(string text, int depth)
        {
            Text = text;
            Depth = depth;
        }

        public TokenContext(TokenContext context)
        {
            Text = context.Text;
            Depth = context.Depth + 1;
            ParameterInstance = context.ParameterInstance;
            Parameter = context.Parameter;
        }

        public TokenContext(TokenContext context, ParameterExpression? arg)
        {
            Text = context.Text;
            Depth = context.Depth + 1;
            ParameterInstance = context.ParameterInstance;
            Parameter = arg;
        }

    }
}
