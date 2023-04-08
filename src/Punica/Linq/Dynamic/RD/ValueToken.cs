using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD
{
    public class ValueToken : IToken
    {
        public bool IsLeftAssociative => false;

        public short Precedence => 0;
        public Expression Value { get; }
        public TokenType TokenType { get; }
        public ExpressionType ExpressionType => Value.NodeType;

        public ValueToken(Expression value)
        {
            Value = value;
            TokenType = TokenType.Value;
        }

    }
}
