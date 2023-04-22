using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Tokens
{
    public class Argument //: ITokenList
    {
        private readonly List<string> _lambdas = new List<string>();
        private ParameterToken[] _parameter;
        public IReadOnlyList<string> Lambdas => _lambdas;
        public bool IsLeftAssociative => true;
        public IExpression? Parameter { get; } = null;
        public List<IToken> Tokens { get; }
        public short Precedence => 0;
        public TokenType TokenType => TokenType.List;
        public ExpressionType ExpressionType => ExpressionType.Extension;

        public Argument()
        {
            Tokens = new List<IToken>();

        }

        public void AddToken(IToken token)
        {
            Tokens.Add(token);
        }

        public void AddParameters(ParameterToken[] paras)
        {
            _parameter = paras;
        }

        public IReadOnlyList<string> ProcessLambda()
        {
            bool openParenthesis = false;
            bool closeParenthesis = false;

            foreach (var token in Tokens)
            {
                if (token.TokenType == TokenType.OpenParen)
                {
                    if (!openParenthesis)
                    {
                        openParenthesis = true;
                        continue;
                    }

                    throw new ArgumentException("Invalid Expression");
                }

                if (token.TokenType == TokenType.CloseParen)
                {
                    if (!closeParenthesis && openParenthesis && Tokens.IndexOf(token)== Tokens.Count - 1)
                    {
                        closeParenthesis = true;
                        continue;
                    }

                    throw new ArgumentException("Invalid Expression");
                }

                if (token.TokenType == TokenType.Comma)
                {
                    continue;
                }

                if (token.TokenType == TokenType.Value)
                {
                    var propertyToken = token as PropertyToken;
                    if (propertyToken == null)
                    {
                        throw new ArgumentException("Invalid Expression");
                    }

                    _lambdas.Add(propertyToken.Name);
                    
                }
            }

            if (!openParenthesis || !closeParenthesis)
            {
                throw new ArgumentException("Invalid Expression");
            }

            return _lambdas;
        }

        public bool IsFirstOpenParenthesis()
        {
            if (Tokens.Count > 0 && Tokens[0].TokenType == TokenType.OpenParen)
            {
                return true;
            }
             
            return false;
        }


        public ParameterExpression? SetParameterExpressionBody(IExpression memberExpression)
        {
            if (_parameter.Length > 1)
            {
                throw new Exception("More than 1 arg is not handled");
            }

            if (_parameter.Length == 1)
            {
                _parameter[0].SetExpression(memberExpression);
                return (ParameterExpression)_parameter[0].Evaluate();
            }

            return null;
        }
    }
}
