using Haley.MVVM.Interfaces;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.MVVM.Events
{
    public class SubscriberBase : ISubscriber
    {
        public string id { get; set; }
        public Action listener { get; set; }
        public SubscriberBase(Action _listener) { id = Guid.NewGuid().ToString(); listener = _listener; }

        public void sendMessage(params object[] args)
        {
            listener.Invoke();
        }
    }

    public class SubscriberBase<T> : ISubscriber
    {
        public string id { get; set; }
        public Action<T> listener { get; set; }
        public SubscriberBase(Action<T> _listener) { id = Guid.NewGuid().ToString(); listener = _listener; }

        public void sendMessage(params object[] args)
        {
            var _incomingtype = args[0].GetType();
            var _this_generic = this.GetType().GetGenericArguments()[0];
            if ( _this_generic.IsAssignableFrom(_incomingtype))
            {
                listener.Invoke((T)args[0]);
            }
        }
    }
}
