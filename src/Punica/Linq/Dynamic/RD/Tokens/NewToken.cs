﻿using System.Linq.Expressions;
using Punica.Dynamic;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;
using Punica.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class NewToken : Operation, ITokenList
    {
       // public Expression MemberExpression { get; }
        public ParameterExpression? Parameter { get; }
        public List<IToken> Tokens { get; }
        public override short Precedence => 14;
        public override ExpressionType ExpressionType => ExpressionType.New;

        public override Expression Evaluate(Stack<Expression> stack)
        {
            List<Expression> expressions = new List<Expression>();
            foreach (var token in Tokens)
            {
                var list = token as ITokenList;

                var expression = Process(list.Tokens);

                expressions.Add(expression);
            }

            var properties = new List<AnonymousProperty>();
            var bindkeys = new Dictionary<string, Expression>();

            foreach (var expression in expressions)
            {
                var name = GetName(expression);
                bindkeys[name] = expression;
                properties.Add(new AnonymousProperty(name, expression.Type));
            }

            var type = AnonymousTypeFactory.CreateType(properties);

            var bindings = new List<MemberBinding>();
            var members = type.GetProperties();

            foreach (var member in members)
            {
                bindings.Add(Expression.Bind(member, bindkeys[member.Name]));
            }

            return Expression.MemberInit(Expression.New(type), bindings);

        }

        public NewToken(ParameterExpression? parameter)
        {
            Tokens = new List<IToken>();
            Parameter = parameter;
        }

        public void AddToken(IToken token)
        {
            Tokens.Add(token);
        }

        private string GetName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression)expression;
                    return GetName(memberExpression.Expression) + memberExpression.Member.Name;
                    break;
                case ExpressionType.Parameter:
                    return "";
                    break;
                case ExpressionType.Extension:
                    if (expression is AliasExpression e)
                    {
                        return e.Alias;
                    }
                    throw new ArgumentException("Invalid Expression");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}
