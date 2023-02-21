using System.Reflection;
using Punica.Bp.Ddd.Domain.Events;

namespace Punica.Bp.Ddd.EFCore.Interceptors.Events
{
    public class EventTriggerCache : IEventTriggerCache
    {
        private readonly Dictionary<Type,bool> _createdEvents = new Dictionary<Type,bool>();
        private readonly Dictionary<Type, bool> _modifiedEvents = new Dictionary<Type, bool>();
        private readonly Dictionary<Type, bool> _deletedEvents = new Dictionary<Type, bool>();

        public bool IsCreatedEventEnabled(Type type)
        {
            if (_createdEvents.TryGetValue(type, out var value))
            {
                return value;
            }

            var result = type.IsDefined(typeof(EnableCreatedEventAttribute));
            _createdEvents[type] = result;
            return result;
        }

        public bool IsModifiedEventEnabled(Type type)
        {
            if (_modifiedEvents.TryGetValue(type, out var value))
            {
                return value;
            }

            var result = type.IsDefined(typeof(EnableModifiedEventAttribute)); 
            _modifiedEvents[type] = result;
            return result;
        }

        public bool IsDeletedEventEnabled(Type type)
        {
            if (_deletedEvents.TryGetValue(type, out var value))
            {
                return value;
            }

            var result = type.IsDefined(typeof(EnableDeletedEventAttribute)); 
            _deletedEvents[type] = result;
            return result;
        }
    }
}
