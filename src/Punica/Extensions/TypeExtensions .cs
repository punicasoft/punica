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
        /// TODO: handle type == fromType ? check whether it cause issue assembly scanner and use it since this is not used
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

        /// <summary>
        /// Get the implemented element type (first argument) of a type based on given generic implementation to look for.
        /// This will only return if the type implemented from the given generic. Best work with open generic while closed type supported may not be useful.   
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Check whether a given type is a collection. <see cref="Array"/> , <see cref="IList{T}"/>,  <see cref="ICollection{T}"/> are included
        /// but string does not included as a collection.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCollection(this Type type)
        {
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();

                if (genericTypeDefinition == typeof(List<>)
                    || genericTypeDefinition == typeof(IList<>)
                    || genericTypeDefinition == typeof(ICollection<>)
                    || genericTypeDefinition == typeof(IQueryable<>)
                    || (genericTypeDefinition == typeof(IEnumerable<>) && type != typeof(string)))
                {
                    return true;
                }
            }

            return type.IsArray;
        }

        /// <summary>
        /// Check whether a given type is a collection. <see cref="Array"/> , <see cref="IList{T}"/>,  <see cref="ICollection{T}"/> are included
        /// but string does not included as a collection.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="elementType">Returns element type of the collection null if it not a collection</param>
        /// <returns></returns>
        /// TODO: utilize Is implemented from
        public static bool IsCollection(this Type type, out Type? elementType)
        {
            elementType = null;

            var result = type.IsCollection();

            if (result)
            {
                elementType = type.GetElementOrGenericArgType();
            }

            return result;
            
        }


        /// <summary>
        /// Returns element type of collections such as <see cref="Array"/> , <see cref="IList{T}"/>,  <see cref="ICollection{T}"/>
        /// or element type of first argument of any generic type. However this will only look for any generic of given type not on any implemented type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type? GetElementOrGenericArgType(this Type type)
        {
            if (type.IsArray) return type.GetElementType();

            if (type.IsGenericType) return type.GetGenericArguments()[0];

            return null;
        }

       

    }
}
