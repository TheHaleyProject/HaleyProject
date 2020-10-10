using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using Haley.Events;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Threading;

namespace Haley.MVVM.Containers
{
    public sealed class WindowContainer : IHaleyWindowContainer  //Implementation of the DialogService Interface.
    {
        private IHaleyDIContainer _di_instance = new DIContainer() { };
        private ConcurrentDictionary<Type, Type> view_vm_mapping = new ConcurrentDictionary<Type, Type>();
        
        public WindowContainer(IHaleyDIContainer _injection_container = null) 
        {
        if (_injection_container != null)
            {
                _di_instance = _injection_container;
            }
        }

        #region Registration Methods
        public void register<ViewModelType, ViewType>(ViewModelType instance = null)
            where ViewModelType : class, IHaleyWindowVM
            where ViewType : IHaleyWindow
        {
            //First map the viewmodel type with its view.
            if (view_vm_mapping.ContainsKey(typeof(ViewModelType)) == true)
            {
                throw new ArgumentException($"{typeof(ViewModelType)} is already registered to {typeof(ViewType)}");
            }
            view_vm_mapping.TryAdd(typeof(ViewModelType), typeof(ViewType));

            var _status = _di_instance.checkIfRegistered(typeof(ViewModelType));
            if (!_status.status)
            {
                _di_instance.Register<ViewModelType>(instance); //If instance is null, a new instance will be created by DI
            }
        }
        #endregion

        #region Show Methods
        public bool? showDialog<ViewModelType>(ViewModelType InputViewModel = null, bool generate_vm_instance = false) where ViewModelType : class, IHaleyWindowVM
        {
            return _invokeDisplay(InputViewModel, generate_vm_instance, false); //This is modal
        }

        public void show<ViewModelType>(ViewModelType InputViewModel = null, bool generate_vm_instance = false) where ViewModelType : class, IHaleyWindowVM
        {
            _invokeDisplay(InputViewModel, generate_vm_instance, true); //This is modeless
        }

        #endregion

        #region Private Methods
        private IHaleyWindow _getWindowToDisplay<ViewModelType>(ViewModelType InputViewModel) where ViewModelType : IHaleyWindowVM
        {
            if (view_vm_mapping.ContainsKey(typeof(ViewModelType)) == false || view_vm_mapping.Count == 0) //Note: We are using typeof(viewmodeltype)
            {
                throw new ArgumentException($"{typeof(ViewModelType)} is not registered to any Views. Please check again.");
            }
            Type ViewType = view_vm_mapping[typeof(ViewModelType)];
            IHaleyWindow WindowToDisplay = null;

            //Using System.Activator, create an instance of the above retrieved view and display it. Remember all views are already implementing IHaleyWindow  interface. So, Casting the object as the IHaleyWindow .
             WindowToDisplay = _createWindowInstance<ViewModelType>(ViewType, InputViewModel);

            //Once we receive the view to display in above stage, we can display the view to the user and obtain the DialogResult using the button clicks.To achieve button clicks events, we need to write codes in the Code behind. To avoid this, we are creating a below listener class to ensure MVVM implementations and separation of logics.

            //Now, we can show the window as a dialog (using showdialog). 
            // We need a publisher (which raises an event, in this case, ViewModel) and a subscriber (which will subscribe to the Publisher)
            HaleyObserver CustomOP = new HaleyObserver(WindowToDisplay, InputViewModel);
            CustomOP.subscribe();
            return WindowToDisplay;
        }
        private IHaleyWindow _createWindowInstance<ViewModelType>(Type ViewType,ViewModelType Instance) where ViewModelType : IHaleyWindowVM
        {
            IHaleyWindow WindowToDisplay = (IHaleyWindow)System.Activator.CreateInstance(ViewType);
            WindowToDisplay.DataContext = Instance;
            return WindowToDisplay;
        }
        private bool? _getresult<ViewModelType>(ViewModelType InputViewModel, bool is_modeless=false) where ViewModelType : class, IHaleyWindowVM
        {
            bool? _result = false;
            IHaleyWindow _hwindow = null;
            _hwindow = _getWindowToDisplay(InputViewModel);
            if (_hwindow != null)
            {
                if(is_modeless)
                {
                    _hwindow.Show(); //Modeless
                }
                else
                {
                    _result = _hwindow.ShowDialog(); //Modal
                }
            }
            return _result;
        }
        private bool? _invokeDisplay<ViewModelType>(ViewModelType InputViewModel = null, bool generate_vm_instance = false, bool is_modeless= false) where ViewModelType : class, IHaleyWindowVM
        {
            bool? _result = null;
            if (InputViewModel == null)
            {
                InputViewModel = _di_instance.Resolve<ViewModelType>(generate_vm_instance);
            }
            //If Thread is not STA
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                Thread new_ui_thread = new Thread(() =>
                {
                    _result = _getresult(InputViewModel, is_modeless);
                });
                new_ui_thread.SetApartmentState(ApartmentState.STA);
                new_ui_thread.Start();
                new_ui_thread.Join();
            }
            else
            {
                _result = _getresult(InputViewModel);
            }

            return _result;
        }
        #endregion
    }

    public class HaleyObserver
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

        public HaleyObserver(IHaleyWindow  subscriberView, IHaleyWindowVM publisherViewModel)
        {
            subscriber = subscriberView;
            publisher = publisherViewModel;
        }
    }
}
