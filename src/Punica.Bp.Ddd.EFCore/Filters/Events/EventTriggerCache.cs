using System.Reflection;
using Punica.Bp.Ddd.Domain.Events;

namespace Punica.Bp.Ddd.EFCore.Filters.Events
{
    public class EventTriggerCache : IEventTriggerCache
    {
        private Dictionary<Type,bool> _createdEvents = new Dictionary<Type,bool>();
        private Dictionary<Type, bool> _modifiedEvents = new Dictionary<Type, bool>();
        private Dictionary<Type, bool> _deletedEvents = new Dictionary<Type, bool>();

        public bool IsCreatedEventEnabled(Type type)
        {
            if (_createdEvents.TryGetValue(type, out var value))
            {
                return value;
            }

            var result = HasAttribute(type, typeof(EnableCreatedEventAttribute));
            _createdEvents[type] = result;
            return result;
        }

        public bool IsModifiedEventEnabled(Type type)
        {
            if (_modifiedEvents.TryGetValue(type, out var value))
            {
                return value;
            }

            var result = HasAttribute(type, typeof(EnableModifiedEventAttribute));
            _modifiedEvents[type] = result;
            return result;
        }

        public bool IsDeletedEventEnabled(Type type)
        {
            if (_deletedEvents.TryGetValue(type, out var value))
            {
                return value;
            }

            var result = HasAttribute(type, typeof(EnableCreatedEventAttribute));
            _deletedEvents[type] = result;
            return result;
        }

        private bool HasAttribute(Type type, Type attributeType)
        {
            var attributes = type.GetCustomAttributes(attributeType);

            if (attributes.Any())
            {
                return true;
            }

            return false;
        }
    }
}
