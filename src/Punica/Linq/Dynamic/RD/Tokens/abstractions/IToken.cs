using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD.Tokens.abstractions
{
    public interface IToken
    {
        public bool IsLeftAssociative { get; }
        public short Precedence { get; }
        public TokenType TokenType { get; }

        public ExpressionType ExpressionType { get; }
    }
}
