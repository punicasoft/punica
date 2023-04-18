using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Punica.Extensions;

namespace Punica.Reflection
{
 
    public class MethodHandler
    {
        private static readonly IMethodHandler EnumerableHandler  =new EnumerableMethodHandler();
        private static readonly IMethodHandler QueryableHandler = new QueryableMethodHandler();

        private static readonly Dictionary<string, List<MethodInfo>> _methods = new Dictionary<string, List<MethodInfo>>();
        public static MethodHandler Instance { get; } = new MethodHandler();

        private MethodHandler()
        {
           InitializeMethodInfo(typeof(Enumerable));
        }

        private void InitializeMethodInfo(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                var key = $"{type.FullName}.{method.Name}.{method.GetParameters().Length}";
                if (!_methods.ContainsKey(key))
                {
                    _methods.Add(key, new List<MethodInfo>(){ method });
                }
                else
                {
                    _methods[key].Add(method);
                }
            }

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


        public MethodInfo GetMethod(Type type, string methodName, int argCount)
        {
            var key = $"{nameof(Enumerable)}.{methodName}.{argCount}";
            if (type.IsCollection())
            {
                if (_methods.ContainsKey(key))
                {
                    var methodInfos = _methods[key];

                    if (methodInfos.Count == 1)
                    {
                        return methodInfos[0];
                    }
                    else
                    {
                        var methodInfo = methodInfos.FirstOrDefault(m => m.GetParameters()[0].ParameterType == type);
                        if (methodInfo != null)
                        {
                            return methodInfo;
                        }
                        else
                        {
                            throw new ArgumentException($"Method {methodName} with {argCount} arguments not found in {type.FullName}");
                        }
                    }
                }
            }

            throw new ArgumentException($"Method {methodName} with {argCount} arguments not found in {type.FullName} 2");
        }

        //public MethodInfo GetMethod(Type type, string methodName, int argNo)
        //{
        //    var key = $"{type.FullName}.{methodName}.{argNo}";
        //    if (_methods.ContainsKey(key))
        //    {
        //        return _methods[key];
        //    }
        //    var handler = GetHandler(type);
        //    if (handler.IsFunction(methodName, argNo))
        //    {
        //        var method = handler.GetFunction(methodName, argNo);
        //        _methods.Add(key, method);
        //        return method;
        //    }
        //    else
        //    {
        //        var method = handler.GetMethod(methodName, argNo);
        //        _methods.Add(key, method);
        //        return method;
        //    }
        //}



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
