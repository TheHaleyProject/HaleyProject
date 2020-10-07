using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.MVVM.Containers;
using Haley.Events;

namespace Haley.MVVM
{
    public sealed class EventStore
    {
        private Dictionary<Type, HBaseEvent> _event_collection = new Dictionary<Type, HBaseEvent>();
        // Core idea is that a list of delegates are stored. During run time, the delegates are invoked.

        public T GetEvent<T>() where T : HBaseEvent,new()
        {
            Type _target_type = typeof(T);
            if (!_event_collection.ContainsKey(_target_type))
            {
                //If key is not present , add it
                _event_collection.Add(_target_type, new T());
            }
            T result = (T) _event_collection[_target_type] ?? null;
            return result;
        }

        public static EventStore Singleton = new EventStore();
        public EventStore() { }
    }
}
