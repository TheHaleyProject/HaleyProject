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
        protected virtual string baseSubscribe(ISubscriber subscriber, bool allow_duplicates=false)
        {
            if (!allow_duplicates)
            {
                var _kvp = _subscribers.FirstOrDefault(sub =>
            sub.Value.listener_method == subscriber.listener_method && sub.Value.declaring_type == subscriber.declaring_type);
                if (_kvp.Value != null)
                {
                    return _kvp.Value.id;
                }
            }
            
            _subscribers.TryAdd(subscriber.id, subscriber);
            return subscriber.id;
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
        /// <summary>
        /// Subscribers with the input key will be unsubscribed.
        /// </summary>
        /// <param name="subscription_key"></param>
        /// <returns></returns>
        public virtual bool unSubscribe(string subscription_key) //Only one item will be unsubscribed.
        {
            return baseUnSubscribe(subscription_key);
        }

        /// <summary>
        /// All subscribers(delegates) with the declaring parent type will be unsubscribed.
        /// </summary>
        /// <param name="subscription_key"></param>
        /// <returns></returns>
        public virtual bool unSubscribe(Type declaring_parent) //Only one item will be unsubscribed.
        {
            if (declaring_parent == null) return false;
            try
            {
                var _toremove = _subscribers.Where(_kvp => _kvp.Value.declaring_type == declaring_parent)?.Select(p => p.Key)?.ToList();
                if (_toremove == null || _toremove.Count == 0) return false;

                foreach (var item in _toremove)
                {
                    baseUnSubscribe(item);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// All subscribers(delegates) with the declaring parent type will be unsubscribed.
        /// </summary>
        /// <typeparam name="TParent">Declaring Type to be removed.</typeparam>
        /// <returns></returns>
        public virtual bool unSubscribe<TParent>()
        {
            return unSubscribe(typeof(TParent));
        }

        #endregion
    }
}
