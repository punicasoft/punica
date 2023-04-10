using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Punica.Reflection
{
    public interface IMethodHandler
    {
        MethodCallExpression CallMethod(string methodName, Expression member, ParameterExpression parameter, Expression[] expressions);
    }
}
