using Punica.Bp.CQRS.Messages;

namespace Punica.Bp.CQRS
{
    public interface ISender
    {
        Task Send(ICommand command, CancellationToken cancellationToken = default);
        Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
        Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);

        //Task<TResponse> Send<TCommand , TResponse>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResponse>;
        //Task<TResponse> Send<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResponse>;
    }
}
