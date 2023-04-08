﻿using System.Linq.Expressions;
using System.Reflection.Metadata;
using Punica.Extensions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;
using Punica.Reflection;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class MethodToken : Operation, ITokenList, IExpression
    {
        private readonly int _depth;
        public string MethodName { get; }
        public IExpression MemberExpression { get; } // TODO support chaining of methods
        public IExpression? Parameter { get; }
        public List<IToken> Tokens { get; }
        public override bool IsLeftAssociative => false;
        public override short Precedence => 14;
        //public TokenType TokenType => TokenType.Member;
        public override ExpressionType ExpressionType => ExpressionType.Call;
        

        public MethodToken(string methodName, IExpression memberExpression, int depth)
        {
            _depth = depth;
            MethodName = methodName;
            MemberExpression = memberExpression;
            Tokens = new List<IToken>();

            Parameter = new ParameterToken(memberExpression, "arg" + _depth);
            //var memberExpression = MemberExpression.Evaluate();

            //if (memberExpression.Type.IsCollection(out var type))
            //{
            //    _isCollection = true;
            //    Parameter = Expression.Parameter(type, "arg" + _depth);
            //}
            //else
            //{
            //    Parameter = Expression.Parameter(memberExpression.Type, "arg" + _depth);
            //}
        }

        public void AddToken(IToken token)
        {
            Tokens.Add(token);
        }

        public override Expression Evaluate(Stack<Expression> stack)
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
                
                // call the method
                LambdaExpression e2;
                switch (MethodName) //TODO find the method better
                {
                    case "Any":
                        if (expressions.Count != 1)
                        {
                            throw new ArgumentException($"Invalid expression for Any");
                        }

                        var anyType = typeof(Func<,>).MakeGenericType(parameter.Type, typeof(bool));
                        e2 = Expression.Lambda(anyType, expressions[0], parameter);

                        return Expression.Call(EnumerableCachedMethodInfo.Any(parameter.Type), memberExpression, e2);
                    case "Contains":
                        //if (expressions.Length != 1)
                        //{
                        //    throw new ArgumentException($"Invalid expression for Contains");
                        //}

                        //var containType = typeof(Func<,>).MakeGenericType(type, typeof(bool));
                        //e2 = Expression.Lambda(containType, expressions[0], arg2);
                        //return Expression.Call(CachedMethodInfo.Enumerable_Contains(type), e1, e2);
                        return null;

                    case "Select":
                        if (expressions.Count != 1)
                        {
                            throw new ArgumentException($"Invalid expression for Select");
                        }
                        var selectType = typeof(Func<,>).MakeGenericType(parameter.Type, expressions[0].Type);
                        e2 = Expression.Lambda(selectType, expressions[0], parameter);
                        return Expression.Call(EnumerableCachedMethodInfo.Select(parameter.Type, expressions[0].Type), memberExpression, e2);

                    case "ToList":
                        return Expression.Call(CachedMethodInfo.ToList(parameter.Type), memberExpression); //TODO check this validity since there is no right side
                    default:
                        throw new ArgumentException($"Invalid method {MethodName}");
                }

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

        public Expression Evaluate()
        {
            return Evaluate(null);
        }
    }
}
