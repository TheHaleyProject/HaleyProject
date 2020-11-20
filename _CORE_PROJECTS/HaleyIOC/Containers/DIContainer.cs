using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using Haley.Enums;
using System.Runtime.InteropServices;
using Haley.Models;
using Haley.Abstractions;

namespace Haley.Containers
{
    public sealed class DIContainer : IHaleyDIContainer
    {
        #region ATTRIBUTES
        private readonly ConcurrentDictionary<string, (Type contract_type, Type concrete_type, object concrete_instance, bool is_singleton)> _mappings = new ConcurrentDictionary<string, (Type contract_type, Type concrete_type, object concrete_instance, bool is_singleton)>();
        #endregion

        #region Properties
        public bool ignore_if_registered { get; set; }
        public bool overwrite_if_registered { get; set; }
        #endregion

        #region PRIVATE METHODS

        #region Helpers
        private string _getKey(Type contract_type, string priority_key)
        {
            //Key = $contractType##$priority_key
            string _key = "";

            if (contract_type != null)
            {
                _key = $@"{contract_type.ToString()}";
            }

            if (!string.IsNullOrEmpty(priority_key) || !string.IsNullOrWhiteSpace(priority_key))
            {
                if (!string.IsNullOrEmpty(_key)) _key += "##";
                _key += priority_key.ToLower();
            }
            return _key;
        }
        private TransientCreationLevel _convertToTransientLevel(MappingLevel mapping_level)
        {
            TransientCreationLevel _transient_level = TransientCreationLevel.None;
            switch (mapping_level)
            {
                case MappingLevel.Current:
                    _transient_level = TransientCreationLevel.Current;
                    break;
                case MappingLevel.CurrentWithProperties:
                    _transient_level = TransientCreationLevel.CurrentWithProperties;
                    break;
                case MappingLevel.CascadeAll:
                    _transient_level = TransientCreationLevel.CascadeAll;
                    break;
            }
            return _transient_level;
        }
        private bool _validateExistence(Type contract_type, Type concrete_type, string priority_key)
        {
            var _status = checkIfRegistered(contract_type, priority_key);
            //If registered and also ignore
            if (_status.status)
            {
                if (!ignore_if_registered)
                {
                    //Throw the error, only if you should not ignore the registered status.
                    if (_status.registered_type != concrete_type)
                    {
                        throw new ArgumentException(_status.message);
                    }
                }
            }
            return _status.status; //Returns if registered.
        }
        private void _validateConcreteType(Type concrete_type)
        {
            if (concrete_type.IsAbstract || concrete_type.IsEnum || concrete_type.IsInterface)
            {
                throw new ArgumentException($@"Concrete type cannot be an abstract class or enum or interface. Unable to register {concrete_type}");
            }
        }
        private (bool exists, Type contract_type, Type concrete_type, object concrete_instance, bool is_singleton) _getMapping(string priority_key, Type contract_type)
        {

            //Try to see if we can get value using the priority/contract_type mix.
            bool _exists = false;

            (Type _contract_type, Type _concrete_type, object _concrete_instance, bool is_singleton) _registered_tuple = (null, null, null, true);
            string _key = _getKey(contract_type, priority_key);

            //Give priority to contract_type and PriorityKey
            if (_mappings.ContainsKey(_key))
            {
                _exists = true;
                _mappings.TryGetValue(_key, out _registered_tuple);
            }
            if (!_exists)
            {
                //Reassigning the key with contract type name.
                _key = contract_type?.ToString();
                if (_mappings.ContainsKey(_key))
                {
                    _exists = true;
                    _mappings.TryGetValue(_key, out _registered_tuple);
                }
            }

            return (_exists, _registered_tuple._contract_type, _registered_tuple._concrete_type, _registered_tuple._concrete_instance, _registered_tuple.is_singleton);
        }
        #endregion

