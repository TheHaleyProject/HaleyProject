using Haley.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Events
{
    /// <summary>
    /// Implementing a simple observer pattern
    /// </summary>
    public abstract class HBaseEvent
    {
        private List<ISubscriber> _subscribers = new List<ISubscriber>();

        protected virtual void basePublish(params object[] arguments)
        {
            lock(_subscribers)
            {
                // Using params keyword, because we can have zero or more parameters
                //This should invoke all the delegates
                foreach (var _subscriber in _subscribers)
                {
                    _subscriber.sendMessage(arguments);
                }
            }
        }

        protected virtual void baseSubscribe(ISubscriber subscriber)
        {
            lock(_subscribers)
            {
                if (_subscribers.Any(p => p.id == subscriber.id)) return;
                _subscribers.Add(subscriber);
            }
        }

        protected virtual bool baseUnSubscribe(string subscriber_id)
        {
            lock (_subscribers)
            {
                var _toremove = _subscribers.FirstOrDefault(p => p.id == subscriber_id);
                if (_toremove != null)
                {
                    _subscribers.Remove(_toremove);
                    return true;
                }
            }
            return false;
        }
        protected virtual void baseUnSubscriberAll()
        {
            _subscribers = new List<ISubscriber>();
        }

    }
}
