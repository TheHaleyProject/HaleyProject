using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Haley.Events
{

    public class HEvent : HBaseEvent
    {
        public void publish()
        {
            //Publish without passing arguments
            basePublish();
        }
        public string subscribe(Action listener)
        {
            SubscriberBase _newinfo = new SubscriberBase(listener);
            return baseSubscribe(_newinfo); //Returning the subscription id
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
            return baseSubscribe(_newinfo); //Returning the subscription id
        }
    }
}
