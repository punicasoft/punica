﻿using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class RootToken : ITokenList, IExpression
    {
        //public IExpression? Parameter { get; set; }
        private readonly ParameterToken[] _parameters;
        public List<IToken> Tokens { get; }

        public bool IsLeftAssociative => true;
        public short Precedence => 0;
        public TokenType TokenType => TokenType.Operator;
        public ExpressionType ExpressionType => ExpressionType.Lambda;


        public RootToken(List<ParameterToken> argExpression, List<IToken> tokens)
        {
            _parameters = argExpression.ToArray();
            Tokens = tokens;
        }

        public Expression Evaluate()
        {
            var body = ExpressionEvaluator.Evaluate(Tokens);

            if (_parameters.Length == 0)
            {
                return Expression.Lambda(body);
            }

            //TODO try and see instead of using as lambda expression use as method call? but both need to be supported
            //new { Account.Name , Account.Balance } is shorten of p => new { p.Account.Name, p.Account.Balance }
            // Select(new { Account.Name , Account.Balance }) is persons.Select(p => new { p.Account.Name, p.Account.Balance })
            // but it currently evaluate as  persons => persons.Select(p => new { p.Account.Name, p.Account.Balance })
            return Expression.Lambda(body, GetParameters());
        }


        public void AddToken(IToken token)
        {
            throw new NotImplementedException();
        }

        private List<ParameterExpression> GetParameters()
        {
            var parameters = new List<ParameterExpression>();

            foreach (var parameter in _parameters)
            {
                parameters.Add((ParameterExpression)parameter.Evaluate());
            }
            return parameters;
        }
    }
}