        #region Register
        private bool _register(string priority_key, Type contract_type, Type concrete_type, object concrete_instance, IMappingProvider dependencyProvider, MappingLevel mapping_level, bool is_singleton)
        {
            //Validate if the contract type is alredy registered. If so, against correct concrete type.
            bool _exists = _validateExistence(contract_type, concrete_type,priority_key);

            //If it already exists and we should not over write, then do not proceed.
            if (_exists && (!overwrite_if_registered)) return false;

            //Validate if the concrete type can be registered
            _validateConcreteType(concrete_type);

            //Generate instance only if the provided value is null and also singleton. Only if it is singleton, we create an instance and store. Else we store only the concrete type and save instance as null.
            if (concrete_instance == null && is_singleton)
            {
                concrete_instance = _createInstance(concrete_type, dependencyProvider, mapping_level, TransientCreationLevel.None, ResolveMode.AsRegistered,priority_key); //Create instance resolving all dependencies
            }

            //Get the key to register.
            string _key = _getKey(contract_type, priority_key);

            //Generate new tuple
            var _new_tuple = (contract_type, concrete_type, concrete_instance, is_singleton);

            //We have already validate if overwrite is required or not. If we reach this point, then overwrite is required.
            if (_exists)
            {
                //Update the existing value 
                (Type contract_type, Type concrete_type, object concrete_instance, bool is_singleton) _current_tuple;
                _mappings.TryGetValue(_key, out _current_tuple);
                _mappings.TryUpdate(_key, _new_tuple, _current_tuple); //Remember to assign the instance
            }
            else
            {
                _mappings.TryAdd(_key, _new_tuple);
            }
            return true;
        }
        #endregion

        #region Creation Methods
        private object _createInstance(Type concrete_type, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level,ResolveMode mode,string priority_key)
        {
            //If transient creation is current level only, then further dependencies should not generate new instance.
            if (transient_level == TransientCreationLevel.Current) transient_level = TransientCreationLevel.None;

