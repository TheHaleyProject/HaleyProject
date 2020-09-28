using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haley.MVVM.Events
{
    /// <summary>
    /// Implementing a simple observer pattern
    /// </summary>
    public class HEvent<T>: HEventBase
    {
        public void publish(T arguments)
        {
           
        }
        public void subscribe(EventSubscriber listener)
        {
        }
    }
}
