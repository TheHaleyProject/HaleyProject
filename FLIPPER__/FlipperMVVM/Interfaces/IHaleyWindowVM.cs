using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Haley.MVVM.EventArguments;

namespace Haley.MVVM.Interfaces
{
        public interface IHaleyWindowVM 
        {
            event EventHandler<FrameClosingEventArgs> OnWindowsClosed; //this is the name... :) :) :) 
        }
}
