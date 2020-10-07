using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using Haley.Events;
using Haley.MVVM.Containers;

namespace Haley.MVVM
{
    public sealed class ContainerStore
    {
        public DIContainer DI { get; set; }
        public ControlContainer controls { get; set; }
        public WindowContainer windows { get; set; }

        public ContainerStore() 
        { 
          controls = new ControlContainer(); 
          windows = new WindowContainer();
          DI = new DIContainer();
        }
        public static ContainerStore Singleton = new ContainerStore();
    }
}
