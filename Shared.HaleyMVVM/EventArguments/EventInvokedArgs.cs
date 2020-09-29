using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.MVVM.EventArguments
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
