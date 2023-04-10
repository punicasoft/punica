using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punica.Reflection
{
 
    public class MethodHandlerFactory
    {
        private static readonly IMethodHandler EnumerableHandler  =new EnumerableMethodHandler();
        private static readonly IMethodHandler QueryableHandler = new QueryableMethodHandler();

        public static MethodHandlerFactory Instance { get; } = new MethodHandlerFactory();

        private MethodHandlerFactory()
        {
        }

        public IMethodHandler GetHandler(Type type)
        {
            if (typeof(IQueryable).IsAssignableFrom(type))
            {
                return QueryableHandler;
            }
            else
            {
                return EnumerableHandler;
            }
        }
    }
}
