using Haley.Abstractions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Events.Utils;

namespace Haley.Events
{
    public class SubscriberBase : ISubscriber
    {
        public string id { get; set; }
        public Action listener { get; set; }
        public Type declaring_type { get;}

        public SubscriberBase(Action _listener) 
        { 
            listener = _listener;
            declaring_type = listener.Method.DeclaringType;
            //If a GUID is directly used, it could result in duplicated entries.
            byte[] determinisitc_byte = HashHelper.computeHash(declaring_type.FullName + "###" + listener.Method.Name);
            id = new Guid(determinisitc_byte).ToString();
        }

        public void sendMessage(params object[] args)
        {
            listener.Invoke();
        }
    }

    public class SubscriberBase<T> : ISubscriber
    {
        public string id { get; set; }
        public Action<T> listener { get; set; }
        public Type declaring_type { get; }
        public SubscriberBase(Action<T> _listener) 
        {
            listener = _listener;
            declaring_type = listener.Method.DeclaringType;
            //If a GUID is directly used, it could result in duplicated entries.
            byte[] determinisitc_byte = HashHelper.computeHash(declaring_type.FullName + "###" + listener.Method.Name);
            id = new Guid(determinisitc_byte).ToString();
        }

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
