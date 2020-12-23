using System;
using System.Collections.Generic;
using System.Text;
using Haley.Abstractions;

namespace Haley.IOC
{
    public sealed class IOCStore
    {
        public IHaleyDIContainer DI { get; }

        public IOCStore()
        {
            DI = new DIContainer() { };
        }
        public static IOCStore Singleton = new IOCStore();
    }
}
