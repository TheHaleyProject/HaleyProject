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
using Haley.Enums;

namespace Haley.MVVM.Containers
{
    public sealed class WindowContainer : UIContainerBase<IHaleyWindowVM,IHaleyWindow>, IHaleyWindowContainer<IHaleyWindowVM, IHaleyWindow>  //Implementation of the DialogService Interface.
    {
        public WindowContainer(IHaleyDIContainer _injection_container) : base(_injection_container) { }

        #region ShowDialog Methods
        public bool? showDialog(Enum key, object InputViewModel = null, GenerateNewInstance instance_level = GenerateNewInstance.None)
        {
            string _key = StringHelpers.getEnumAsKey(key);
            return showDialog(_key, InputViewModel, instance_level);
        }
        public bool? showDialog<ViewModelType>(ViewModelType InputViewModel = null, GenerateNewInstance instance_level = GenerateNewInstance.None) where ViewModelType : class, IHaleyWindowVM
        {
            string _key = typeof(ViewModelType).ToString();
            return showDialog(_key, InputViewModel, instance_level);
        }
        public bool? showDialog<ViewType>(GenerateNewInstance instance_level = GenerateNewInstance.None) where ViewType : IHaleyWindow
        {
            string _key = typeof(ViewType).ToString();
            return showDialog(_key, null, instance_level);
        }
        public bool? showDialog(string key, object InputViewModel = null, GenerateNewInstance instance_level = GenerateNewInstance.None)
        {
            return _invokeDisplay(key, InputViewModel,instance_level, is_modeless: false); //This is modal
        }
        #endregion

        #region Show Methods
        public void show<ViewModelType>(ViewModelType InputViewModel = null, GenerateNewInstance instance_level = GenerateNewInstance.None) where ViewModelType : class, IHaleyWindowVM
        {
            string _key = typeof(ViewModelType).FullName;
            show(_key, InputViewModel, instance_level);
        }
        public void show<ViewType>(GenerateNewInstance instance_level = GenerateNewInstance.None) where ViewType : IHaleyWindow
        {
            string _key = typeof(ViewType).FullName;
            show(_key, null, instance_level);
        }
        public void show(Enum key, object InputViewModel = null, GenerateNewInstance instance_level = GenerateNewInstance.None)
        {
            string _key = StringHelpers.getEnumAsKey(key);
            show(_key, InputViewModel, instance_level);
        }
        public void show(string key, object InputViewModel = null, GenerateNewInstance instance_level = GenerateNewInstance.None)
        {
            _invokeDisplay(key, InputViewModel, instance_level,is_modeless: true); //This is modeless
        }

        #endregion

        #region Overridden Methods
        public override IHaleyWindow generateView(string key, object InputViewModel = null, GenerateNewInstance instance_level = GenerateNewInstance.None)
        {
            try
            {
                var _kvp = _generateValuePair(key, instance_level);
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
        private bool? _invokeDisplay(string key, object InputViewModel, GenerateNewInstance instance_level, bool is_modeless)
        {
            bool? _result = null;

            //If Thread is not STA
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                Thread new_ui_thread = new Thread(() =>
                {
                    IHaleyWindow _hwindow = generateView(key, InputViewModel, instance_level);
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
