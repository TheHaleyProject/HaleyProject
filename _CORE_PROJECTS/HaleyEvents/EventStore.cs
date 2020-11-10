using System;
using System.Collections.Concurrent;

namespace Haley.Events
{
    public sealed class EventStore
    {
        private ConcurrentDictionary<Type, HBaseEvent> _event_collection = new ConcurrentDictionary<Type, HBaseEvent>();
        // Core idea is that a list of delegates are stored. During run time, the delegates are invoked.

        public T GetEvent<T>() where T : HBaseEvent,new()
        {
            Type _target_type = typeof(T);
            if (!_event_collection.ContainsKey(_target_type))
            {
                //If key is not present , add it
                _event_collection.TryAdd(_target_type, new T());
            }
            T result = (T) _event_collection[_target_type] ?? null;
            return result;
        }

        public static EventStore Singleton = new EventStore();
        public EventStore() { }
    }
}
