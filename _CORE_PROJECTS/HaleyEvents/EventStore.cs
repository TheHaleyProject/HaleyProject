using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

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

        public void clearAll ()
        {
            _event_collection = new ConcurrentDictionary<Type, HBaseEvent>(); //Clear all previously subscribed events.
        }

        /// <summary>
        /// Clear all the events with the declaring parent matching the arguments.
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        public void clearAll<TParent>()
        {
            foreach (var _event in _event_collection.Values)
            {
                _event.unSubscribe<TParent>(); //This will try and remove the parents if already registered.
            }
        }

        public void clearAll(Type parent)
        {
            foreach (var _event in _event_collection.Values)
            {
                _event.unSubscribe(parent); //This will try and remove the parents if already registered.
            }
        }

        public static EventStore Singleton = new EventStore();
        public EventStore() { }
    }
}
