using System.Reflection;

namespace ExpressionDynamicTest.Parsing
{
    public static class CachedMethodInfo
    {
        public static MethodInfo StringContains = typeof(string).GetMethod(nameof(string.Contains), new Type[]{typeof(string)});

        private static MethodInfo? _concatMethod;

        public static MethodInfo ConcatMethod => _concatMethod ??= new Func<string?, string?, string>(string.Concat).GetMethodInfo();

        private static MethodInfo? _enumerableContainsMethod;
        public static MethodInfo EnumerableContainsMethod(Type type) => (_enumerableContainsMethod ??= new Func<IEnumerable<object>, object, bool>(Enumerable.Contains).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);

        private static MethodInfo? _anyMethod;
        public static MethodInfo AnyMethod(Type type) =>  (_anyMethod ??= new Func<IEnumerable<object>, Func<object, bool>, bool>(Enumerable.Any).GetMethodInfo().GetGenericMethodDefinition())
            .MakeGenericMethod(type);
    }
}
