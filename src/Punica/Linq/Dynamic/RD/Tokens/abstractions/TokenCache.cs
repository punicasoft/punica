using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD.Tokens.abstractions
{
    public static class TokenCache
    {
        public static IToken Add = new AddToken();
        public static IToken Subtract = new SubtractToken();
        public static IToken Multiply = new MultiplyToken();
        public static IToken Divide = new DivideToken();
        public static IToken Modulo = new ModuloToken();
        public static IToken Equal = new EqualToken();
        public static IToken NotEqual = new NotEqualToken();
        public static IToken GreaterThan = new GreaterThanToken();
        public static IToken GreaterThanOrEqual = new GreaterThanOrEqualToken();
        public static IToken LessThan = new LessThanToken();
        public static IToken LessThanOrEqual = new LessThanOrEqualToken();
        public static IToken AndAlso = new AndToken();
        public static IToken OrElse = new OrToken();
        public static IToken Not = new NotToken();
        public static IToken LeftParenthesis = new GeneralOperationToken(TokenType.OpenParen);//new LeftParenthesisToken();
        public static IToken RightParenthesis = new GeneralOperationToken(TokenType.CloseParen);//new RightParenthesisToken();
        public static IToken Comma = new AddToken();//new CommaToken();
        public static IToken Dot = new AddToken();//new DotToken();
        public static IToken QuestionMark = new AddToken();//new QuestionMarkToken();
        public static IToken Colon = new ColonToken();
        public static IToken True = new ValueToken(Expression.Constant(true));//new TrueToken();
        public static IToken False = new ValueToken(Expression.Constant(false));//new FalseToken();
        public static IToken Null = new AddToken();//new NullToken();
        public static IToken Identifier = new AddToken();//new IdentifierToken();
        public static IToken BitwiseAnd = new BitwiseAndToken();
        public static IToken BitwiseOr = new BitwiseOrToken();
        public static IToken BitwiseXor = new BitwiseXorToken();
        public static IToken BitwiseNot = new BitwiseNotToken();
        public static IToken LeftShift = new AddToken();//new LeftShiftToken();
        public static IToken RightShift = new AddToken();//new RightShiftToken();
        public static IToken Assign = new AddToken();//new StringToken();
        public static IToken NullCoalescing = new NullCoalescingToken();
        public static IToken Conditional = new ConditionalToken();
        public static IToken In = new InToken();
        public static IToken As = new AsToken();
    }

  
}
