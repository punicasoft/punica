using System.Linq.Expressions;

namespace Punica.Linq.Dynamic.RD.Rd2
{
    public class Identifier
    {
        public Expression Expression { get;  }
        public string Name { get;}

        public Identifier(string name, Expression expression)
        {
            Expression = expression;
            Name = name;
        }
    }
}
