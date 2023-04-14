using System.Linq.Expressions;
using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD
{
    public interface ITokenList: IToken
    {
        //public IExpression? Parameter { get;}
        public List<IToken> Tokens { get; }

        void AddToken(IToken token);
    }
}
