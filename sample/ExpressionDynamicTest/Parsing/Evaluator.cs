using ExpressionDynamicTest.Parsing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Punica.Dynamic;

namespace ExpressionDynamicTest.Parsing
{
    public class Evaluator : IEvaluator
    {
       
        private readonly Expression _parameterInstance;

        // private ParaObject _paras;
        private readonly ParameterExpression _arg;
        


        public Evaluator(Type type, Expression parameterInstance)
        {
           
            _parameterInstance = parameterInstance;
            // _paras = new ParaObject();

            if (type == null)
            {
                _arg = null;
                return;
            }
            _arg = Expression.Parameter(type, "arg");
        }

        public Evaluator(ParameterExpression arg, Expression parameterInstance)
        {

            _parameterInstance = parameterInstance;
            _arg = arg;
        }

        public Expression<Func<bool>> GetFilterExpression(Expression exp)
        {
            return Expression.Lambda<Func<bool>>(exp);
        }

        public Expression GetCompiledExpression(Expression exp)
        {

            return Expression.Lambda<Func<bool>>(exp);
        }

        public Expression<Func<T>> GetFilterExpression<T>(Expression exp)
        {
            return Expression.Lambda<Func<T>>(exp);
        }


        public Expression<Func<TResult, T>> GetFilterExpression<TResult, T>(Expression exp)
        {
            return Expression.Lambda<Func<TResult, T>>(exp, _arg);
        }

        public Expression Add(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);

            if (operands.Left.Type == typeof(string))
            {
                //var concatMethod = typeof(string).GetMethod(nameof(string.Concat), new Type[] { typeof(string), typeof(string) });
                return Expression.Call(CachedMethodInfo.ConcatMethod, operands.Left, operands.Right);
            }

            return Expression.Add(operands.Left, operands.Right);
        }

        public Expression Subtract(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.Subtract(operands.Left, operands.Right);
        }

        public Expression Multiply(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.Multiply(operands.Left, operands.Right);
        }

        public Expression Divide(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.Divide(operands.Left, operands.Right);
        }

        public Expression Modulo(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.Modulo(operands.Left, operands.Right);
        }

        public Expression And(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.AndAlso(operands.Left, operands.Right);
        }

        public Expression Not(object operand)
        {
            if (operand is Expression e1)
            {
                return Expression.Not(e1);
            }

            if (operand is Token t1 && t1.Type == TokenType.Boolean)
            {
                return Expression.Not(Expression.Constant(bool.Parse(t1.Value)));
            }

            throw new ArgumentException("invalid operands");
        }

        public Expression Equal(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.Equal(operands.Left, operands.Right);
        }

        public Expression NotEqual(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.NotEqual(operands.Left, operands.Right);
        }

        public Expression GreaterThan(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.GreaterThan(operands.Left, operands.Right);
        }

        public Expression LessThan(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.LessThan(operands.Left, operands.Right);
        }

        public Expression GreaterOrEqual(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.GreaterThanOrEqual(operands.Left, operands.Right);
        }

        public Expression LessOrEqual(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.LessThanOrEqual(operands.Left, operands.Right);
        }

        public Expression Single(Token token)
        {
            if (token.Type == TokenType.Member)
            {
                var e2 = Operands.GetProperty(token.Value, _arg);
                return e2;
            }

            var val = Operands.ParseString(typeof(bool), token.Value);
            return Expression.Constant(val);
        }

        public Expression Or(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.OrElse(operands.Left, operands.Right);
        }

        public Expression Condition(object condition, object left, object right)
        {
            Expression ex = null;

            if (condition is Expression e1)
            {
                ex = e1;
            }
            else if (condition is Token t1) 
            {
                ex = Single(t1);
            }
            else
            {
                throw new ArgumentException("invalid condition");
            }

            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.Condition(ex, operands.Left, operands.Right);
        }

