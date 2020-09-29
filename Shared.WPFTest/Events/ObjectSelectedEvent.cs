using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.MVVM.Events;
using Haley.MVVM.EventArguments;

namespace Test.Events
{
    public class ObjectSelectedEvent : HEvent
    {
    }

    public class ObjectDeletedEvent : HEvent<string>
    {
        
    }

}
