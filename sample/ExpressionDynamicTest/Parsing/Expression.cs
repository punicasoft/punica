using System.Linq.Expressions;

namespace ExpressionDynamicTest.Parsing
{
    public class ExpressionExt
    {
        public static AliasExpression Field(Expression expression, string alias)
        {
            ArgumentNullException.ThrowIfNull(alias);

            return new AliasExpression(expression, alias);
        }
    }
}
