using System.Reflection;

namespace Punica.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Check assignable types including open generic, generic type assignability
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fromType"></param>
        /// <returns></returns>
        public static bool IsImplementedFrom(this Type type, Type fromType)
        {
            Type? implementedType = null;


            if (type.IsAssignableTo(fromType))
            {
                return true;
            }

            if (!fromType.IsGenericType) return false;
            var interfaces = type.GetInterfaces();
            var genericTypeDefinition = fromType.GetGenericTypeDefinition();


            implementedType = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition);


            if (implementedType != null)
            {
                if (fromType.IsGenericTypeDefinition)
                {
                    return true;
                }
                else if (implementedType.GetGenericArguments()[0]
                         .IsImplementedFrom(fromType.GetGenericArguments()[0]))
                {
                    return true;
                }

                return false;

            }

            return false;

        }

        public static Type? GetImplementedType(this Type type, Type genericType)
        {
            Type? implementedType = null;

            if (!genericType.IsGenericType) return null;
            var interfaces = type.GetInterfaces();
            var genericTypeDefinition = genericType.GetGenericTypeDefinition();


            implementedType = interfaces.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition);


            if (implementedType != null)
            {
                if (genericType.IsGenericTypeDefinition)
                {
                    return implementedType.GetGenericArguments()[0];
                }

                if (implementedType.GetGenericArguments()[0]
                    .IsImplementedFrom(genericType.GetGenericArguments()[0]))
                {
                    return implementedType.GetGenericArguments()[0];
                }
            }

            return null;

        }

        public static bool IsOpenGeneric(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition || type.GetTypeInfo().ContainsGenericParameters;
        }

    }
}
