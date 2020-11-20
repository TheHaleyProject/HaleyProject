using System;
using Haley.Abstractions;
using Haley.Containers;

namespace Haley.MVVM
{
    public sealed class ContainerStore
    {
        public IHaleyDIContainer DI { get; set; }
        public IHaleyControlContainer<IHaleyControlVM,IHaleyControl> controls { get;  }
        public IHaleyWindowContainer<IHaleyWindowVM,IHaleyWindow> windows { get;  }

        public ContainerStore() 
        {
            DI = new DIContainer() {};
            controls = new ControlContainer(DI); 
            windows = new WindowContainer(DI);
        }
        public static ContainerStore Singleton = new ContainerStore();
    }
}
