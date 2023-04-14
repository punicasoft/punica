using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD
{
    /// <summary>
    /// Arguments are external inputs to expressions
    /// VariablesInstance is similar to arguments but used as a parameter for expressions so that sql conversion use it as a parameter //TODO: check whether you can merge Arguments and VariablesInstance together?
    /// <see cref="MethodToken"/> for rest of details. but there could be lambda expression at <see cref="RootToken"/> level
    /// </summary>
    public class TokenContext
    {
        public string Text { get; }

        /// <summary>
        /// Used to pass parameters for expressions @param where param is a property/field of VariablesInstance
        /// </summary>
        public Expression? VariablesInstance { get; }

        /// <summary>
        /// Arguments
        /// </summary>
        public IExpression[] Argumentss { get; }

        public MethodContext MethodContext { get; }


        public TokenContext(string text, MethodContext? methodContext = null, Expression? variablesInstance = null, params IExpression[] args)
        {
            Text = text;
            VariablesInstance = variablesInstance;
            Argumentss = args;
            MethodContext = methodContext ?? new MethodContext();
        }


        //public TokenContext(string text, MethodContext? methodContext = null): this(text, methodContext, null)
        //{

        //}

        //internal TokenContext(string text, int depth, IExpression? arguments = null, Expression? variablesInstance = null)
        //{
        //    Text = text;
        //    Depth = depth;
        //    VariablesInstance = variablesInstance;
        //    Arguments = arguments;
        //}

        //public TokenContext(string text, IExpression? parameter = null, Expression? parameterInstance = null) : this(text, 0, parameter, parameterInstance)
        //{
        //}

        //public TokenContext(string text, ParameterExpression parameter) : this(text, 0, new ParameterToken(parameter), null)
        //{
        //}



    }
}
