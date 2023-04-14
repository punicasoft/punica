using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Punica.Reflection
{
 
    public class MethodHandler
    {
        private static readonly IMethodHandler EnumerableHandler  =new EnumerableMethodHandler();
        private static readonly IMethodHandler QueryableHandler = new QueryableMethodHandler();

        public static MethodHandler Instance { get; } = new MethodHandler();

        private MethodHandler()
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

        public bool IsFunction(string methodName, int argNo)
        {
            if (argNo == 1)
            {
                switch (methodName)
                {
                    case "All":
                    case "Any":
                    case "Average":
                    case "Count":
                    case "First":
                    case "FirstOrDefault":
                    case "GroupBy":
                    case "Last":
                    case "LastOrDefault":
                    case "LongCount":
                    case "Max":
                    case "Min":
                    case "OrderBy":
                    case "OrderByDescending":
                    case "Select":
                    case "SelectMany":
                    case "Single":
                    case "SingleOrDefault":
                    case "SkipWhile":
                    case "Sum":
                    case "TakeWhile":
                    case "ThenBy":
                    case "ThenByDescending":
                    case "Where":
                        return true;
                    case "Concat":
                    case "Contains":
                    case "DefaultIfEmpty":
                    case "Distinct":
                    case "ElementAt":
                    case "ElementAtOrDefault":
                    case "Except":
                    case "GroupJoin":
                    case "Intersect":
                    case "Join":
                    case "Reverse":
                    case "SequenceEqual":
                    case "Skip":
                    case "Take":
                    case "ToArray":
                    case "ToList":
                    case "Union":
                        return false;
                    default:
                        throw new ArgumentException($"{methodName} is do not support or does not have number {argNo} arguments");
                }
            }

            if (argNo == 2)
            {
                switch (methodName)
                {
                    case "GroupBy":
                    case "SelectMany":
                    case "GroupJoin":
                    case "Join":
                        return true;
                    default:
                        throw new ArgumentException($"{methodName} is do not support or does not have number {argNo} arguments");
                }
            }


            if (argNo == 3)
            {
                switch (methodName)
                {
                    case "GroupJoin":
                    case "Join":
                        return true;
                    default:
                        throw new ArgumentException($"{methodName} is do not support or does not have number {argNo} arguments");
                }
            }


            throw new ArgumentException($"{methodName} is do not support or does not have number {argNo} arguments");
        }
    }
}
