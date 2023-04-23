using Punica.Linq.Dynamic.RD.Tokens.abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Punica.Extensions;

namespace Punica.Linq.Dynamic.RD.Rd2
{
    //public class ParameterToken3 : IExpressionToken
    //{
    //    private Expression? _value;
    //    private bool _evaluated;
    //    private IExpression? _expression;
    //    private readonly string? _name;

    //    /// <summary>
    //    /// The Name of the parameter or variable.
    //    /// </summary>
    //    public string? Name => _name;

    //    public ParameterToken3(ParameterExpression parameterExpression)
    //    {
    //        _value = parameterExpression;
    //        _name = parameterExpression.Name;
    //        _evaluated = true;

    //    }

    //    public ParameterToken3(string name)
    //    {
    //        _name = name;
    //        _evaluated = false;
    //    }

    //    public ParameterToken3(IExpression expression, string name)
    //    {
    //        _expression = expression;
    //        _name = name;
    //        _evaluated = false;
    //    }

    //    internal void SetExpression(IExpression expression)
    //    {
    //        _expression = expression; //TODO handle invalid scenarios
    //    }

    //    public Expression Evaluate()
    //    {
    //        if (!_evaluated)
    //        {
    //            var memberExpression = _expression!.Evaluate();

    //            if (memberExpression.Type.IsCollection(out var type))
    //            {
    //                _value = Expression.Parameter(type, _name);
    //            }
    //            else
    //            {
    //                _value = Expression.Parameter(memberExpression.Type, _name);
    //            }
    //            _evaluated = true;
    //        }

    //        return _value;
    //    }

    //    //TODO remove?
    //    public bool IsLeftAssociative => true;
    //    public short Precedence => 0;
    //    public TokenType TokenType => TokenType.Value;
    //    public ExpressionType ExpressionType => ExpressionType.Parameter;
    //}
}
