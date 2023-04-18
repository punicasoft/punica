
using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class GeneralOperationToken : IToken
    {
        public bool IsLeftAssociative => true;
        public short Precedence => -2;
        public TokenType TokenType { get; }
        public ExpressionType ExpressionType { get; }

        public GeneralOperationToken(TokenType tokenType)
        {
            TokenType = tokenType;
            ExpressionType = ExpressionType.Extension;
        }
    }
}
