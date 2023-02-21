namespace Punica.Bp.Ddd.EFCore.Interceptors.Events
{
    public interface IEventTriggerCache
    {
        bool IsCreatedEventEnabled(Type type);
        bool IsModifiedEventEnabled(Type type);
        bool IsDeletedEventEnabled(Type type);
    }
}
