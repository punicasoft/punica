namespace Punica.Bp.CQRS
{
    public class MissingHandlerException : Exception
    {
        public object? MediatorMessage { get; }

        public MissingHandlerException(object? message)
            : base("No handler registered for message type: " + message?.GetType().FullName ?? "Unknown")
        {
            MediatorMessage = message;
        }
    }
}
