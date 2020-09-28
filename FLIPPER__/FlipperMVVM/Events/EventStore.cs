using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.MVVM.Events
{
    public class EventStore
    {
        private Dictionary<Type, HEvent> _event_collection = new Dictionary<Type, HEvent>();
        // Core idea is that a list of delegates are stored. During run time, the delegates are invoked.

        public T Get<T>() where T : HEvent
        {

        }

        public static EventStore Singleton = new EventStore();
        public EventStore() { }
    }
}
