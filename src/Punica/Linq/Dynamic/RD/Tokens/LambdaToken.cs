using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    //TODO remove if not used currently using property token instead
    public class LambdaToken : IToken
    {
        
        public bool IsLeftAssociative => true;
        public short Precedence => 0;
        public TokenType TokenType => TokenType.String;
        public ExpressionType ExpressionType => ExpressionType.Constant;

        public string Name { get; }

        public LambdaToken(string name)
        {
            Name = name;
        }



    }
}
