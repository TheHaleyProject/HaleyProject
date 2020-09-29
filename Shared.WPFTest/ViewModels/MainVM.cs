using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.MVVM.EventArguments;
using Haley.MVVM.Interfaces;
using Haley.MVVM;
using Haley.MVVM.Models;
using System.Windows.Input;
using Test.Events;

namespace Test.ViewModels
{
    public class MainVM : ChangeNotifier, IHaleyWindowVM
    {
        private string _message;
        public string message
        {
            get { return _message; }
            set { _message = value; onPropertyChanged(); }
        }

        public ICommand CMD_Method1 => new HCommand(invokeMethod);

        public ICommand CMD_Method2 => new HCommand(invokeMethod);

        private void invokeMethod(object obj)
        {
            int input = int.Parse((string)obj);
            switch(input)
            {
                case 1:
                    EventStore.Singleton.GetEvent<ObjectSelectedEvent>().publish();
                    break;
                case 2:
                    EventStore.Singleton.GetEvent<ObjectDeletedEvent>().publish("Deleted something");
                    break;
            }
        }
        public MainVM() 
        {
            message = "Startup";
            OnWindowsClosed += MainVM_OnWindowsClosed;
        }

        private void MainVM_OnWindowsClosed(object sender, FrameClosingEventArgs e)
        {
            
        }

        public event EventHandler<FrameClosingEventArgs> OnWindowsClosed;
    }
}
