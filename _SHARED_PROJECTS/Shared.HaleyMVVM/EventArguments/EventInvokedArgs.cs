using System;
using System.Linq;

namespace Haley.Events
{
    public class EventInvokedArgs : EventArgs
    {
        public object sender { get; set; }
        public object message { get; set; }
        public EventInvokedArgs(object _sender, object _message)
        {
            sender = _sender;
            message = _message;
        }
    }
}
