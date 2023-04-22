using Punica.Linq.Dynamic.RD.Tokens.abstractions;
using Punica.Linq.Dynamic.RD.Tokens;
using Punica.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Punica.Extensions;

namespace Punica.Linq.Dynamic.RD.Rd2
{
    public class MethodToken3 : IExpressionToken
    {
        //private readonly int _depth;
        public string MethodName { get; }
        public IExpression MemberExpression { get; } 
        private IExpression? Parameter { get; }
        public List<Argument> Tokens { get; }
        public bool IsLeftAssociative => false;
        public short Precedence => 14;
        public TokenType TokenType => TokenType.Operator;
        public ExpressionType ExpressionType => ExpressionType.Call;


        public MethodToken3(string methodName, IExpression memberExpression)
        {
            // _depth = depth;
            MethodName = methodName;
            MemberExpression = memberExpression;
            Tokens = new List<Argument>();
            //Parameter = parameter;
            // Parameter = new ParameterToken(memberExpression, "arg" + _depth);
        }

        public void AddToken(Argument token)
        {
            Tokens.Add(token);
        }

        //IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        //                     SelectMany<TSource, TCollection, TResult>(MemberExpression                , Tokens[0]                                                 , Tokens[1])
        public Expression Evaluate()
        {
            var memberExpression = MemberExpression.Evaluate();
            ParameterExpression? parameter = null;// = (ParameterExpression)Parameter.Evaluate();

            //var methodInfo = MethodFinder.Instance.GetMethod(memberExpression.Type, MethodName, Tokens.Count);

            if (memberExpression.Type.IsCollection())
            {

                List<Expression> expressions = new List<Expression>();
                foreach (var token in Tokens)
                {
                    //var list = token as ITokenList;
                    parameter = token.SetParameterExpressionBody(MemberExpression);

                    var expression = ExpressionEvaluator.Evaluate(token.Tokens);

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
