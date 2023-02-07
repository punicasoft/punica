using System.Text.Json;
using Castle.Core.Logging;
using Moq;
using Punica.Bp.CQRS.Handlers;
using Punica.Bp.CQRS.Messages;
using Punica.Bp.CQRS.Pipeline;

namespace Punica.Bp.CQRS.Tests
{
    public class UnitTest1
    {
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<ILogger> _loggerMock;

        public UnitTest1()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _loggerMock = new Mock<ILogger>();
        }


        [Fact]
        public async Task Test1()
        {

            _serviceProviderMock.Setup(p => p.GetService(typeof(ICommandHandler<Command1>))).Returns(new Handler1());
            _serviceProviderMock.Setup(p => p.GetService(typeof(IEnumerable<ICommandPipelineBehavior<Command1, Unit>>))).Returns(new List<ICommandPipelineBehavior<Command1, Unit>>()
            {
                new CommandLogBehavior<Command1, Unit>(_loggerMock.Object)
            });

            var mediator = new Mediator(_serviceProviderMock.Object);

            await mediator.Send(new Command1() { Name = "Randima" });
            _loggerMock.Verify(l=> l.Debug("{\"Name\":\"Randima\"}"));
        }

        public class Command1 : ICommand
        {
            public string? Name { get; set; }
        }

        public class Handler1 : ICommandHandler<Command1>
        {
            public async Task Handle(Command1 command, CancellationToken cancellationToken)
            {
                await Task.Delay(10, cancellationToken);
            }
        }

        public class CommandLogBehavior<TCommand, TResponse> : ICommandPipelineBehavior<TCommand, TResponse>
        {
            private readonly ILogger _logger;

            public CommandLogBehavior(ILogger logger)
            {
                _logger = logger;
            }

            public async Task<TResponse> Handle(TCommand message, Func<TCommand, CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
            {
                var serialize = JsonSerializer.Serialize(message);
                _logger.Debug(serialize);
                return await next(message, cancellationToken);
            }
        }
    }
}