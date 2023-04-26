using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Punica.Extensions.DependencyInjection
{
    //TODO: use type extensions
    public static class AssemblyScannerExtensions
    {
        public static void RegisterServices(this IServiceCollection services, Type targetType, Assembly[] assemblies,
            AssemblyScanOptions options)
        {
            if (targetType.IsOpenGeneric())
            {
                RegisterOpenTypes(services, targetType, options, assemblies);
            }
            else
            {
                RegisterAssignableTypes(services, options.Lifetime, targetType, assemblies);
            }
        }

        private static void RegisterOpenTypes(IServiceCollection services, Type templateType,
            AssemblyScanOptions options, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.DefinedTypes.Where(t => t.IsConstructable()))
                {
                    if (type.IsOpenGeneric()) // Is open generic Concrete<T1,T2>
                    {
                        if (options.RegisterOpenGenerics)
                        {
                            foreach (var implementedInterface in type.ImplementedInterfaces)
                            {
                                // length should match otherwise cannot be constructed.
                                // Concrete<T1> : Interface<T1,T2> impossible ,
                                // Concrete<T1, T2, T3> : Interface<T1,T2> cannot be constructed for the Interface<T1,T2>,
                                // TODO Concrete<T1> : Interface<T1,int> might be possible 
                                // TODO Concrete : Interface<string,int> might be possible
                                // TODO Concrete<T1,T2> : Interface<List<T1>, List<T2>> might be possible
                                // Concrete<T1> : Interface<string,int> not possible as missing T1
                                var implementedTypeInfo = implementedInterface;

                                if (implementedTypeInfo.IsOpenGeneric() &&
                                    implementedTypeInfo.GetGenericTypeDefinition() ==
                                    templateType && // matches interface exactly
                                    implementedTypeInfo.GenericTypeArguments.Length ==
                                    type.GetGenericArguments().Length &&
                                    implementedTypeInfo.GenericTypeArguments.All(x =>
                                        x.IsGenericParameter)) // all parameters should be generic parameters
                                {
                                    services.Add(new ServiceDescriptor(templateType, type, options.Lifetime));
                                }
                            }
                        }
                    }
                    else // but derived type is not open generic ex: Concrete, Concrete<string,int>
                    {
                        foreach (var implementedInterface in type.ImplementedInterfaces)
                        {
                            if (implementedInterface.IsConstructedGenericType &&
                                implementedInterface.GetGenericTypeDefinition() == templateType)
                            {
                                services.Add(new ServiceDescriptor(implementedInterface, type, options.Lifetime));
                            }
                        }
                    }

                }
            }
        }

        public static bool IsConstructable(this Type type)
        {
            return type.IsClass && !type.IsAbstract;
        }

        private static void RegisterAssignableTypes(IServiceCollection services, ServiceLifetime lifetime,
            Type serviceType, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.DefinedTypes.Where(t => t.IsConstructable()))
                {
                    if (!type.IsOpenGeneric()) // serviceType is closed type Concrete, Concrete<string,int>
                    {
                        if (serviceType.IsAssignableFrom(type))
                        {
                            services.Add(new ServiceDescriptor(serviceType, type, lifetime));
                        }
                    }
                    else // serviceType is open type Concrete<T1,T2>:Interface<T1, T2>
                    {
                        if (serviceType.IsConstructedGenericType) // Interface<string, string>
                        {
                            var genericType = serviceType.GetGenericTypeDefinition();
                            foreach (var implementedInterface in type.ImplementedInterfaces)
                            {
                                if (implementedInterface.GetGenericTypeDefinition() ==
                                    genericType && // matches interface exactly
                                    implementedInterface.GenericTypeArguments.Length ==
                                    type.GetGenericArguments().Length &&
                                    implementedInterface.GenericTypeArguments.All(x =>
                                        x.IsGenericParameter)) // all parameters should be generic parameters
                                {
                                    services.Add(new ServiceDescriptor(serviceType,
                                        type.MakeGenericType(serviceType.GenericTypeArguments), lifetime));
                                }
                            }
                        }
                    }

                }
            }
        }

       
    }

}
