﻿using System.Data;
using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class ColonToken : Operation
    {
        public override bool IsLeftAssociative => false;
        public override short Precedence => 1;
        public override ExpressionType ExpressionType => ExpressionType.Default;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            throw new InvalidExpressionException("Colon is not valid expression");
        }
    }
}
