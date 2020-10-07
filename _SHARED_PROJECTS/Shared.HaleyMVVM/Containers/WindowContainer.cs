using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using Haley.Events;

namespace Haley.MVVM.Containers
{
    public sealed class WindowContainer : IHaleyWindowContainer  //Implementation of the DialogService Interface.
    {
        private Dictionary<Type, Type> view_viewmodel_mapping;
        public WindowContainer()
        {
            view_viewmodel_mapping = new Dictionary<Type, Type>();
        }

        public void register<ViewModelType, ViewType>()
            where ViewModelType : IHaleyWindowVM,new()
            where ViewType : IHaleyWindow 
        {
            if (view_viewmodel_mapping.ContainsKey(typeof(ViewModelType)) == true)
            {
                throw new ArgumentException($"{typeof(ViewModelType)} is already registered to {typeof(ViewType)}");
            }
            view_viewmodel_mapping.Add(typeof(ViewModelType), typeof(ViewType));
        }

        public bool? showDialog<ViewModelType>(ViewModelType InputViewModel) where ViewModelType : IHaleyWindowVM
        {
            IHaleyWindow  Result = _getWindowToDisplay(InputViewModel);
            if (Result == null) return null;
            return Result.ShowDialog(); //Once initiated, it will automatically unsubscribe as well.
        }

        public void show<ViewModelType>(ViewModelType InputViewModel) where ViewModelType : IHaleyWindowVM
        {
            IHaleyWindow  Result = _getWindowToDisplay(InputViewModel);
            Result.Show();
        }

       private IHaleyWindow  _getWindowToDisplay<ViewModelType>(ViewModelType InputViewModel) where ViewModelType : IHaleyWindowVM
        {
            if (view_viewmodel_mapping.Count == 0) return null;
            if (view_viewmodel_mapping.ContainsKey(typeof(ViewModelType)) == false) //Note: We are using typeof(viewmodeltype)
            {
                throw new ArgumentException($"{typeof(ViewModelType)} is not registered to any Views. Please check again.");
            }
            Type ViewType = view_viewmodel_mapping[typeof(ViewModelType)];

            //Using System.Activator, create an instance of the above retrieved view and display it. Remember all views are already implementing IHaleyWindow  interface. So, Casting the object as the IHaleyWindow .
            IHaleyWindow  WindowToDisplay = (IHaleyWindow )System.Activator.CreateInstance(ViewType);
            WindowToDisplay.DataContext = InputViewModel;

            //Once we receive the view to display in above stage, we can display the view to the user and obtain the DialogResult using the button clicks.To achieve button clicks events, we need to write codes in the Code behind. To avoid this, we are creating a below listener class to ensure MVVM implementations and separation of logics.

            //Now, we can show the window as a dialog (using showdialog). 
            // We need a publisher (which raises an event, in this case, ViewModel) and a subscriber (which will subscribe to the Publisher)
            CustomObserverPattern CustomOP = new CustomObserverPattern(WindowToDisplay, InputViewModel);
            CustomOP.subscribe();
            return WindowToDisplay;
        }
    }

    public class CustomObserverPattern
    {
        //This is kind of interlinked. Ideally, View is the one which initiates the Close command. 
        // For example,  1. when we press a button in View, it sends a command to ViewModel to perform a Action.
        //2. At the end of the action, it triggers the event inside the ViewModel (OnClosingEvent; it is actually publishing the status about the end of process.) 
        //3. Status can either be true or false. Which means, we can either close the window or keep it active. 
        //4. So, View, which has subscribed to that event, receives that status and saves as dialog result. If dialog result is true, then it will close the winodw else it will keep it active.
        public IHaleyWindow  subscriber { get; set; }
        public IHaleyWindowVM publisher { get; set; }

        public void subscribe()
        {
            publisher.OnWindowsClosed += _onWindowsClosedHandler;
        }

        public void unSubscribe() // This takes care of unsubscribing the events
        {
            publisher.OnWindowsClosed -= _onWindowsClosedHandler;
        }

        private void _onWindowsClosedHandler(object sender, FrameClosingEventArgs e)
        {
            if (e.event_result.HasValue)
            {
                subscriber.DialogResult = e.event_result; //The event result will be invoked when a user presses a button. When user presses a button, it will invoke the event stored inside the viewmodel with an input value (bool, in our case). Thus, the event inside the viewmodel is invoked with a value which in turn is stored in the dialogresult.
                unSubscribe(); //the moment we get the dialog resut. we can unsubscribe.
            }
            else
            {
                unSubscribe(); // Under any case, we need to unsubscribe first before closing the dialog window.
                subscriber.Close(); //If user manages to close the dialog without using the predefined button, then the window is closed.
            }
        }

        public CustomObserverPattern(IHaleyWindow  subscriberView, IHaleyWindowVM publisherViewModel)
        {
            subscriber = subscriberView;
            publisher = publisherViewModel;
        }
    }
}
