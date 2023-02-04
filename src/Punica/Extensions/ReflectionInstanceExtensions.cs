namespace Punica.Extensions
{
    public static class ReflectionInstanceExtensions
    {

        //public static object? CreateGenericInstance(this Type type, Type genericType, params object?[]? args)
        //{
        //    var constructed = type.MakeGenericType(genericType);
        //    return Activator.CreateInstance(constructed, args);
        //}

        //public static T? CreateGenericInstance<T>(this Type type, Type genericType, params object?[]? args)
        //{
        //    var constructed = type.MakeGenericType(genericType);
        //    return (T?)Activator.CreateInstance(constructed, args);
        //}

        //public static object? CreateGenericInstance(this Type type, Type[] genericType, params object?[]? args)
        //{
        //    var constructed = type.MakeGenericType(genericType);
        //    return Activator.CreateInstance(constructed, args);
        //}

        public static T? CreateInstance<T>(this Type type, params object?[]? args)
        {
            return (T?)Activator.CreateInstance(type, args);
        }
    }


}
