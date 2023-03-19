using System.Reflection;

namespace Punica.Reflection
{
    public static class CachedMethodInfo
    {
        private static MethodInfo? _contains;

        public static MethodInfo Contains => _contains ??= typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) })!;

        private static MethodInfo? _concat;

        public static MethodInfo Concat => _concat ??= new Func<string?, string?, string>(string.Concat).GetMethodInfo();

        private static MethodInfo? _enumerableContains;

        public static MethodInfo Enumerable_Contains(Type type) => (_enumerableContains ??= new Func<IEnumerable<object>, object, bool>(Enumerable.Contains).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _any;

        public static MethodInfo Any(Type type) => (_any ??= new Func<IEnumerable<object>, Func<object, bool>, bool>(Enumerable.Any).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _select;

        public static MethodInfo Select(Type source, Type result) => (_select ??= new Func<IEnumerable<object>, Func<object, object>, IEnumerable<object>>(Enumerable.Select).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(source, result);

        private static MethodInfo? _toList;

        public static MethodInfo ToList(Type type) => _toList ??= (typeof(Enumerable).GetMethod(nameof(Enumerable.ToList))!)
            .MakeGenericMethod(type);

       
    }
}
