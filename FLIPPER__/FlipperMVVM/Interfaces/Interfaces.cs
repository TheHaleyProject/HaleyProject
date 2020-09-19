using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.MVVM.Interfaces
{
    #region DialogService

        public interface IFlipperWindow
        {
#pragma warning disable IDE1006 // Naming Styles

            bool? DialogResult { get; set; } 
            Object DataContext { get; set; } 
            void Close(); 
            bool? ShowDialog(); 
            void Show();

#pragma warning restore IDE1006 // Naming Styles
    }

        public interface IFlipperWindowVM 
        {
            event EventHandler<FrameClosingEventArgs> OnWindowsClosed; //this is the name... :) :) :) 
        }

        public interface IFlipperWindowService 
        {
            void register<ViewModelType, ViewType>() where ViewModelType : IFlipperWindowVM where ViewType : IFlipperWindow; 
            bool? showDialog<ViewModelType>(ViewModelType InputViewModel) where ViewModelType : IFlipperWindowVM;
            void show<ViewModelType>(ViewModelType InputViewModel) where ViewModelType : IFlipperWindowVM;
    }

        
    #endregion

    #region Controls Service

    public interface IFlipperControl
    {
        object DataContext { get; set; }
    }

    public interface IFlipperControlVM
    {
        event PropertyChangedEventHandler PropertyChanged;
        event EventHandler<FrameClosingEventArgs> OnControlClosed;
        void seed(object parameters);
    }

    #endregion

    public interface ICheckedItem : INotifyPropertyChanged
    {
        bool is_checked { get; set; }
        String name { get; set; }
        int id { get; set; }
    }

    public class FrameClosingEventArgs : EventArgs
    {
        public bool? event_result { get; set; } //Should carry if the window closed.
        public object message { get; set; }
        public FrameClosingEventArgs(bool DialogResult, object _message)
        {
            event_result = DialogResult;
            message = _message;
        }
    }

}
