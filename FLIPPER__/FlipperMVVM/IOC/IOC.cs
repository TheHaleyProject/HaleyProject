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
    public static class Global
    {
        #region Properties
        private static bool _is_container_initiated = false;
        private static HaleyContainer _container;
        public static HaleyContainer container
        {
            get 
            {
                if (_container == null) _container = new HaleyContainer();
                return _container; 
            }
        }

        #endregion
    }
}
