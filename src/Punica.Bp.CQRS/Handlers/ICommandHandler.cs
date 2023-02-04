using Punica.Bp.CQRS.Messages;

namespace Punica.Bp.CQRS.Handlers
{
    public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command, CancellationToken cancellationToken);
    }


    //internal class CommandHandler<TCommand1, TResponse> : ICommandHandler<TCommand1, TResponse> where TCommand1 : ICommand<TResponse>
    //{
    //    private readonly ICommandHandler<>
    //    public Task<TResponse> Handle(TCommand1 command, CancellationToken cancellationToken)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    public static class TestExtension
    {
        public static async Task<Unit> Convert<TCommand>(this ICommandHandler<TCommand> handler, TCommand command, CancellationToken cancellationToken) where TCommand : ICommand 
        {
            await handler.Handle(command, cancellationToken);
            return Unit.Value;
        }
    }

}
