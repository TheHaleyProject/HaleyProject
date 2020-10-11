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
using Haley.Models;
using Haley.Utils;

namespace Haley.MVVM.Containers
{
    public sealed class WindowContainer : UIContainerBases<IHaleyWindowVM,IHaleyWindow>, IHaleyWindowContainer<IHaleyWindowVM, IHaleyWindow>  //Implementation of the DialogService Interface.
    {
        public WindowContainer(IHaleyDIContainer _injection_container = null) : base(_injection_container) { }

        #region ShowDialog Methods
        public bool? showDialog(Enum key, object InputViewModel = null, bool generate_vm_instance = false)
        {
            string _key = StringHelpers.getEnumAsKey(key);
            return showDialog(_key, InputViewModel, generate_vm_instance);
        }
        public bool? showDialog<ViewModelType>(ViewModelType InputViewModel = null, bool generate_vm_instance = false) where ViewModelType : class, IHaleyWindowVM
        {
            string _key = typeof(ViewModelType).FullName;
            return showDialog(_key, InputViewModel, generate_vm_instance);
        }
        public bool? showDialog<ViewType>(bool generate_vm_instance = false) where ViewType : IHaleyWindow
        {
            string _key = typeof(ViewType).FullName;
            return showDialog(_key, null, generate_vm_instance);
        }
        public bool? showDialog(string key, object InputViewModel = null, bool generate_vm_instance = false)
        {
            return _invokeDisplay(key, InputViewModel,generate_vm_instance, is_modeless: false); //This is modal
        }
        #endregion

        #region Show Methods
        public void show<ViewModelType>(ViewModelType InputViewModel = null, bool generate_vm_instance = false) where ViewModelType : class, IHaleyWindowVM
        {
            string _key = typeof(ViewModelType).FullName;
            show(_key, InputViewModel, generate_vm_instance);
        }
        public void show<ViewType>(bool generate_vm_instance = false) where ViewType : IHaleyWindow
        {
            string _key = typeof(ViewType).FullName;
            show(_key, null, generate_vm_instance);
        }
        public void show(Enum key, object InputViewModel = null, bool generate_vm_instance = false)
        {
            string _key = StringHelpers.getEnumAsKey(key);
            show(_key, InputViewModel, generate_vm_instance);
        }
        public void show(string key, object InputViewModel = null, bool generate_vm_instance = false)
        {
            _invokeDisplay(key, InputViewModel, generate_vm_instance,is_modeless: true); //This is modeless
        }

        #endregion

        #region Overridden Methods
        public override IHaleyWindow generateView(string key, object InputViewModel = null, bool generate_vm_instance = false)
        {
            try
            {
                var _kvp = _generateValuePair(key, generate_vm_instance);
                if (InputViewModel != null)
                {
                    _kvp.view.DataContext = InputViewModel; //Assinging actual viewmodel from user which becomes instance.
                }
                else
                {
                    _kvp.view.DataContext = _kvp.view_model; //Assinging generated viewmodel (also satisfying generate vm instance)
                }

                //Enable Haleyobserver so that when view closes, viewmodel event is triggered.
                HaleyObserver CustomOP = new HaleyObserver(_kvp.view, _kvp.view_model);
                CustomOP.subscribe();

                return _kvp.view;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private Methods
        private bool? _invokeDisplay(string key, object InputViewModel, bool generate_vm_instance, bool is_modeless)
        {
            bool? _result = null;

            //If Thread is not STA
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                Thread new_ui_thread = new Thread(() =>
                {
                    IHaleyWindow _hwindow = generateView(key, InputViewModel, generate_vm_instance);
                    _result = _displayWindow(_hwindow, is_modeless);
                });
                new_ui_thread.SetApartmentState(ApartmentState.STA);
                new_ui_thread.Start();
                new_ui_thread.Join();
            }
            else
            {
                IHaleyWindow _hwindow = generateView(key, InputViewModel);
                _result = _displayWindow(_hwindow, is_modeless);
            }

            return _result;
        }
        
        private bool? _displayWindow(IHaleyWindow _hwindow, bool is_modeless)
        {
            bool? _result = false;
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

        #endregion

      
    }


}
