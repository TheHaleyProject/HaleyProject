using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Haley.MVVM.EventArguments;

namespace Haley.MVVM.Interfaces
{

    public interface IHaleyControlVM
    {
        event PropertyChangedEventHandler PropertyChanged;
        event EventHandler<FrameClosingEventArgs> OnControlClosed;
        void seed(object parameters);
    }
}
