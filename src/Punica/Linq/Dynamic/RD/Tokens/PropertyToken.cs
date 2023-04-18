﻿using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens;

public class PropertyToken : IExpressionToken
{
    private readonly IExpression? _expression;
    private readonly string _propertyName;
    private Expression _value;
    private bool _evaluated;

    public string Name => _propertyName;

    public PropertyToken(IExpression? expression, string propertyName)
    {
        _expression = expression;
        _propertyName = propertyName;
        _evaluated = false;
    }

    public Expression Evaluate()
    {
        if (!_evaluated)
        {
            if (_expression == null)
            {
                throw new ArgumentException("expression can not be null");
            }

            _value = Expression.PropertyOrField(_expression.Evaluate(), _propertyName);
            _evaluated = true;
        }

        return _value;
    }

    //TODO remove?
    public bool IsLeftAssociative => true;
    public short Precedence => 0;
    public TokenType TokenType => TokenType.Value;
    public ExpressionType ExpressionType => ExpressionType.MemberAccess;
}