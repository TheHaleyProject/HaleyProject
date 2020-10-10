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
    public abstract class UIContainerBases<BaseViewModelType,BaseViewType> : IHaleyUIContainer<BaseViewModelType, BaseViewType>
    {
        #region Initation
        protected IHaleyDIContainer _di_instance = new DIContainer();
        protected ConcurrentDictionary<string,(Type VMtype, Type ViewType,bool is_singleton)> main_mapping { get; set; } //Dictionary to store enumvalue and viewmodel type as key and usercontrol as value

        public UIContainerBases(IHaleyDIContainer _injection_container = null)
        {
            main_mapping = new ConcurrentDictionary<string, (Type VMtype, Type ViewType, bool is_singleton)>();
            if (_injection_container != null)
            {
                _di_instance= _injection_container;
            }
        }

        #endregion

        #region Register Methods
        public virtual string register<viewmodelType, viewType>(viewmodelType InputViewModel = null, bool use_vm_as_key = true, bool is_singleton = true)
            where viewmodelType : class, BaseViewModelType
            where viewType : BaseViewType
        {
            string _key = null;
            if (use_vm_as_key)
            {
                _key = typeof(viewmodelType).FullName;
            }
            else
            {
                _key = typeof(viewType).FullName;
            }

           return register<viewmodelType, viewType>(_key, InputViewModel, is_singleton);
        }

        public virtual string register<viewmodelType, viewType>(Enum @enum, viewmodelType InputViewModel = null, bool is_singleton = true)
           where viewmodelType : class, BaseViewModelType
           where viewType : BaseViewType
        {
            //Get the enum value and its type name to prepare a string
            string _key = StringHelpers.getEnumAsKey(@enum);
           return register<viewmodelType, viewType>(_key, InputViewModel, is_singleton);
        }

        public virtual string register<viewmodelType, viewType>(string key, viewmodelType InputViewModel = null, bool is_singleton = true)
            where viewmodelType : class, BaseViewModelType
            where viewType : BaseViewType
        {
            try
            {
                //First add the internal main mappings.
                if (main_mapping.ContainsKey(key) == true)
                {
                    throw new ArgumentException($@"Key : {key} is already registered to - VM : {main_mapping[key].VMtype.GetType()} and View : {main_mapping[key].ViewType.GetType()}");
                }

                var _tuple = (typeof(viewmodelType), typeof(viewType), is_singleton);
                main_mapping.TryAdd(key, _tuple);

                if (is_singleton) //Only if singleton proceed further and register in the DI.
                {
                    var _status = _di_instance.checkIfRegistered(typeof(viewmodelType));
                    if (!_status.status)
                    {
                        _di_instance.Register<viewmodelType>(InputViewModel);
                    }
                }
                return key;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods
        protected (BaseViewModelType view_model, BaseViewType view) _generateValuePair(string key, bool generate_vm_instance = false)
        {
            var _mapping_value = getMappingValue(key);

            //Generate a View
            BaseViewType resultcontrol = (BaseViewType)Activator.CreateInstance(_mapping_value.view_type); //Create the control.

            //Generate a ViewModel. First validate, how it is registered.
            //If this is false, then always generate instance
            if (!_mapping_value.is_singleton) generate_vm_instance = true; //Irrespective of what the user asks.
            BaseViewModelType resultViewModel = _generateViewModel(key, _mapping_value.viewmodel_type, generate_vm_instance);

            return (resultViewModel, resultcontrol);
        }
        protected BaseViewModelType _generateViewModel(string key, Type viewmodelType, bool generate_vm_instance = false)
        {
            BaseViewModelType _result;
            _result = (BaseViewModelType)_di_instance.Resolve(viewmodelType, generate_vm_instance);
            return _result;
        }
        #endregion

        #region View Retrieval Methods
        //Return a generic type which implements BaseViewType 
        public BaseViewType generateView<viewmodelType>(viewmodelType InputViewModel = null, bool generate_vm_instance = false) where viewmodelType : class, BaseViewModelType
        {
            string _key = typeof(viewmodelType).FullName;
            return generateView(_key, InputViewModel, generate_vm_instance);
        }
        public BaseViewType generateView(Enum @enum, object InputViewModel = null, bool generate_vm_instance = false)
        {
            //Get the enum value and its type name to prepare a string
            string _key = StringHelpers.getEnumAsKey(@enum);
            return generateView(_key, InputViewModel,generate_vm_instance);
        }
        public abstract BaseViewType generateView(string key, object InputViewModel = null, bool generate_vm_instance = false);
        
        #endregion

        #region VM Retrieval Methods
        public (Type viewmodel_type, Type view_type, bool is_singleton) getMappingValue(Enum @enum)
        {
            //Get the enum value and its type name to prepare a string
            string _key = StringHelpers.getEnumAsKey(@enum);
            return getMappingValue(_key);
        }
        public (Type viewmodel_type, Type view_type, bool is_singleton) getMappingValue(string key)
        {
            if (main_mapping.Count == 0 || !main_mapping.ContainsKey(key))
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
        public BaseViewModelType generateViewModel(Enum @enum, bool generate_vm_instance)
        {
            //Get the enum value and its type name to prepare a string
            string _key = StringHelpers.getEnumAsKey(@enum);
            BaseViewModelType _result = generateViewModel(_key, generate_vm_instance);
            return _result;
        }
        public BaseViewModelType generateViewModel(string key, bool generate_vm_instance = false) //If required we can even return the actural viewmodel concrete type as well.
        {
            try
            {
                BaseViewModelType _result;
                var _mappingValue = getMappingValue(key);
                _result = (BaseViewModelType)_di_instance.Resolve(_mappingValue.viewmodel_type, generate_vm_instance);
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}

