namespace Punica.Extensions
{
    public static class ReflectionInstanceExtensions
    {
        public static T? CreateInstance<T>(this Type type, params object?[]? args)
        {
            return (T?)Activator.CreateInstance(type, args);
        }
    }


}
