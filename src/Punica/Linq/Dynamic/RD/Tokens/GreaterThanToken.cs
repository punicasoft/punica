﻿using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class GreaterThanToken :Operation
    {
        public override short Precedence => 9;
        public override ExpressionType ExpressionType => ExpressionType.GreaterThan;
        public override Expression Evaluate(Stack<Expression> stack)
        {
            var right = stack.Pop();
            var left = stack.Pop();

            var tuple = ConvertExpressions(left, right);
            return Expression.GreaterThan(tuple.left, tuple.right);
        }
    }
}
