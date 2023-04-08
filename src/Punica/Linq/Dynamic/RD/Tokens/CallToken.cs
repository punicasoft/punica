using System.Linq.Expressions;
using System.Reflection.Metadata;
using Punica.Extensions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    //TODO: name this call properly
    public class CallToken : IExpression
    {
        private readonly MethodToken _methodToken;
        private Expression _value;
        private bool _evaluated;

        public CallToken(MethodToken methodToken)
        {
            _methodToken = methodToken;
            _evaluated = false;
        }

        public Expression Evaluate()
        {
            if (!_evaluated)
            {
                _value = _methodToken.Evaluate(null);
                _evaluated = true;
            }

            return _value;
        }
    }

    public class PropertyToken : IExpression
    {
        private readonly IExpression _expression;
        private readonly string _propertyName;
        private Expression _value;
        private bool _evaluated;

        public PropertyToken(IExpression expression, string propertyName)
        {
            _expression = expression;
            _propertyName = propertyName;
            _evaluated = false;
        }

        public Expression Evaluate()
        {
            if (!_evaluated)
            {
                _value = Expression.PropertyOrField(_expression.Evaluate(), _propertyName);
                _evaluated = true;
            }

            return _value;
        }
    }

    public class ParameterToken : IExpression
    {
        private Expression _value;
        private bool _evaluated;
        private readonly IExpression _expression;
        private readonly string _name;

        public ParameterToken(ParameterExpression parameterExpression)
        {
            _value = parameterExpression;
            _evaluated = true;
        }

        public ParameterToken(IExpression expression, string name)
        {
            _expression = expression;
            _name = name;
            _evaluated = false;
        }

        public Expression Evaluate()
        {
            if (!_evaluated)
            {
                var memberExpression = _expression.Evaluate();

                if (memberExpression.Type.IsCollection(out var type))
                {
                    _value = Expression.Parameter(type, _name);
                }
                else
                {
                    _value = Expression.Parameter(memberExpression.Type, _name);
                }
                _evaluated = true;
            }

            return _value;
        }
    }
}
