using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Rd2
{
    public struct Alias
    {
        public TokenId Id { get; set; }
        public IToken Token { get; set; }
    }
}