            object concrete_instance = null;
            _validateConcreteType(concrete_type);
            ConstructorInfo constructor = _getConstructor(concrete_type);
            _resolveConstructorParameters(ref constructor, concrete_type, mapping_provider, mapping_level, transient_level,mode, ref concrete_instance);
            _resolveProperties(concrete_type, mapping_provider, mapping_level, transient_level,mode, ref concrete_instance);
            return concrete_instance;
        }
        private ConstructorInfo _getConstructor(Type concrete_type)
        {
            ConstructorInfo constructor = null;
            var constructors = concrete_type.GetConstructors();

            if (constructors.Length == 0)
            {
                throw new ArgumentException($@"No constructors found. Unable to create an instance for {concrete_type.Name}");
            }

            if (constructors.Length > 1)
            {
                //If we have more items, get the first constructor that has [HaleyInject]
                foreach (var _constructor in constructors)
                {
                    var attr = _constructor.GetCustomAttribute(typeof(HaleyInjectAttribute));
                    if (attr != null)
                    {
                        constructor = _constructor;
                        break;
                    }
                }
            }

            //Taking the first constructor.
            if (constructor == null)
            {
                //Get the first constructor where ignore attribute is not present.
                constructor = constructors.FirstOrDefault(p => p.GetCustomAttribute(typeof(HaleyIgnoreAttribute)) == null);
                if (constructor == null) throw new ArgumentException($@"No valid constructors found. Unable to create an instance for {concrete_type.Name}");
            }
            return constructor;
        }
        private void _resolveConstructorParameters(ref ConstructorInfo constructor, Type concrete_type, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level,ResolveMode mode, ref object concrete_instance)
        {
            //If creation is current with properties, then constructor and props should generate new instance. Rest should be resolved.
            if (transient_level == TransientCreationLevel.CurrentWithProperties) transient_level = TransientCreationLevel.Current;
            if (mapping_level == MappingLevel.CurrentWithProperties) mapping_level = MappingLevel.Current;

            //Resolve the param arugments for the constructor.
            ParameterInfo[] constructor_params = constructor.GetParameters();

            //If parameter less construction, return a new creation.
            if (constructor_params.Length == 0)
            {
                concrete_instance = Activator.CreateInstance(concrete_type);
            }
            else
            {
                List<object> parameters = new List<object>(constructor_params.Length);
                foreach (ParameterInfo pinfo in constructor_params)
                {
                    Type _paramtype = pinfo.ParameterType;

                    parameters.Add(_mainResolve(null, pinfo.Name, pinfo.ParameterType, concrete_type, mapping_provider, mapping_level, transient_level, mode, InjectionTarget.Constructor));
                }
                concrete_instance = constructor.Invoke(parameters.ToArray());
            }

        }
        private void _resolveProperties(Type concrete_type, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level,ResolveMode mode, ref object concrete_instance)
        {
            //If creation is current with properties, then constructor and props should generate new instance. Rest should be resolved.
            if (transient_level == TransientCreationLevel.CurrentWithProperties) transient_level = TransientCreationLevel.Current;
            if (mapping_level == MappingLevel.CurrentWithProperties) mapping_level = MappingLevel.Current;

            //Resolve only properties that are of type Haley inject and also ignore if it has haleyignore
            var _props = concrete_type.GetProperties().Where(
                p => Attribute.IsDefined(p, typeof(HaleyInjectAttribute)));

            if (_props.Count() > 0)
            {
                foreach (PropertyInfo pinfo in _props)
                {
                    try
                    {
                        Type _prop_type = pinfo.PropertyType;

                        var resolved_value = _mainResolve(null, pinfo.Name, _prop_type, concrete_type, mapping_provider, mapping_level, transient_level, mode,InjectionTarget.Property);
                        if (resolved_value != null) pinfo.SetValue(concrete_instance, resolved_value);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }
        #endregion

        #region Resolution Methods
        private object _mainResolve(string priority_key, string contract_name, Type contract_type, Type contract_parent, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level, ResolveMode resolve_mode, InjectionTarget injection = InjectionTarget.All)
        {
            object concrete_instance = null;

            //If dependency provider is not null, then try to resolve using external mapping.
            _mappingProviderResolve(mapping_provider, mapping_level, contract_name, contract_type, contract_parent, injection, out concrete_instance);

            //Reset Mapping Level.
            if (mapping_level == MappingLevel.Current) mapping_level = MappingLevel.None;

            if (concrete_instance == null)
            {
                //If request is for creation of a transient object, then do not try to resolve using internal methods. Just try fetch the concrete type and create instance.
                _transientResolve(priority_key, contract_type, mapping_provider, mapping_level, resolve_mode, transient_level, out concrete_instance);
            }

            //If still object is null, try resolving using internal mapping.
            if (concrete_instance == null)
            {
                if (contract_type == typeof(string) || contract_type.IsValueType)
                {
                    throw new ArgumentException($@"Value type dependency error. The {contract_parent ?? contract_type} contains a value dependency {contract_name ?? ""}. Try adding a mapping provider for injecting value types.");
                }
                _asRegisteredResolve(priority_key, contract_type, mapping_provider, mapping_level, transient_level, resolve_mode, out concrete_instance);
            }

            return concrete_instance;
        }
        private object _mappingResolve(string priority_key, string contract_name, Type contract_type, Type contract_parent, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level, ResolveMode resolve_mode, InjectionTarget injection = InjectionTarget.All)
        {
            //For Initial contract_type make a create instance. This will ensure that the initial contract_type's params and props gets values from mapping provider (using _resolve).
            var _mapping_values = _getMapping(priority_key, contract_type);
            if (_mapping_values.exists)
            {
                return _createInstance(_mapping_values.concrete_type??contract_type, mapping_provider, mapping_level, transient_level, resolve_mode,priority_key);
            }
            else
            {
                return _createInstance(contract_type, mapping_provider, mapping_level, transient_level, resolve_mode,priority_key);
            }
        }
        private void _mappingProviderResolve(IMappingProvider mapping_provider, MappingLevel mapping_level, string contract_name, Type contract_type, Type contract_parent, InjectionTarget injection, out object concrete_instance)
        {
            //Begin with null output.
            concrete_instance = null;

            //Mapping level defines until which stage or level , the mapping should be applied. If mapping provider is null or mapping level is none, don't proceed. 
            if (mapping_provider == null || mapping_level == MappingLevel.None) return;

            if (contract_type == null) { throw new ArgumentNullException(nameof(contract_type)); }

            var _dip_values = mapping_provider.Resolve(contract_type, contract_name, contract_parent);

            //if external mapping resolves to a value, ensure that this injection is for suitable target or it should be for all
            if (_dip_values.concrete_instance != null && (injection == _dip_values.injection || injection == InjectionTarget.All))
            {
                concrete_instance = _dip_values.concrete_instance;
            }
        }
        private void _asRegisteredResolve(string priority_key, Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level,ResolveMode mode, out object concrete_instance)
        {
            concrete_instance = null;

            if (contract_type == null) throw new ArgumentNullException(nameof(contract_type));

            var _mapping_value = _getMapping(priority_key, contract_type);

            if (concrete_instance == null && _mapping_value.exists)
            {
                //If resolve mode is default, we process, as per the registration.
                if (_mapping_value.is_singleton) //If singleton, return concrete instance.
                {
                    concrete_instance = _mapping_value.concrete_instance;
                }
                else //Registered as transient.
                {
                    concrete_instance = _createInstance(_mapping_value.concrete_type, mapping_provider, mapping_level, transient_level, mode,priority_key);
                }
            }
        }
        private void _transientResolve(string priority_key, Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level,ResolveMode resolve_mode, TransientCreationLevel transient_level, out object concrete_instance)
        {
            concrete_instance = null;
            //If resolve mode is default, it could be transient or singleton. So, in the case of default, proceed until internal mapping resolve and then validate.
            if (resolve_mode == ResolveMode.Transient && transient_level != TransientCreationLevel.None)
            {
                var _mapping_value = _getMapping(priority_key, contract_type);
                if (_mapping_value.exists) //In case, contract is of Interface which already has a registered concrete type.
                {
                    concrete_instance = _createInstance(_mapping_value.concrete_type, mapping_provider, mapping_level, transient_level,resolve_mode,priority_key);
                }
                else
                {
                    concrete_instance = _createInstance(contract_type, mapping_provider, mapping_level, transient_level, resolve_mode,priority_key);
                }
            }
        }
        
        #endregion

        #endregion

        #region PUBLIC METHODS

        #region Validations
        public (bool status, Type registered_type, string message, bool is_singleton) checkIfRegistered(string key)
        {
            (Type contract_type, Type concrete_type, object concrete_instance, bool is_singleton) _registered_tuple = (null, null, null, true);

            string _message = null;
            bool _is_registered = false;

            if (_mappings.ContainsKey(key))
            {
                _is_registered = true;
                _mappings.TryGetValue(key, out _registered_tuple);
                _message = $@"The key : {key} is already registered against the type {_registered_tuple.concrete_type}.";
            }

            return (_is_registered, _registered_tuple.concrete_type, _message, _registered_tuple.is_singleton);
        }
        public (bool status, Type registered_type, string message, bool is_singleton) checkIfRegistered(Type contract_type, string priority_key)
        {
            return checkIfRegistered(_getKey(contract_type, priority_key));
        }
        public (bool status, Type registered_type, string message, bool is_singleton) checkIfRegistered<Tcontract>(string priority_key)
        {
            return checkIfRegistered(typeof(Tcontract), priority_key);
        }

        #endregion

        #region Register Methods
        public void Register<TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            RegisterWithKey<TConcrete>(null, mode);
        }
        public void Register<TConcrete>(TConcrete instance) where TConcrete : class
        {
            RegisterWithKey(null, instance);
        }
        public void Register<TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class
        {
            RegisterWithKey<TConcrete>(null, dependencyProvider, mapping_level);
        }
        public void Register<TContract, TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            RegisterWithKey<TContract, TConcrete>(null, mode);
        }
        public void Register<TContract, TConcrete>(TConcrete instance) where TConcrete : class, TContract
        {
            RegisterWithKey<TContract, TConcrete>(null, instance);
        }
        public void Register<TContract, TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class, TContract
        {
            RegisterWithKey<TContract, TConcrete>(null, dependencyProvider, mapping_level);
        }

        #endregion

        #region TryRegister Methods
        public bool TryRegister<TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            try
            {
                return RegisterWithKey<TConcrete>(null, mode);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TryRegister<TConcrete>(TConcrete instance) where TConcrete : class
        {
            try
            {
                return RegisterWithKey(null, instance);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TryRegister<TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class
        {
            try
            {
                return RegisterWithKey<TConcrete>(null, dependencyProvider, mapping_level);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TryRegister<TContract, TConcrete>(TConcrete instance) where TConcrete : class, TContract
        {
            try
            {
                return RegisterWithKey<TContract, TConcrete>(null, instance);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TryRegister<TContract, TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            try
            {
                return RegisterWithKey<TContract, TConcrete>(null, mode);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TryRegister<TContract, TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class, TContract
        {
            try
            {
                return RegisterWithKey<TContract, TConcrete>(null, dependencyProvider, mapping_level);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region RegisterWithKey Methods
        public bool RegisterWithKey<TConcrete>(string priority_key, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            return _register(priority_key, typeof(TConcrete), typeof(TConcrete), null, null, MappingLevel.None, mode == RegisterMode.Singleton);
        }
        public bool RegisterWithKey<TConcrete>(string priority_key, TConcrete instance) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            //If we have an instance, then obviously it is of singleton registration type.
            return _register(priority_key, typeof(TConcrete), typeof(TConcrete), instance, null, MappingLevel.None, true);
        }
        public bool RegisterWithKey<TConcrete>(string priority_key, IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            return _register(priority_key, typeof(TConcrete), typeof(TConcrete), null, dependencyProvider, mapping_level, true);
        }
        public bool RegisterWithKey<TContract, TConcrete>(string priority_key, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            return _register(priority_key, typeof(TContract), typeof(TConcrete), null, null, MappingLevel.None, mode == RegisterMode.Singleton);
        }
        public bool RegisterWithKey<TContract, TConcrete>(string priority_key, TConcrete instance) where TConcrete : class, TContract
        {
            return _register(priority_key, typeof(TContract), typeof(TConcrete), instance, null, MappingLevel.None, true);
        }
        public bool RegisterWithKey<TContract, TConcrete>(string priority_key, IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class, TContract
        {
            return _register(priority_key, typeof(TContract), typeof(TConcrete), null, dependencyProvider, mapping_level, true);
        }
        #endregion

        #region Resolve Methods
        public T Resolve<T>(ResolveMode mode = ResolveMode.AsRegistered)
        {
            var _obj = Resolve(typeof(T), mode);
            return (T)_obj;
        }
        public object Resolve(Type contract_type, ResolveMode mode = ResolveMode.AsRegistered)
        {
            return _mainResolve(null, null, contract_type, null, null, MappingLevel.None, TransientCreationLevel.Current, mode);
        }
        public object Resolve(string priority_key, Type contract_type, ResolveMode mode = ResolveMode.AsRegistered)
        {
            return _mainResolve(priority_key, null, contract_type, null, null, MappingLevel.None, TransientCreationLevel.Current, mode);
        }

        #endregion

        #region TryResolve Methods
        public bool TryResolve(Type contract_type, out object concrete_instance, ResolveMode mode = ResolveMode.AsRegistered)
        {
            try
            {
                concrete_instance = Resolve(contract_type, mode);
                return true;
            }
            catch (Exception)
            {
                concrete_instance = null;
                return false;
            }
        }
        public bool TryResolve(string priority_key, Type contract_type, out object concrete_instance, ResolveMode mode = ResolveMode.AsRegistered)
        {
            try
            {
                concrete_instance = Resolve(priority_key, contract_type, mode);
                return true;
            }
            catch (Exception)
            {
                concrete_instance = null;
                return false;
            }
        }
        #endregion

        #region ResolveTransient Methods

        public T ResolveTransient<T>(TransientCreationLevel transient_level)
        {
            var _obj = ResolveTransient(typeof(T), transient_level);
            return (T)_obj;
        }
        public object ResolveTransient(Type contract_type, TransientCreationLevel transient_level)
        {
            return _mainResolve(null, null, contract_type, null, null, MappingLevel.Current, transient_level, ResolveMode.Transient);
        }
        public object ResolveTransient(string priority_key, Type contract_type, TransientCreationLevel transient_level)
        {
            return _mainResolve(priority_key, null, contract_type, null, null, MappingLevel.Current, transient_level, ResolveMode.Transient);
        }

        public T ResolveTransient<T>(IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithProperties)
        {
            var _obj = ResolveTransient(typeof(T), mapping_provider, mapping_level);
            return (T)_obj;
        }
        public object ResolveTransient(Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithProperties)
        {
            return _mappingResolve(null, null, contract_type, null, mapping_provider, mapping_level, _convertToTransientLevel(mapping_level), ResolveMode.Transient);
        }
        public object ResolveTransient(string priority_key, Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithProperties)
        {
            return _mappingResolve(priority_key, null, contract_type, null, mapping_provider, mapping_level, _convertToTransientLevel(mapping_level), ResolveMode.Transient);
        }
        #endregion
        #endregion

        public DIContainer() 
        {
            overwrite_if_registered = false;
            ignore_if_registered = false; 
        }
    }
}
