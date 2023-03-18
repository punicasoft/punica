﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Punica.Bp.EFCore.Query.Parsing
{
    public class Evaluator : IEvaluator
    {
        private readonly Type _type;
        private ParaObject _paras;
        private readonly ParameterExpression _arg;

        private static readonly MethodInfo _concatMethod =
            typeof(string).GetMethod(nameof(string.Concat), new Type[] { typeof(string), typeof(string) });


        public Evaluator(Type type)
        {
            _type = type;
            _paras = new ParaObject();
            _arg = Expression.Parameter(type, "arg");
        }


        public Expression<Func<TResult, bool>> GetFilterExpression<TResult>(Expression exp)
        {
            return Expression.Lambda<Func<TResult, bool>>(exp, _arg);
        }

        public Expression Add(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);

            if (operands.Left.Type == typeof(string))
            {
                return Expression.Call(_concatMethod, operands.Left, operands.Right);
            }

            return Expression.Add(operands.Left, operands.Right);
        }

        public Expression Subtract(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.Subtract(operands.Left, operands.Right);
        }

        public Expression Multiply(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.Multiply(operands.Left, operands.Right);
        }

        public Expression Divide(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.Divide(operands.Left, operands.Right);
        }

        public Expression Modulo(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.Modulo(operands.Left, operands.Right);
        }

        public Expression And(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.AndAlso(operands.Left, operands.Right);
        }

        public Expression Not(object operand)
        {
            if (operand is Expression e1)
            {
                return Expression.Not(e1);
            }

            throw new ArgumentException("invalid operands");
        }

        public Expression Equal(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.Equal(operands.Left, operands.Right);
        }

        public Expression NotEqual(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.NotEqual(operands.Left, operands.Right);
        }

        public Expression GreaterThan(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.GreaterThan(operands.Left, operands.Right);
        }

        public Expression LessThan(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.LessThan(operands.Left, operands.Right);
        }

        public Expression GreaterOrEqual(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.GreaterThanOrEqual(operands.Left, operands.Right);
        }

        public Expression LessOrEqual(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.LessThanOrEqual(operands.Left, operands.Right);
        }

        public Expression Single(Token token)
        {
            if (token.Type == TokenType.Member)
            {
                var e2 = Operands.GetProperty(token.Value, _arg);
            }

            Operands.ParseString(typeof(bool), token.Value, ref _paras);
            return Expression.Constant(_paras.para1);
        }

        public Expression Or(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _paras);
            return Expression.OrElse(operands.Left, operands.Right);
        }

    }
}