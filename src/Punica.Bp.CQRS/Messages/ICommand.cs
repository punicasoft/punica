namespace Punica.Bp.CQRS.Messages
{
    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<out TResponse>
    {
    }
}
