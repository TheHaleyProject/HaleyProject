using Haley.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Haley.Events
{
    /// <summary>
    /// Implementing a simple observer pattern
    /// </summary>
    public abstract class HBaseEvent
    {
        //private ConcurrentBag<ISubscriber> _subscribers = new ConcurrentBag<ISubscriber>();
        private ConcurrentDictionary<string, ISubscriber> _subscribers = new ConcurrentDictionary<string, ISubscriber>();

        #region PROTECTED METHODS
        protected virtual void basePublish(params object[] arguments)
        {
            // Using params keyword, because we can have zero or more parameters
            //This should invoke all the delegates
            foreach (var _subscriber in _subscribers)
            {
                _subscriber.Value.sendMessage(arguments);
            }
        }
        protected virtual void baseSubscribe(ISubscriber subscriber)
        {
            if (_subscribers.ContainsKey(subscriber.id)) return;
            _subscribers.TryAdd(subscriber.id, subscriber);
        }
        protected virtual bool baseUnSubscribe(string subscriber_id)
        {
            ISubscriber _removed_value;
            var _removed = _subscribers.TryRemove(subscriber_id, out _removed_value);
            return _removed;
        }
        protected virtual void baseUnSubscriberAll()
        {
            _subscribers = new ConcurrentDictionary<string, ISubscriber>();
        }
        #endregion

        #region VIRTUAL METHODS
        public virtual bool unSubscribe(string subscription_key)
        {
            return baseUnSubscribe(subscription_key);
        }
        
        #endregion
    }
}