        public Expression Coalesce(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);
            return Expression.Coalesce(operands.Left, operands.Right);
        }

        public Expression Contains(object left, object right)
        {
            var operands = Operands.Create(left, right, _arg, _parameterInstance);

            if (operands.Right.Type == typeof(string))
            {
                return Expression.Call(operands.Right, CachedMethodInfo.StringContains, operands.Left);
            }
            else
            {
               return Expression.Call(CachedMethodInfo.EnumerableContainsMethod(operands.Left.Type), operands.Right, operands.Left);
            }

            return null;
        }

        //TODO refactor since this only support very specific situation and others are not handled
        public Expression Any(object left, object right)
        {
            if (left is Token t1 && t1.Type == TokenType.Member)
            {
                var e1 = Operands.GetProperty(t1.Value, _arg);
                var type = Operands.GetImplementedType(e1.Type);
                ParameterExpression arg2 = Expression.Parameter(type, "arg2");

                Evaluator evaluator = new Evaluator(arg2, _parameterInstance);

                var t2 = (Token)right;

                var body = TextParser.Evaluate(t2.Value, evaluator);

              
                var funcType = typeof(Func<,>).MakeGenericType(type, typeof(bool));
                var e2 = Expression.Lambda(funcType, body[0], arg2);

                return Expression.Call(CachedMethodInfo.AnyMethod(type), e1, e2);

            }

            throw new NotImplementedException();
        }

        public Expression New(object right)
        {
            if (right is Token t2)
            {
                Evaluator evaluator = new Evaluator(_arg, _parameterInstance);
                var body = TextParser.Evaluate(t2.Value, evaluator);

               return ParseNew(body);

            }

            throw new ArgumentException("invalid operands");
        }

        public Expression Dot(object left, object right)
        {
            if (left is Token t1 && right is Token t2)
            {
                var e1 = Expression.PropertyOrField(_arg, t1.Value);
                return Expression.PropertyOrField(e1, t2.Value);
            }


            //if (left is Expression e11 && right is Token t22)
            //{
            //   return Expression.PropertyOrField(e1, t2.Value);
            //}

            return null;
        }

        public Expression Call(object left, object method, object right)
        {
            if (left is Token t1 && right is Token t2)
            {
                var e1 = Expression.PropertyOrField(_arg, t1.Value);

                if (Operands.IsCollectionOrList(e1.Type))
                {
                    var type = Operands.GetImplementedType(e1.Type);

                    ParameterExpression arg2 = Expression.Parameter(type, "arg2");
                    Evaluator evaluator = new Evaluator(arg2, _parameterInstance);

                    var body = TextParser.Evaluate(t2.Value, evaluator);
                    // call the method
                }
                else
                {
                    ParameterExpression arg2 = Expression.Parameter(e1.Type, "arg2");
                    Evaluator evaluator = new Evaluator(arg2, _parameterInstance);

                    var body = TextParser.Evaluate(t2.Value, evaluator);

                    //TODO find the method find the expression
                    // return Expression.Call(CachedMethodInfo.EnumerableContainsMethod(e1.Type), right);
                }
            }

            //return Expression.Call(CachedMethodInfo.EnumerableContainsMethod(operands.Left.Type), operands.Right, operands.Left);

            //if (left is Token t1 && t1.Type == TokenType.Member)
            //{
            //    var e1 = Operands.GetProperty(t1.Value, _arg);
            //    var type = Operands.GetImplementedType(e1.Type);
            //    ParameterExpression arg2 = Expression.Parameter(type, "arg2");

            //    Evaluator evaluator = new Evaluator(arg2, _parameterInstance);

            //    var t2 = (Token)right;

            //    var body = TextParser.Evaluate(t2.Value, evaluator);


            //    var funcType = typeof(Func<,>).MakeGenericType(type, typeof(bool));
            //    var e2 = Expression.Lambda(funcType, body[0], arg2);

            //    return Expression.Call(CachedMethodInfo.AnyMethod(type), e1, e2);

            //}


            return null;
        }

        public Expression As(object left, object right)
        {
            if (left is Expression e1 && right is Token t2)
            {
                return new AliasExpression(e1, t2.Value);
            }

            throw new ArgumentException("invalid operands");
        }


        private Expression ParseNew(Expression[] expressions)
        {
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
                bindings.Add(Expression.Bind(member, bindkeys[member.Name] ));
            }

            return Expression.MemberInit(Expression.New(type), bindings);
        }

        private string GetName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberExpression =(MemberExpression)expression;
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
