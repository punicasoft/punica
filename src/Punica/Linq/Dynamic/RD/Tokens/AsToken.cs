﻿using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;
using Punica.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class AsToken : Operation
    {
        public override short Precedence => 9;
        public override ExpressionType ExpressionType => ExpressionType.TypeAs;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();

            return new AliasExpression(left, (ConstantExpression)right);
        }
    }
}
