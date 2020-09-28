using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.MVVM.Events
{

    public class HEvent : HEventBase
    {
        public void publish()
        {

        }
        public void subscribe(EventSubscriber listener)
        {
            SubscriberInfo _newinfo = new SubscriberInfo(listener);
            base.baseSubscribe(_newinfo);
        }
    }

    public class test
    {
        private HEventBase _baseevent = new HEvent();
        private void testm()
        {
            ((HEvent)_baseevent).subscribe(otha);
        }

        public void otha()
        {

        }
    }
}
