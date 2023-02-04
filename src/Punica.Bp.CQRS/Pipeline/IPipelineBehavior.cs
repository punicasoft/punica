namespace Punica.Bp.CQRS.Pipeline
{
    public interface IPipelineBehavior<TMessage, TResponse> where TMessage : notnull
    {
        Task<TResponse> Handle(TMessage message, Func<TMessage, CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken);
    }
}
