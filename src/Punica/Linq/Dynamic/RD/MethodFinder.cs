using System;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata;
using Punica.Extensions;
using Punica.Reflection;

namespace Punica.Linq.Dynamic.RD
{
    public class MethodFinder
    {
        private static readonly Dictionary<string, List<MethodInfo>> _methods = new Dictionary<string, List<MethodInfo>>();

        public static MethodFinder Instance { get; } = new MethodFinder();

        private MethodFinder()
        {
            InitializeMethodInfo(typeof(Enumerable));
        }


        public void InitializeMethodInfo(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                var key = $"{type.FullName}.{method.Name}.{method.GetParameters().Length}";
                if (!_methods.ContainsKey(key))
                {
                    _methods.Add(key, new List<MethodInfo>() { method });
                }
                else
                {
                    _methods[key].Add(method);
                }
            }

        }

        public void Print()
        {
            foreach (var key in _methods.Keys)
            {
                Console.WriteLine(key +" "+ _methods[key].Count);
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
                        //TODO : support overloads
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

        public SignatureResolver GetArgData(MethodInfo methodInfo)
        {
            var map = new Dictionary<string, Func<Expression[], Type>>();
            var parameters = methodInfo.GetParameters();

            var inputs = new Dictionary<int, Func<Expression[], ParameterExpression[], Expression>>();
            var lambdas = new Dictionary<int, Func<Expression[], Type[]>>();

            var genericParameters = methodInfo.GetGenericArguments();
            var genericParametersNames = genericParameters.Select(g => g.Name).ToList();

            //foreach (var parameter in genericParameters)
            //{
            //    Console.WriteLine($"Generic:{parameter.IsGenericType}, Name:{parameter.Name} , {parameter}");
            //    //map[parameter.Name] = null;
            //}

            for (var index = 0; index < parameters.Length; index++)
            {
                var arg = parameters[index];
                if (arg.ParameterType.IsGenericType)
                {
                    if (arg.ParameterType.Name.StartsWith("Func"))
                    {
                        var typeArguments = arg.ParameterType.GetGenericArguments();
                        var last = typeArguments.Last();

                        if (genericParametersNames.Contains(last.Name) && !map.ContainsKey(last.Name))
                        {
                            var index1 = index;
                            map[last.Name] = args => args[index1].Type;  // $"Func_[{index}].Type";
                        }

                        var parTypes = new List<Func<Expression[], Type>>();

                        //set input types and skip last output types
                        for (var i = 0; i < typeArguments.Length - 1; i++)
                        {
                            if (map.ContainsKey(typeArguments[i].Name))
                            {
                                parTypes.Add(map[typeArguments[i].Name]);

                            }
                            else
                            {
                                var genericArguments = typeArguments[i].GetGenericArguments();

                                foreach (var genericArgument in genericArguments)
                                {
                                    if (map.ContainsKey(genericArgument.Name))
                                    {
                                        var i1 = i;
                                        parTypes.Add(args => typeArguments[i1].GetGenericTypeDefinition().MakeGenericType(map[genericArgument.Name](args)));
                                    }
                                }
                            }
                        }

                        lambdas[index] =  args =>
                        {
                            var outputArray = new Type[parTypes.Count];

                            for (var i = 0; i < parTypes.Count; i++)
                            {
                                outputArray[i] = parTypes[i](args);
                            }

                            return outputArray;
                        };

                        var index2 = index;
                        inputs[index] = (args,paras) => Expression.Lambda(arg.ParameterType.MakeGenericType(lambdas[index2](args)), args[index2], paras);
                    }
                    else
                    {
                        if (genericParametersNames.Contains(arg.ParameterType.Name) && !map.ContainsKey(arg.ParameterType.Name))
                        {
                            var index1 = index;
                            map[arg.ParameterType.Name] = args => args[index1].Type;  
                        }
                        else
                        {
                            var genericArguments = arg.ParameterType.GetGenericArguments();

                            for (var i = 0; i < genericArguments.Length; i++)
                            {
                                var argument = genericArguments[i];
                                if (genericParametersNames.Contains(argument.Name) && !map.ContainsKey(argument.Name))
                                {
                                    var index1 = index;
                                    var i1 = i;
                                    map[argument.Name] =
                                        args => args[index1].Type.GetGenericArguments()[i1]; 
                                }
                            }
                        }

                        var index2 = index;
                        inputs[index] = (args, paras) => args[index2];
                    }
                }
            }

            return new SignatureResolver(methodInfo, map.Values.ToArray(), lambdas.Values.ToArray(), inputs.Values.ToArray());
        }
        
    }

    public class SignatureResolver
    {
        private readonly MethodInfo _methodInfo;
        private readonly Func<Expression[], Type>[] _genericArgumentsResolvers;
        private readonly Func<Expression[], Type[]>[] _lambdasInputTypesResolvers;
        private readonly Func<Expression[], ParameterExpression[], Expression>[] _methodArgumentResolvers;

        public int FuncCount => _lambdasInputTypesResolvers.Length;
        public Type[] LambdasTypes(Expression[] args, int index)
        {
            return _lambdasInputTypesResolvers[index](args);
        }

        public SignatureResolver(MethodInfo methodInfo, 
            Func<Expression[], Type>[] genericArgumentsResolvers,
            Func<Expression[], Type[]>[] lambdasInputTypesResolvers, 
            Func<Expression[], ParameterExpression[], Expression>[] methodArgumentResolvers)
        {
            _methodInfo = methodInfo;
            _genericArgumentsResolvers = genericArgumentsResolvers;
            _lambdasInputTypesResolvers = lambdasInputTypesResolvers;
            _methodArgumentResolvers = methodArgumentResolvers;
        }

        public MethodCallExpression Resolve(Expression[] args, ParameterExpression[] paras)
        {
            var methodInfo = _methodInfo.MakeGenericMethod(GetGenericTypeArguments(args));
            return Expression.Call(methodInfo, args);
        }

        public Type[] GetGenericTypeArguments(Expression[] args)
        {
            Type[] outputArray = new Type[_genericArgumentsResolvers.Length];

            for (var i = 0; i < _genericArgumentsResolvers.Length; i++)
            {
                var func = _genericArgumentsResolvers[i];
                outputArray[i] = func(args);
            }

            return outputArray;
        }

        public Expression[] GetArguments(Expression[] args, ParameterExpression[] paras)
        {
            Expression[] outputArray = new Expression[_methodArgumentResolvers.Length];

            for (var i = 0; i < _methodArgumentResolvers.Length; i++)
            {
                var func = _methodArgumentResolvers[i];
                outputArray[i] = func(args, paras);
            }

            return outputArray;
        }
    }
}
