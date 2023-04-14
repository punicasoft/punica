using System.Linq.Expressions;
using Punica.Extensions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;
using Punica.Reflection;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    /// <summary>
    /// So method token you might need lambada expressions for each parameter if the parameter is function
    /// </summary>
    public class MethodToken : Operation, ITokenList, IExpression
    {
        //private readonly int _depth;
        public string MethodName { get; }
        public IExpression MemberExpression { get; } // TODO support chaining of methods
        private IExpression? Parameter { get; }
        public List<IToken> Tokens { get; }
        public override bool IsLeftAssociative => false;
        public override short Precedence => 14;
        public override ExpressionType ExpressionType => ExpressionType.Call;
        

        public MethodToken(string methodName, IExpression memberExpression, IExpression parameter)
        {
           // _depth = depth;
            MethodName = methodName;
            MemberExpression = memberExpression;
            Tokens = new List<IToken>();
            Parameter = parameter;
           // Parameter = new ParameterToken(memberExpression, "arg" + _depth);
        }

        public ParameterToken[]? GetParameter(int depth)
        {
            var argNo = Tokens.Count +1;
            var isFunction = MethodHandler.Instance.IsFunction(MethodName, argNo);

            if (isFunction)
            {
                //TODO handle other argNo on different methods
                
                if (MethodName == "GroupBy")
                {
                    switch (argNo)
                    {
                        case 1: return new ParameterToken[] { new ParameterToken(MemberExpression, "arg" + depth) };
                        case 2: return new ParameterToken[] { new ParameterToken(MemberExpression, "arg" + depth) };
                    }
                }

                if (MethodName == "SelectMany")
                {
                    switch (argNo)
                    {
                        case 1: return new ParameterToken[] { new ParameterToken(MemberExpression, "arg" + depth) };
                        //case 2: return new ParameterToken[] { new ParameterToken(MemberExpression, "arg" + depth), new ParameterToken(Tokens[0], "arg" + depth) };//TODO there is issue with processing of the second parameter
                    }
                }

                return new ParameterToken[]{new ParameterToken(MemberExpression, "arg" + depth)};
            }

            return null;
        }

        public void AddToken(IToken token)
        {
            Tokens.Add(token);
        }

        public override Expression Evaluate(Stack<Expression> stack)
        {
            return Evaluate();
        }

        //IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        //                     SelectMany<TSource, TCollection, TResult>(MemberExpression                , Tokens[0]                                                 , Tokens[1])
        public Expression Evaluate()
        {
            var memberExpression = MemberExpression.Evaluate();
            var parameter = (ParameterExpression)Parameter.Evaluate();

            if (memberExpression.Type.IsCollection())
            {


                List<Expression> expressions = new List<Expression>();
                foreach (var token in Tokens)
                {
                    var list = token as ITokenList;

                    var expression = Process(list.Tokens);

                    expressions.Add(expression);
                }

                var methodHandler = MethodHandler.Instance.GetHandler(memberExpression.Type);

                return methodHandler.CallMethod(MethodName, memberExpression, parameter, expressions.ToArray());

            }
            else
            {
                switch (MethodName)
                {
                    default:
                        throw new ArgumentException($"Invalid method {MethodName}");
                }

                //TODO find the method find the expression
                // return Expression.Call(CachedMethodInfo.EnumerableContainsMethod(e1.Type), right);
            }
        }
    }


}
