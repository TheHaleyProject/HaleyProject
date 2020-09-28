using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.MVVM.Interfaces;
using Haley.MVVM.EventArguments;
using Haley.MVVM.Events;

namespace Haley.MVVM.IOC
{
    public static class ContainerStore
    {
        private static ControlContainer _control_container;
        public static ControlContainer controls
        {
            get 
            {
                if (_control_container == null) _control_container = new ControlContainer();
                return _control_container; 
            }
        }

        private static WindowContainer _window_container;
        public static WindowContainer windows
        {
            get 
            {
                if (_window_container == null) _window_container = new WindowContainer();
                return _window_container; 
            }
        }

    }
}
