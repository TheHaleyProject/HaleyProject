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
        //This will store all the subscriber key against each declaring type. Then we can easily unsubcribe them.
        private ConcurrentDictionary<Type, ConcurrentBag<string>> _declaring_types = new ConcurrentDictionary<Type, ConcurrentBag<string>>(); 
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
        protected virtual bool baseSubscribe(ISubscriber subscriber)
        {
            if (_subscribers.ContainsKey(subscriber.id)) return false;
            return _subscribers.TryAdd(subscriber.id, subscriber);
        }

        protected virtual void baseRegisterDeclaringType(Type declaring_type, string subscriber_id)
        {
            if (_declaring_types.ContainsKey(declaring_type))
            {
                _declaring_types[declaring_type].Add(subscriber_id);
            }
            else
            {
                _declaring_types.TryAdd(declaring_type, new ConcurrentBag<string>() { subscriber_id });
            }
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
        /// All subscribers(delegates) with the input parent type will be unsubscribed.
        /// </summary>
        /// <param name="subscription_key"></param>
        /// <returns></returns>
        public virtual bool unSubscribe(Type parent) //Only one item will be unsubscribed.
        {
            if (!_declaring_types.ContainsKey(parent)) return false; //No type registered.
            if (_declaring_types[parent].Count == 0) return false; //No keys found.

            foreach (var _key in _declaring_types[parent])
            {
                baseUnSubscribe(_key); //If exists remove.
            }

            //Finally, remove this type as well
            ConcurrentBag<string> outlist = new ConcurrentBag<string>();
            return _declaring_types.TryRemove(parent, out outlist);
        }

        #endregion
    }
}
