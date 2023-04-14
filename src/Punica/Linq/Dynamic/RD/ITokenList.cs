namespace Punica.Linq.Dynamic.RD
{
    public interface ITokenList: IToken
    {
        //public IExpression? Parameter { get;}
        public List<IToken> Tokens { get; }

        void AddToken(IToken token);
    }
}
