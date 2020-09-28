using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.MVVM.Events
{
    /// <summary>
    /// Implementing a simple observer pattern
    /// </summary>
    public abstract class HEventBase
    {
        private List<SubscriberInfo> _subscribers = new List<SubscriberInfo>();

        protected virtual void basePublish(params object[] arguments)
        {
            //This should invoke all the delegates
            foreach (var _subscriber in _subscribers)
            {
                if (_subscriber.listener == null) continue; //Don't invoke null delegates
                _subscriber.listener(arguments);
            }
        }

        protected virtual void baseSubscribe(SubscriberInfo subscriber)
        {
            if (_subscribers.Any(p => p.id == subscriber.id)) return;
            _subscribers.Add(subscriber);
        }

    }
}
