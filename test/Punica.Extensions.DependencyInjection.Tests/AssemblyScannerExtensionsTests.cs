using Microsoft.Extensions.DependencyInjection;

namespace Punica.Extensions.DependencyInjection.Tests
{
    public class AssemblyScannerExtensionsTests
    {
        private readonly IServiceCollection _services;

        public AssemblyScannerExtensionsTests()
        {
            _services = new ServiceCollection();
        }


        [Fact]
        public void RegisterServices_Should_Work_For_Interface()
        {
            ContainsOneRegistration<ICommand>();
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Generic_Parameters()
        {
            ContainsOneRegistration<ICommand<int>>();
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Generic_Parameters_2()
        {
            ContainsOneRegistration<ICommandHandler<CommandA>>();
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Generic_Parameters_3()
        {
            ContainsOneRegistration<ICommandHandler<string, string, string>>();
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Generic_Parameters_4()
        {
            ContainsOneRegistration<ICommandPipelineBehavior<string, string>>();
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Open_Generic()
        {
            ContainsOneRegistration<ICommandHandler<CommandA>>(typeof(ICommandHandler<>));
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Open_Generic_2()
        {
            ContainsOneRegistration<ICommandHandler<CommandB, string>>(typeof(ICommandHandler<,>));
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Open_Generic_3()
        {
            ContainsOneRegistration<ICommandHandler<string, string, string>>(typeof(ICommandHandler<,,>));
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Open_Generic_With_Open_Generic_Instance()
        {
            ContainsOneRegistration<ICommandHandler<string, string, string>>(typeof(ICommandHandler<,,>), true);
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Open_Generic_With_Open_Generic_Instance_2()
        {
            ContainsOneRegistration<ICommandPipelineBehavior<CommandB, string>>(typeof(ICommandPipelineBehavior<,>), true);
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Open_Generic_With_Open_Generic_Instance_3()
        {
            ContainsNoRegistration<ITestInterface<string, int>>(typeof(ITestInterface<,>), true);
        }

        [Fact]
        public void RegisterServices_Should_Work_For_Interface_With_Open_Generic_With_No_Genric_Construction()
        {
            ContainsOneRegistration<INotificationHandler<EntityEvent<Entity>>>(typeof(INotificationHandler<>), true);
            List<IDomainEvent> events = new List<IDomainEvent>();
            events.Add(new EntityEvent<Entity>());

            foreach (var domainEvent in events)
            {
                Publish(domainEvent);
            }
           
        }

        private void Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var serviceProvider = _services.BuildServiceProvider();
            var provider = serviceProvider.CreateScope().ServiceProvider;

            var type = notification.GetType();
            var genericType = typeof(INotificationHandler<>).MakeGenericType(type);
            //Activator.CreateInstance(typeof(CommandHandlerWrapper<>).MakeGenericType(type))!)

            var handlers = provider.GetServices(genericType); //provider.GetServices<INotificationHandler<TNotification>>();

            Assert.Single(handlers);

        }


        private void ContainsOneRegistration<T>()
        {
            _services.RegisterServices(typeof(T), new[] { this.GetType().Assembly }, new AssemblyScanOptions());

            var serviceProvider = _services.BuildServiceProvider();
            var provider = serviceProvider.CreateScope().ServiceProvider;
            var handlers = provider.GetServices<T>();
            Assert.Single(handlers);
        }

        private void ContainsOneRegistration<T>(Type type, bool registerOpenGenerics = false)
        {
            _services.RegisterServices(type, new[] { this.GetType().Assembly }, new AssemblyScanOptions()
            {
                RegisterOpenGenerics = registerOpenGenerics
            });

            var serviceProvider = _services.BuildServiceProvider();
            var provider = serviceProvider.CreateScope().ServiceProvider;
            var handlers = provider.GetServices<T>();
            Assert.Single(handlers);
        }

        private void ContainsNoRegistration<T>(Type type, bool registerOpenGenerics = false)
        {
            _services.RegisterServices(type, new[] { this.GetType().Assembly }, new AssemblyScanOptions()
            {
                RegisterOpenGenerics = registerOpenGenerics
            });

            var serviceProvider = _services.BuildServiceProvider();
            var provider = serviceProvider.CreateScope().ServiceProvider;
            var handlers = provider.GetServices<T>();
            Assert.Empty(handlers);
        }

    }


    public interface ICommand : ICommand<int>
    {
    }

    public interface ICommand<out TResponse>
    {
    }

    public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command, CancellationToken cancellationToken);
    }

    public interface ICommandHandler<in TCommand1, in TCommand2, TResponse>
    {
        Task<TResponse> Handle(TCommand1 command1, TCommand2 command2, CancellationToken cancellationToken);
    }

    public interface IPipelineBehavior<TMessage, TResponse> where TMessage : notnull
    {
        Task<TResponse> Handle(TMessage message, Func<TMessage, CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken);
    }

    public interface ICommandPipelineBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : notnull // where TCommand : ICommand<TResponse>
    {
    }

    public interface IQueryPipelineBehavior<TQuery, TResponse> : IPipelineBehavior<TQuery, TResponse> where TQuery : notnull
    {
    }

    public interface ITestInterface<in T1, in T2>
    {
        public void Test1(T1 t1, T2 t2);
    }

    public class CommandA : ICommand
    {

    }

    public class CommandB : ICommand<string>
    {

    }

    public class CommandHandlerA : ICommandHandler<CommandA>
    {
        public Task Handle(CommandA command, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class CommandHandlerB : ICommandHandler<CommandB, string>
    {
        public Task<string> Handle(CommandB command, CancellationToken cancellationToken)
        {
            return Task.FromResult("Hello");
        }
    }

    public class CommandHandlerC : ICommandHandler<string, string, string>
    {
        public Task<string> Handle(string command1, string command2, CancellationToken cancellationToken)
        {
            return Task.FromResult("Hello");
        }
    }

    public class CommandHandlerD<TCommand, TResponse> : ICommandHandler<string, string, string>, ITestInterface<TCommand, int>, ICommandPipelineBehavior<TCommand, TResponse> where TCommand : notnull
    {
        public Task<string> Handle(string command1, string command2, CancellationToken cancellationToken)
        {
            return Task.FromResult("Hello");
        }

        public void Test1(TCommand t1, int t2)
        {

        }

        public Task<TResponse> Handle(TCommand message, Func<TCommand, CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class CommandHandlerE<T1, T2, T3> : ICommandHandler<string, string, string>
    {
        public Task<string> Handle(string command1, string command2, CancellationToken cancellationToken)
        {
            return Task.FromResult("Hello");
        }
    }

    #region Genric Type Detection

    public class Entity
    {

    }

    public class EntityEvent<TAggregate> : IDomainEvent 
    {

    }

    public class EntityEventHandler : IDomainEventHandler<EntityEvent<Entity>>
    {
        public Task Handle(EntityEvent<Entity> notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDomainEvent
    {
    }

    public interface INotificationHandler<in TNotification>
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }

    public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    {
    }

    #endregion
}
