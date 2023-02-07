using Punica.Bp.CQRS.Messages;
using Punica.Bp.CQRS.Handlers;

namespace Punica.Bp.CQRS.Extensions
{
    public static class MediatorExtensions
    {
        public static async Task<Unit> Convert<TCommand>(this ICommandHandler<TCommand> handler, TCommand command, CancellationToken cancellationToken) where TCommand : ICommand
        {
            await handler.Handle(command, cancellationToken);
            return Unit.Value;
        }
    }

   
}
