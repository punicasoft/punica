﻿
using System.Linq.Expressions;

namespace Punica.Bp.EFCore.Query.Parsing
{
    public interface IEvaluator
    {
        Expression<Func<TResult, bool>> GetFilterExpression<TResult>(Expression exp);
        Expression Add(object left, object right);
        Expression Subtract(object left, object right);
        Expression Multiply(object left, object right);
        Expression Divide(object left, object right);
        Expression Modulo(object left, object right);
        Expression And(object left, object right);
        Expression Or(object left, object right);
        Expression Not(object operand);
        Expression Equal(object left, object right);
        Expression NotEqual(object left, object right);
        Expression GreaterThan(object left, object right);
        Expression LessThan(object left, object right);
        Expression GreaterOrEqual(object left, object right);
        Expression LessOrEqual(object left, object right);
        Expression Single(Token token);
    }
}
