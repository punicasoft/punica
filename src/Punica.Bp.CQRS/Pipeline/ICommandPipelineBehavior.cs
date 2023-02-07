namespace Punica.Bp.CQRS.Pipeline
{
    public interface ICommandPipelineBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : notnull
    {
    }
}
