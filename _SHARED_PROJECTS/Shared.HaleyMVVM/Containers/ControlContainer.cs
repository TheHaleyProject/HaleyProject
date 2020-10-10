using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using Haley.Events;
using Haley.Utils;
using System.Collections.Concurrent;

namespace Haley.MVVM.Containers
{
    public sealed class ControlContainer : IHaleyControlContainer
    {
        #region Initation
        private IHaleyDIContainer _di_instance = new DIContainer();
        private ConcurrentDictionary<string,(Type ViewModelType, Type ViewType,bool is_singleton)> main_mapping { get; set; } //Dictionary to store enumvalue and viewmodel type as key and usercontrol as value

        public ControlContainer(IHaleyDIContainer _injection_container = null)
        {
            main_mapping = new ConcurrentDictionary<string, (Type ViewModelType, Type ViewType, bool is_singleton)>();
            if (_injection_container != null)
            {
                _di_instance= _injection_container;
            }
        }

        #endregion

        #region Register Methods
        public void register<ViewModelType, ControlType>(ViewModelType InputViewModel = null, bool use_vm_as_key = false,bool is_singleton=true)
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
                
            register<ViewModelType, ControlType>(_key, InputViewModel,is_singleton);
        }

        public void register<ViewModelType, ControlType>(Enum @enum, ViewModelType InputViewModel = null, bool is_singleton = true)
           where ViewModelType : class, IHaleyControlVM
           where ControlType : IHaleyControl 
        {
            //Get the enum value and its type name to prepare a string
            string _key = StringHelpers.getEnumAsKey(@enum);
            register<ViewModelType, ControlType>(_key, InputViewModel,is_singleton);
        }

        public void register<ViewModelType, ControlType>(string key, ViewModelType InputViewModel = null, bool is_singleton = true)
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

                var _tuple = (typeof(ViewModelType), typeof(ControlType), is_singleton); 
                main_mapping.TryAdd(key, _tuple);

                if (is_singleton) //Only if singleton proceed further and register in the DI.
                {
                    var _status = _di_instance.checkIfRegistered(typeof(ViewModelType));
                    if (!_status.status)
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

        #region Private Methods
        private (IHaleyControlVM view_model, IHaleyControl view) _generateValuePair(string key, bool generate_vm_instance = false)
        {
            var _mapping_value = getMappingValue(key);

            //Generate a View
            IHaleyControl resultcontrol = (IHaleyControl)Activator.CreateInstance(_mapping_value.view_type); //Create the control.

            //Generate a ViewModel. First validate, how it is registered.
            //If this is false, then always generate instance
            if (!_mapping_value.is_singleton) generate_vm_instance = true; //Irrespective of what the user asks.
            IHaleyControlVM resultViewModel = _generateViewModel(key, _mapping_value.viewmodel_type, generate_vm_instance);

            return (resultViewModel, resultcontrol);
        }
        private IHaleyControlVM _generateViewModel(string key, Type viewmodeltype, bool generate_vm_instance = false)
        {
            IHaleyControlVM _result;
            _result = (IHaleyControlVM)_di_instance.Resolve(viewmodeltype, generate_vm_instance);
            return _result;
        }
        #endregion

        #region Control Retrieval Methods
        //Return a generic type which implements IHaleyControl 
        public IHaleyControl obtainControl<ViewModelType>(ViewModelType InputViewModel = null) where ViewModelType : class, IHaleyControlVM
        {
            string _key = typeof(ViewModelType).FullName;
            return obtainControl(_key, InputViewModel);
        }
        public IHaleyControl obtainControl<ViewModelType>(bool generate_vm_instance = false) where ViewModelType : class, IHaleyControlVM
        {
            string _key = typeof(ViewModelType).FullName;
            return obtainControl(_key, generate_vm_instance);
        }
        public IHaleyControl obtainControl(Enum @enum, object InputViewModel =null)
        {
            //Get the enum value and its type name to prepare a string
            string _key = StringHelpers.getEnumAsKey(@enum);
            return obtainControl(_key,InputViewModel);
        }
        public IHaleyControl obtainControl(Enum @enum, bool generate_vm_instance = false)
        {
            //Get the enum value and its type name to prepare a string
            string _key = StringHelpers.getEnumAsKey(@enum);
            return obtainControl(_key, generate_vm_instance);
        }
        public IHaleyControl obtainControl(string key, bool generate_vm_instance = false)
        {
            try
            {
                var _kvp = _generateValuePair(key, generate_vm_instance);
                _kvp.view.DataContext = _kvp.view_model; //Assinging actual viewmodel
                return _kvp.view;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IHaleyControl obtainControl(string key, object InputViewModel = null)
        {
            try
            {
                var _kvp = _generateValuePair(key);
                if (InputViewModel != null)
                {
                    _kvp.view.DataContext = InputViewModel; //Assinging actual viewmodel
                }
                else
                {
                    _kvp.view.DataContext = _kvp.view_model; //Assinging generated viewmodel
                }
               
                return _kvp.view;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region VM Retrieval Methods
        public  (Type viewmodel_type, Type view_type, bool is_singleton) getMappingValue(Enum @enum)
        {
            //Get the enum value and its type name to prepare a string
            string _key = StringHelpers.getEnumAsKey(@enum);
            return getMappingValue(_key);
        }
        public (Type viewmodel_type, Type view_type,bool is_singleton) getMappingValue(string key)
        {
            if (main_mapping.Count == 0 || ! main_mapping.ContainsKey(key))
            {
                throw new ArgumentException($"Key {key} is not registered to any controls. Please check.");
            }


            (Type _viewmodel_type, Type _view_type, bool _is_singleton) _registered_tuple = (null, null, true);
            main_mapping.TryGetValue(key, out _registered_tuple);

            if (_registered_tuple._viewmodel_type == null || _registered_tuple._view_type == null)
            {
                StringBuilder sbuilder = new StringBuilder();
                sbuilder.AppendLine($@"The key {key} has null values associated with it.");
                sbuilder.AppendLine($@"ViewModel Type : {_registered_tuple._viewmodel_type}");
                sbuilder.AppendLine($@"View Type : {_registered_tuple._view_type}");
                throw new ArgumentException(sbuilder.ToString());
            }

            return _registered_tuple;
        }
        public IHaleyControlVM generateViewModel(Enum @enum, bool generate_vm_instance)
        {
            //Get the enum value and its type name to prepare a string
            string _key = StringHelpers.getEnumAsKey(@enum);
            IHaleyControlVM _result = generateViewModel(_key, generate_vm_instance);
            return _result;
        }
        public IHaleyControlVM generateViewModel(string key, bool generate_vm_instance = false)
        {
            try
            {
                IHaleyControlVM _result;
                var _mappingValue = getMappingValue(key);
                _result = (IHaleyControlVM)_di_instance.Resolve(_mappingValue.viewmodel_type, generate_vm_instance);
                return _result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

    }
}

