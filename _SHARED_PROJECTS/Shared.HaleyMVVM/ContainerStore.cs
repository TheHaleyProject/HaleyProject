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
        public IHaleyDIContainer DI { get; set; }
        public IHaleyControlContainer controls { get; set; }
        public IHaleyWindowContainer windows { get; set; }

        public ContainerStore() 
        {
            DI = new DIContainer() {};
            controls = new ControlContainer(DI); 
            windows = new WindowContainer(DI);
        }
        public static ContainerStore Singleton = new ContainerStore();
    }
}
