using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD
{
    public interface ITokenList: IToken
    {
        public ParameterExpression? Parameter { get;}
        public List<IToken> Tokens { get; }

        void AddToken(IToken token);
    }
}
