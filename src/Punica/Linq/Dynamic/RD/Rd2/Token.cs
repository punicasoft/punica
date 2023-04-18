using Punica.Linq.Dynamic.RD.Tokens.abstractions;

namespace Punica.Linq.Dynamic.RD.Rd2
{
    public struct Token
    {
        public TokenId Id { get; set; }
        public string Text { get; set; }
        public IToken? ParsedToken { get; set; }
    }
}
