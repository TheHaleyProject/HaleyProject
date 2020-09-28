using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Haley.MVVM.Events
{

    public class HEvent : HBaseEvent
    {
        public void publish()
        {
            //Publish without passing arguments
            base.basePublish();
        }
        public string subscribe(Action listener)
        {
            SubscriberBase _newinfo = new SubscriberBase(listener);
            base.baseSubscribe(_newinfo);
            return _newinfo.id; //Returning the subscription id
        }

        public bool unSubscribe(string subscription_key)
        {
           return base.baseUnSubscribe(subscription_key);
        }
    }

    public class HEvent<T> : HBaseEvent
    {
        public void publish(T eventArguments)
        {
            base.basePublish(eventArguments);
        }
        public string subscribe(Action<T> listener)
        {
            SubscriberBase<T> _newinfo = new SubscriberBase<T>(listener);
            base.baseSubscribe(_newinfo);
            return _newinfo.id; //Returning the subscription id
        }
        public bool unSubscribe(string subscription_key)
        {
           return base.baseUnSubscribe(subscription_key);
        }
    }
}
