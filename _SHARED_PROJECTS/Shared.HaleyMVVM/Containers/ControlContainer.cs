using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using Haley.Events;

namespace Haley.MVVM.Containers
{
    public sealed class ControlContainer : IHaleyControlContainer
    {
        #region Initation
        private IHaleyDIContainer _di_instance = new DIContainer();
        private Dictionary<string,(Type ViewModelType, Type ViewType)> main_mapping { get; set; } //Dictionary to store enumvalue and viewmodel type as key and usercontrol as value

        public ControlContainer(IHaleyDIContainer _injection_container = null)
        {
            main_mapping = new Dictionary<string, (Type ViewModelType, Type ViewType)>();
            if (_injection_container != null)
            {
                _di_instance= _injection_container;
            }
        }

        #endregion

        #region Helper Methods
        private string _getEnumKey(Enum @enum)
        {
            try
            {
                string enum_type_name = @enum.GetType().Name;
                string enum_value_name = @enum.ToString();
                string enum_key = enum_type_name + "." + enum_value_name; //Concatenated value for storing as key
                return enum_key;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Register Methods
        public void register<ViewModelType, ControlType>(ViewModelType InputViewModel = null, bool use_vm_as_key = false)
            where ViewModelType :class, IHaleyControlVM
            where ControlType : IHaleyControl 
        {
            //Get the full name of the CONTROL type (Not the view model type). This is done , so that it could help in locating viewmodel.
            string _key = null;
            if (use_vm_as_key)
            {
                _key = typeof(ViewModelType).FullName;
            }
            else
            {
                _key=  typeof(ControlType).FullName;
            }
                
            register<ViewModelType, ControlType>(_key, InputViewModel);
        }

        public void register<ViewModelType, ControlType>(Enum @enum, ViewModelType InputViewModel = null)
           where ViewModelType : class, IHaleyControlVM
           where ControlType : IHaleyControl 
        {
            //Get the enum value and its type name to prepare a string
            string _key = _getEnumKey(@enum);
            register<ViewModelType, ControlType>(_key, InputViewModel);
        }

        public void register<ViewModelType, ControlType>(string key, ViewModelType InputViewModel = null)
            where ViewModelType : class, IHaleyControlVM
            where ControlType : IHaleyControl
        {
            try
            {
                //First add the internal main mappings.
                if (main_mapping.ContainsKey(key) == true)
                {
                    throw new ArgumentException($@"Key : {key} is already registered to - VM : {main_mapping[key].ViewModelType.GetType()} and View : {main_mapping[key].ViewType.GetType()}");
                }
                var _tuple = (typeof(ViewModelType), typeof(ControlType));
                main_mapping.Add(key, _tuple);

                //Now add the viewmodel to the DI
                var _status = _di_instance.checkIfRegistered(typeof(ViewModelType));
                if (!_status.status)
                {
                    if (InputViewModel == null)
                    {
                        _di_instance.Register<ViewModelType>(); //Resolve and add to concrete container
                    }
                    else
                    {
                        _di_instance.Register<ViewModelType>(InputViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Retrieval Methods
        //Return a generic type which implements IHaleyControl 
        public IHaleyControl obtainControl<ViewModelType>(ViewModelType InputViewModel = null, bool create_new_vm = false) where ViewModelType : class, IHaleyControlVM
        {
            string _key = typeof(ViewModelType).FullName;
            return obtainControl(_key, InputViewModel, create_new_vm);
        }
        public IHaleyControl  obtainControl(Enum @enum, object InputViewModel =null, bool create_new_vm = false)
        {
            //Get the enum value and its type name to prepare a string
            string _key = _getEnumKey(@enum);
            return obtainControl(_key,InputViewModel,create_new_vm);
        }

        public IHaleyControl obtainControl(string key, object InputViewModel = null, bool create_new_vm = false) 
        {
            try
            {
                //Check if this key is already used or not.
                if (main_mapping.Count == 0)
                {
                    throw new ArgumentException("No viewmodel/views are registered for the control container.");
                }

                if (main_mapping.ContainsKey(key) == false)
                {
                    throw new ArgumentException($"Key {key} is not registered to any controls. Please check.");
                }

                var result_tuple = main_mapping[key];
                Type resultViewModelType = result_tuple.ViewModelType;
                Type resultControlType = result_tuple.ViewType; // Get the type of control for the provided input types
                IHaleyControl resultcontrol = (IHaleyControl)Activator.CreateInstance(resultControlType);

                if (InputViewModel == null)
                {
                    InputViewModel = _di_instance.Resolve(resultViewModelType, create_new_vm);
                }
            
                resultcontrol.DataContext = InputViewModel; //Assinging actual viewmodel
                return resultcontrol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object obtainVM(Enum key, bool create_new_vm = false)
        {
            //Get the enum value and its type name to prepare a string
            string _key = _getEnumKey(key);
            return obtainVM(_key);
        }

        public object obtainVM(string key, bool create_new_vm = false)
        {
            try
            {
                if (main_mapping.Count == 0)
                {
                    throw new ArgumentException("No viewmodel/views are registered for the control container.");
                }

                if (!main_mapping.ContainsKey(key))
                {
                    throw new ArgumentException($"Key {key} for the type is not registered to any controls. Please check.");
                }

                Type resultViewModelType =  main_mapping[key].ViewModelType;
                var resultVM = _di_instance.Resolve(resultViewModelType,create_new_vm);
                return resultVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}

