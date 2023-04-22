using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionDynamicTest
{
    //public class ArgumentInfo1
    //{
    //    private MethodInfo _methodInfo;

    //    private ArgumentInfo1(MethodInfo methodInfo)
    //    {
    //        _methodInfo = methodInfo;
    //    }

    //    public List<Type> GetListTypeForInputParameters(int argumentPosition, List<Type> args)
    //    {
    //        var parameters = _methodInfo.GetParameters();
    //        if (argumentPosition < 0 || argumentPosition >= parameters.Length)
    //            return null;

    //        var parameterType = parameters[argumentPosition].ParameterType;
    //        if (!parameterType.IsGenericType || !parameterType.Name.StartsWith("Func"))
    //            return null;

    //        var typeArguments = parameterType.GetGenericArguments();
    //        var inputTypes = new List<Type>();

    //        var genericArgsMap = BuildGenericArgumentsMap(args);

    //        for (int i = 0; i < typeArguments.Length - 1; i++)
    //        {
    //            if (typeArguments[i].IsGenericParameter)
    //            {
    //                if (genericArgsMap.TryGetValue(typeArguments[i], out Type mappedType))
    //                {
    //                    inputTypes.Add(mappedType);
    //                }
    //            }
    //            else
    //            {
    //                inputTypes.Add(typeArguments[i]);
    //            }
    //        }

    //        return inputTypes;
    //    }

    //    private Dictionary<Type, Type> BuildGenericArgumentsMap(List<Type> args)
    //    {
    //        var genericArgsMap = new Dictionary<Type, Type>();
    //        var genericParameters = _methodInfo.GetGenericArguments();
    //        var nonFuncArgs = new List<Type>();

    //        foreach (var parameter in _methodInfo.GetParameters())
    //        {
    //            var parameterType = parameter.ParameterType;
    //            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() != typeof(Func<,,>))
    //            {
    //                nonFuncArgs.Add(parameterType);
    //            }
    //        }

    //        for (int i = 0; i < nonFuncArgs.Count; i++)
    //        {
    //            var genericArguments = nonFuncArgs[i].GetGenericArguments();
    //            var providedArguments = args[i].GetGenericArguments();

    //            for (int j = 0; j < genericArguments.Length; j++)
    //            {
    //                genericArgsMap[genericArguments[j]] = providedArguments[j];
    //            }
    //        }

    //        return genericArgsMap;
    //    }

    //    public static ArgumentInfo1 CreateFromMethodInfo(MethodInfo methodInfo)
    //    {
    //        return new ArgumentInfo1(methodInfo);
    //    }
    //}

    public class ArgumentInfo1
    {
        private MethodInfo _methodInfo;
        private Dictionary<Type, Type> _genericArgsMap;

        private ArgumentInfo1(MethodInfo methodInfo, List<Type> args)
        {
            _methodInfo = methodInfo;
            _genericArgsMap = BuildGenericArgumentsMap(args);
        }

        public List<Type> GetListTypeForInputParameters(int argumentPosition)
        {
            var parameters = _methodInfo.GetParameters();
            if (argumentPosition < 0 || argumentPosition >= parameters.Length)
                return null;

            var parameterType = parameters[argumentPosition].ParameterType;
            if (!parameterType.IsGenericType || !parameterType.Name.StartsWith("Func"))
                return null;

            var typeArguments = parameterType.GetGenericArguments();
            var inputTypes = new List<Type>();

            for (int i = 0; i < typeArguments.Length - 1; i++)
            {
                if (typeArguments[i].IsGenericParameter)
                {
                    if (_genericArgsMap.TryGetValue(typeArguments[i], out Type mappedType))
                    {
                        inputTypes.Add(mappedType);
                    }
                }
                else
                {
                    inputTypes.Add(typeArguments[i]);
                }
            }

            return inputTypes;
        }

        private Dictionary<Type, Type> BuildGenericArgumentsMap(List<Type> args)
        {
            var genericArgsMap = new Dictionary<Type, Type>();
            var genericParameters = _methodInfo.GetGenericArguments();
            var nonFuncArgs = _methodInfo.GetParameters()
                .Where(p => !p.ParameterType.IsGenericType || !p.ParameterType.Name.StartsWith("Func")).ToList();
            var nonFuncArgsIndex = 0;

            for (int i = 0; i < genericParameters.Length; i++)
            {
                if (!genericArgsMap.ContainsKey(genericParameters[i]))
                {
                    var providedArguments = args[nonFuncArgsIndex].GetGenericArguments();
                    genericArgsMap[genericParameters[i]] = providedArguments[0];
                    nonFuncArgsIndex++;
                }
            }

            return genericArgsMap;
        }

        public static ArgumentInfo1 CreateFromMethodInfo(MethodInfo methodInfo, List<Type> args)
        {
            return new ArgumentInfo1(methodInfo, args);
        }
    }

    public static class ArgData
    {
        public static void GetArgData(MethodInfo methodInfo, List<Type> args)
        {
            var map = new Dictionary<string, string>();
            var nonFuncArgs = methodInfo.GetParameters();

            var inputs = new Dictionary<string, string>();

            foreach (var arg in nonFuncArgs)
            {
                Console.WriteLine($"Generic:{arg.ParameterType.IsGenericType}, Name:{arg.ParameterType.Name} , {arg}");
            }

            var genericParameters = methodInfo.GetGenericArguments();
            foreach (var parameter in genericParameters)
            {
                Console.WriteLine($"Generic:{parameter.IsGenericType}, Name:{parameter.Name} , {parameter}");
                map[parameter.Name] = "";
            }

            for (var index = 0; index < nonFuncArgs.Length; index++)
            {
                var arg = nonFuncArgs[index];
                if (arg.ParameterType.IsGenericType)
                {
                    if (arg.ParameterType.Name.StartsWith("Func"))
                    {
                        var typeArguments = arg.ParameterType.GetGenericArguments();
                        var last = typeArguments.Last();
                        if (map.ContainsKey(last.Name) && map[last.Name] == "")
                        {
                            map[last.Name] = $"Func_[{index}].Type";// $"typeof(args[{index}])";
                        }

                        inputs[index.ToString()] = "";

                        //set input types
                        for (var i = 0; i < typeArguments.Length - 1; i++)
                        {
                            if (map.ContainsKey(typeArguments[i].Name))
                            {

                                inputs[index.ToString()] += map[typeArguments[i].Name] + ",";
                            }
                            else
                            {
                                var genericArguments = typeArguments[i].GetGenericArguments();

                                foreach (var genericArgument in genericArguments)
                                {
                                    if (map.ContainsKey(genericArgument.Name))
                                    {
                                        inputs[index.ToString()] += $"{typeArguments[i].Name}<{map[genericArgument.Name]}>,";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (map.ContainsKey(arg.ParameterType.Name))
                        {
                            map[arg.ParameterType.Name] = args[index].Name;// $"typeof(args[{index}])";
                        }
                        else
                        {
                            var genericArguments = arg.ParameterType.GetGenericArguments();

                            foreach (var argument in genericArguments)
                            {
                                if (map.ContainsKey(argument.Name))
                                {
                                    // args[index].GetGenericParameterConstraints();  
                                    // var constraints = args[index].GetGenericParameterConstraints();
                                    var temp = args[index].GetGenericArguments()[0];
                                    map[argument.Name] = args[index].GetGenericArguments()[0].Name;

                                }
                            }
                        }


                    }
                }
            }

            foreach (var keyValuePair in map)
            {
                Console.WriteLine($"{keyValuePair.Key} = {keyValuePair.Value}");
            }

            Console.WriteLine();
            Console.WriteLine(" Inputs:");

            foreach (var keyValuePair in inputs)
            {
                Console.WriteLine($"Input[{keyValuePair.Key}] = {keyValuePair.Value}");
            }

        }
    }
}
