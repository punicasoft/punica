using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Punica.Bp.CQRS;
using Punica.Bp.CQRS.Extensions.DependencyInjection;
using Punica.Bp.CQRS.Handlers;
using Punica.Bp.CQRS.Pipeline;
using Punica.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
            => services.AddMediator(assemblies, null);

        public static IServiceCollection AddMediator(this IServiceCollection services, params Type[] types)
            => services.AddMediator(types.Select(t => t.GetTypeInfo().Assembly), null);

        public static IServiceCollection AddMediator(this IServiceCollection services, Action<MediatorOptions>? options, params Assembly[] assemblies)
            => services.AddMediator(assemblies, options);

        public static IServiceCollection AddMediator(this IServiceCollection services, Action<MediatorOptions>? options, params Type[] types)
            => services.AddMediator(types.Select(t => t.GetTypeInfo().Assembly), options);


        public static IServiceCollection AddMediator(this IServiceCollection services, IEnumerable<Assembly> assemblies, Action<MediatorOptions>? options)
        {
            if (!assemblies.Any())
            {
                throw new ArgumentException("Supply at least one assembly to scan for handlers.");
            }

            var option = new MediatorOptions();

            options?.Invoke(option);

            AddCoreServices(services, option);

            AddImplementations(services, assemblies.ToArray(), option);
            
            return services;
        }

        private static void AddCoreServices(IServiceCollection services, MediatorOptions options)
        {
            services.TryAdd(new ServiceDescriptor(typeof(IMediator), options.ImplementationType, options.Lifetime));
            services.TryAdd(new ServiceDescriptor(typeof(ISender), sp => sp.GetRequiredService<IMediator>(), options.Lifetime));
            services.TryAdd(new ServiceDescriptor(typeof(IPublisher), sp => sp.GetRequiredService<IMediator>(), options.Lifetime));
        }

        private static void AddImplementations(IServiceCollection services, Assembly[] assemblies, MediatorOptions options)
        {

            services.RegisterServices(typeof(ICommandHandler<>), assemblies, new AssemblyScanOptions() { Lifetime = options.Lifetime, RegisterOpenGenerics = false});
            services.RegisterServices(typeof(ICommandHandler<,>), assemblies, new AssemblyScanOptions() { Lifetime = options.Lifetime, RegisterOpenGenerics = false });
            services.RegisterServices(typeof(IQueryHandler<,>), assemblies, new AssemblyScanOptions() { Lifetime = options.Lifetime, RegisterOpenGenerics = false });

            services.RegisterServices(typeof(INotificationHandler<>), assemblies, new AssemblyScanOptions() { Lifetime = options.Lifetime, RegisterOpenGenerics = true });
            services.RegisterServices(typeof(ICommandPipelineBehavior<,>), assemblies, new AssemblyScanOptions() { Lifetime = options.Lifetime, RegisterOpenGenerics = true });
            services.RegisterServices(typeof(IQueryPipelineBehavior<,>), assemblies, new AssemblyScanOptions() { Lifetime = options.Lifetime, RegisterOpenGenerics = true });
        }

       
    }
}
