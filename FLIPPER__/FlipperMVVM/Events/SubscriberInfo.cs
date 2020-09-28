using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.MVVM.Events
{
    public delegate void EventSubscriber(params object[] args);

    public class SubscriberInfo
    {
        public string id { get; set; }
        public Action listener { get; set; }
        public SubscriberInfo(Action _listener) { id = Guid.NewGuid().ToString(); listener = _listener; }
    }
}
