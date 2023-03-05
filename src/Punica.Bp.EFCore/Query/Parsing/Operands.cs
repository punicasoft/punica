using System.ComponentModel;
using System.Linq.Expressions;

namespace Punica.Bp.EFCore.Query.Parsing
{
    public readonly struct Operands
    {
        public Expression Left { get; }
        public Expression Right { get; }

        public Operands(Expression left, Expression right)
        {
            if(left.Type != right.Type)
            {
                throw new ArgumentException($"Mismatch types members in {left}, {right}");
            }

            Left = left;
            Right = right;
        }


        public static Operands Create(object left, object right, Expression arg)
        {

            if (left is Token t1 && right is Token t2)
            {
                return Create(t1, t2, arg);
            }

            if (left is Expression e1 && right is Expression e2)
            {
                return new Operands(e1, e2);
            }

            if (left is Token tt1 && right is Expression ee2)
            {
                return Create(tt1, ee2, arg);
            }

            if (left is Expression ee1 && right is Token tt2)
            {
                return Create(ee1, tt2, arg);
            }

            throw new ArgumentException($"Invalid arguments in {nameof(left)} or {nameof(right)}");
        }

        public static Operands Create(Token left, Expression right, Expression arg)
        {
            if (left.Type == TokenType.Member)
            {
                var e1 = GetProperty(left.Value, arg);
                return new Operands(e1, right);
            }
            else
            {
               var e1 = Expression.Constant(ParseString(right.Type, left.Value));
               return new Operands(e1, right);
            }
        }

        public static Operands Create(Expression left, Token right, Expression arg)
        {
            if (right.Type == TokenType.Member)
            {
                var e2 = GetProperty(right.Value, arg);
                return new Operands(left, e2);
            }
            else
            {
                var e2 = Expression.Constant(ParseString(left.Type, right.Value));
                return new Operands(left, e2);
            }
        }

        public static Operands Create(Token left, Token right, Expression arg)
        {
            if (left.Type == right.Type)
            {
                switch (left.Type)
                {
                    case TokenType.Member:
                        var e1 = GetProperty(left.Value, arg);
                        var e2 = GetProperty(right.Value, arg);
                        return new Operands(e1, e2);
                        break;
                    case TokenType.String:
                        return new Operands(Expression.Constant(left.Value),
                            Expression.Constant(right.Value));
                    case TokenType.Number:
                        return new Operands(Expression.Constant(int.Parse(left.Value)),
                            Expression.Constant(int.Parse(right.Value)));
                    case TokenType.RealNumber:
                        return new Operands(Expression.Constant(decimal.Parse(left.Value)),
                            Expression.Constant(decimal.Parse(right.Value)));
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (left.Type == TokenType.Member)
            {
                var e1 = GetProperty(left.Value, arg);
                var e2 = Expression.Constant(ParseString(e1.Type, right.Value));
                return new Operands(e1, e2);
            }
            else if (right.Type == TokenType.Member)
            {
                var e2 = GetProperty(right.Value, arg); 
                var e1 = Expression.Constant(ParseString(e2.Type, left.Value));
                return new Operands(e1, e2);
            }

            throw new ArgumentException($"Mismatch types in {left.Value}, {right.Value}");
        }

        public static object? ParseString(Type type, string input)
        {
            if (type == typeof(string))
            {
                return input;
            }

            if (type == typeof(int))
            {
                return int.Parse(input);
            }

            if (type == typeof(double))
            {
                return double.Parse(input);
            }

            if (type == typeof(bool))
            {
                return bool.Parse(input);
            }

            var converter = TypeDescriptor.GetConverter(type);
            return converter.ConvertFromInvariantString(input);
        }

        public static Expression GetProperty(string name, Expression expression)
        {

            var parts = name.Split(".");

            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                expression = Expression.PropertyOrField(expression, part);
            }

            return expression;
        }
    }
}
