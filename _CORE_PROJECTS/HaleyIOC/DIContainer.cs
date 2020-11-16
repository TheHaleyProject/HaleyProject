using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using Haley.Enums;
using System.Runtime.InteropServices;
using Haley.Models;
using Haley.Abstractions;


namespace Haley.IOC
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

        #region Validations
        public (bool status, Type registered_type, string message, bool is_singleton) checkIfRegistered(string key)
        {
            (Type contract_type, Type concrete_type, object concrete_instance,bool is_singleton) _registered_tuple = (null, null, null,true);

            string _message = null;
            bool _is_registered = false;

            if (_mappings.ContainsKey(key))
            {
                _is_registered = true;
                _mappings.TryGetValue(key, out _registered_tuple);
                _message = $@"The key : {key} is already registered against the type {_registered_tuple.concrete_type}.";
            }

            return (_is_registered, _registered_tuple.concrete_type, _message,_registered_tuple.is_singleton);
        }
        public (bool status, Type registered_type, string message, bool is_singleton) checkIfRegistered(Type contract_type)
        {
            return checkIfRegistered(contract_type?.ToString());
        }
        public (bool status, Type registered_type, string message, bool is_singleton) checkIfRegistered<Tcontract>()
        {
            return checkIfRegistered(typeof(Tcontract)?.ToString());
        }


        private bool _validateExistence(Type contract_type, Type concrete_type)
        {
            var _status = checkIfRegistered(contract_type);
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
        #endregion

        #region Private Main Methods
        private bool _register(string override_key, Type contract_type, Type concrete_type, object concrete_instance, IMappingProvider dependencyProvider, MappingLevel mapping_level, bool is_singleton)
        {
            //Validate if the contract type is alredy registered. If so, against correct concrete type.
            bool _exists = _validateExistence(contract_type, concrete_type);

            //If it already exists and we should not over write, then do not proceed.
            if (_exists && (!overwrite_if_registered)) return false;

            //Validate if the concrete type can be registered
            _validateConcreteType(concrete_type);

            //Generate instance only if the provided value is null and also singleton is not required. (else, we can save only the type).
            if (concrete_instance == null && !is_singleton)
            {
                concrete_instance = _createInstance(concrete_type, dependencyProvider, mapping_level, TransientCreationLevel.Current,ResolveMode.Transient); //Create instance resolving all dependencies
            }

            //Process the registry key
            if (string.IsNullOrEmpty(override_key) || string.IsNullOrWhiteSpace(override_key))
            {
                override_key = contract_type.ToString();
            }

            //Generate new tuple
            var _new_tuple = (contract_type, concrete_type, concrete_instance,is_singleton);

            //We have already validate if overwrite is required or not. If we reach this point, then overwrite is required.
            if (_exists)
            {
                //Update the existing value 
                (Type contract_type, Type concrete_type, object concrete_instance,bool is_singleton) _current_tuple;
                _mappings.TryGetValue(override_key, out _current_tuple);
                _mappings.TryUpdate(override_key, _new_tuple, _current_tuple); //Remember to assign the instance
            }
            else
            {
                _mappings.TryAdd(override_key, _new_tuple);
            }
            return true;
        }

        #region Creation Methods
        private object _createInstance(Type concrete_type, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level,ResolveMode mode)
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

                    parameters.Add(_resolve(null, pinfo.Name, pinfo.ParameterType, concrete_type, mapping_provider, mapping_level, transient_level, mode, InjectionTarget.Constructor));
                }
                concrete_instance = constructor.Invoke(parameters.ToArray());
            }

        }
        private void _resolveProperties(Type concrete_type, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level,ResolveMode mode, ref object concrete_instance)
        {
            //If creation is current with properties, then constructor and props should generate new instance. Rest should be resolved.
            if (transient_level == TransientCreationLevel.CurrentWithProperties) transient_level = TransientCreationLevel.Current;

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

                        var resolved_value = _resolve(null, pinfo.Name, _prop_type, concrete_type, mapping_provider, mapping_level, transient_level, mode,InjectionTarget.Property);
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
        private object _resolve(string override_key, string contract_name, Type contract_type, Type contract_parent, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level, ResolveMode resolve_mode, InjectionTarget injection = InjectionTarget.All)
        {
            object concrete_instance = null;

            //If request is for creation of a transient object, then do not try to resolve using internal methods. Just try fetch the concrete type and create instance.
            _transientResolve(override_key, contract_type, mapping_provider, mapping_level, resolve_mode, transient_level, out concrete_instance);

            if (concrete_instance != null) return concrete_instance;

            //If dependency provider is not null, then try to resolve using external mapping.
            _externalMappingResolve(mapping_provider, mapping_level, contract_name, contract_type, contract_parent, injection, out concrete_instance);

            //Mapping is only used inside the external mapping resolution. If mapping is only for current level, then do not proceed further.
            if (mapping_level == MappingLevel.Current) mapping_level = MappingLevel.None;

            //If still object is null, try resolving using internal mapping.
            if (concrete_instance == null)
            {
                if (contract_type == typeof(string) || contract_type.IsValueType)
                {
                    throw new ArgumentException($@"Value type dependency error. The {contract_parent ?? contract_type} contains a value dependency {contract_name ?? ""}. Try adding a mapping provider for injecting value types.");
                }
                _internalMappingResolve(override_key, contract_type, mapping_provider, mapping_level, transient_level,resolve_mode, out concrete_instance);
            }

            return concrete_instance;
        }
        private void _externalMappingResolve(IMappingProvider mapping_provider, MappingLevel mapping_level, string contract_name, Type contract_type, Type contract_parent, InjectionTarget injection, out object concrete_instance)
        {
            //Begin with null output.
            concrete_instance = null;

            if (mapping_level == MappingLevel.None) return;

            if (contract_type == null) { throw new ArgumentNullException(nameof(contract_type)); }

            //if external mapping is null, no point in proceeding.
            if (mapping_provider == null) return;

            var _dip_values = mapping_provider.Resolve(contract_type, contract_name, contract_parent);

            //if external mapping resolves to a value, ensure that this injection is for suitable target or it should be for all
            if (_dip_values.concrete_instance != null && (injection == _dip_values.injection || injection == InjectionTarget.All))
            {
                concrete_instance = _dip_values.concrete_instance;
            }
        }
        private void _internalMappingResolve(string override_key, Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level, TransientCreationLevel transient_level,ResolveMode mode, out object concrete_instance)
        {
            concrete_instance = null;

            if (contract_type == null) throw new ArgumentNullException(nameof(contract_type));

            var _mapping_value = _getMapping(override_key, contract_type);

            if (concrete_instance == null && _mapping_value.exists)
            {
                //If resolve mode is default, we process, as per the registration.
                if (_mapping_value.is_singleton) //If singleton, return concrete instance.
                {
                    concrete_instance = _mapping_value.concrete_instance;
                }
                else //Registered as transient.
                {
                    concrete_instance = _createInstance(_mapping_value.concrete_type, mapping_provider, mapping_level, transient_level, mode);
                }
            }
        }
        private void _transientResolve(string override_key, Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level,ResolveMode resolve_mode, TransientCreationLevel transient_level, out object concrete_instance)
        {
            concrete_instance = null;
            if (resolve_mode == ResolveMode.Transient && transient_level != TransientCreationLevel.None)
            {
                var _mapping_value = _getMapping(override_key, contract_type);
                if (_mapping_value.exists) //In case, contract is of Interface which already has a registered concrete type.
                {
                    concrete_instance = _createInstance(_mapping_value.concrete_type, mapping_provider, mapping_level, transient_level,resolve_mode);
                }
                else
                {
                    concrete_instance = _createInstance(contract_type, mapping_provider, mapping_level, transient_level, resolve_mode);
                }
            }
        }

        private (bool exists,Type contract_type, Type concrete_type, object concrete_instance, bool is_singleton) _getMapping(string mapping_key,Type contract_type)
        {
            if (string.IsNullOrEmpty(mapping_key) || string.IsNullOrWhiteSpace(mapping_key))
            {
                mapping_key = contract_type?.ToString();
            }
            bool _exists = false;

            (Type _contract_type, Type _concrete_type, object _concrete_instance, bool is_singleton) _registered_tuple = (null, null, null, true);

            if (_mappings.ContainsKey(mapping_key))
            {
                _exists = true;
                _mappings.TryGetValue(mapping_key, out _registered_tuple);
            }
            return (_exists, _registered_tuple._contract_type,_registered_tuple._concrete_type,_registered_tuple._concrete_instance,_registered_tuple.is_singleton);
        }
        #endregion

        #endregion

        #region Register Methods
        public void Register<TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            RegisterWithKey<TConcrete>(null,mode);
        }
        public void Register<TConcrete>(TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            RegisterWithKey(null,instance, mode);
        }
        public void Register<TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            RegisterWithKey<TConcrete>(null, dependencyProvider, mapping_level, mode);
        }
        public void Register<TContract, TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            RegisterWithKey<TContract, TConcrete>(null, mode);
        }
        public void Register<TContract, TConcrete>(TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            RegisterWithKey<TContract, TConcrete>(null, instance, mode);
        }
        
        public void Register<TContract, TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            RegisterWithKey<TContract, TConcrete>(null, dependencyProvider, mapping_level, mode);
        }

        #endregion

        #region TryRegister Methods
        public bool TryRegister<TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            try
            {
                return _register(null, typeof(TConcrete), typeof(TConcrete), null, null, MappingLevel.None, mode == RegisterMode.Singleton);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TryRegister<TConcrete>(TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            try
            {
                return _register(null, typeof(TConcrete), typeof(TConcrete), instance, null, MappingLevel.None, mode == RegisterMode.Singleton);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryRegister<TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            try
            {
                return _register(null, typeof(TConcrete), typeof(TConcrete), null, dependencyProvider, mapping_level, mode == RegisterMode.Singleton);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryRegister<TContract, TConcrete>(TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            try
            {
                return _register(null, typeof(TContract), typeof(TConcrete), instance, null, MappingLevel.None, mode == RegisterMode.Singleton);
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
                return _register(null, typeof(TContract), typeof(TConcrete), null, null, MappingLevel.None, mode == RegisterMode.Singleton);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryRegister<TContract, TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            try
            {
                return _register(null, typeof(TContract), typeof(TConcrete), null, dependencyProvider, mapping_level, mode == RegisterMode.Singleton);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region RegisterWithKey Methods
        public void RegisterWithKey<TConcrete>(string override_key, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            _register(override_key, typeof(TConcrete), typeof(TConcrete), null, null, MappingLevel.None, mode == RegisterMode.Singleton);
        }

        public void RegisterWithKey<TConcrete>(string override_key, TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            _register(override_key, typeof(TConcrete), typeof(TConcrete), instance, null, MappingLevel.None, mode == RegisterMode.Singleton);
        }

        public void RegisterWithKey<TConcrete>(string override_key, IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            _register(override_key, typeof(TConcrete), typeof(TConcrete), null, dependencyProvider, mapping_level, mode == RegisterMode.Singleton);
        }
        public void RegisterWithKey<TContract, TConcrete>(string override_key, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            _register(override_key, typeof(TContract), typeof(TConcrete), null, null, MappingLevel.None, mode == RegisterMode.Singleton);
        }
        public void RegisterWithKey<TContract, TConcrete>(string override_key, TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
           _register(override_key, typeof(TContract), typeof(TConcrete), instance, null, MappingLevel.None, mode == RegisterMode.Singleton);
        }

        public void RegisterWithKey<TContract, TConcrete>(string override_key, IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            _register(override_key, typeof(TContract), typeof(TConcrete), null, dependencyProvider, mapping_level, mode == RegisterMode.Singleton);
        }
        #endregion

        #region Resolve Methods
        public T Resolve<T>(ResolveMode mode = ResolveMode.Default)
        {
            var _obj = Resolve(typeof(T),mode);
            return (T)_obj;
        }

        public object Resolve(Type contract_type, ResolveMode mode = ResolveMode.Default)
        {
            TransientCreationLevel _transient_level = TransientCreationLevel.None;
            if (mode == ResolveMode.Transient) _transient_level = TransientCreationLevel.Current;

            return _resolve(null,null,contract_type,null,null,MappingLevel.Current, _transient_level, mode);
        }

        public object Resolve(string override_key, ResolveMode mode = ResolveMode.Default)
        {
            TransientCreationLevel _transient_level = TransientCreationLevel.None;
            if (mode == ResolveMode.Transient) _transient_level = TransientCreationLevel.Current;

            return _resolve(override_key, null, null, null, null, MappingLevel.Current, _transient_level,mode);
        }

        public T Resolve<T>(IMappingProvider mapping_provider, MappingLevel mapping_level, ResolveMode mode = ResolveMode.Default)
        {
            var _obj = Resolve(typeof(T), mapping_provider, mapping_level);
            return (T)_obj;
        }

        public object Resolve(Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level, ResolveMode mode = ResolveMode.Default)
        {
            TransientCreationLevel _transient_level = TransientCreationLevel.None;
            if (mode == ResolveMode.Transient) _transient_level = TransientCreationLevel.Current;

            return _resolve(null, null, contract_type, null, mapping_provider, mapping_level, _transient_level,mode);
        }

        public object Resolve(string override_key, IMappingProvider mapping_provider, MappingLevel mapping_level, ResolveMode mode = ResolveMode.Default)
        {
            TransientCreationLevel _transient_level = TransientCreationLevel.None;
            if (mode == ResolveMode.Transient) _transient_level = TransientCreationLevel.Current;

            return _resolve(override_key, null, null, null, mapping_provider, mapping_level, _transient_level,mode);
        }
        #endregion

        #region TryResolve Methods

        public bool TryResolve(Type contract_type, out object concrete_instance, ResolveMode mode = ResolveMode.Default)
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

        public bool TryResolve(string override_key, out object concrete_instance, ResolveMode mode = ResolveMode.Default)
        {
            try
            {
                concrete_instance = Resolve(override_key, mode);
                return true;
            }
            catch (Exception)
            {
                concrete_instance = null;
                return false;
            }
        }

        public bool TryResolve(Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level, out object concrete_instance, ResolveMode mode = ResolveMode.Default)
        {
            try
            {
                concrete_instance = Resolve(contract_type,mapping_provider,mapping_level, mode);
                return true;
            }
            catch (Exception)
            {
                concrete_instance = null;
                return false;
            }
        }

        public bool TryResolve(string override_key, IMappingProvider mapping_provider, MappingLevel mapping_level, out object concrete_instance, ResolveMode mode = ResolveMode.Default)
        {
            try
            {
                concrete_instance = Resolve(override_key, mapping_provider, mapping_level, mode);
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
            var _obj = ResolveTransient(typeof(T),transient_level);
            return (T)_obj;
        }

        public object ResolveTransient(Type contract_type, TransientCreationLevel transient_level)
        {
            return _resolve(null, null, contract_type, null, null, MappingLevel.Current, transient_level,ResolveMode.Transient);
        }

        public object ResolveTransient(string override_key, TransientCreationLevel transient_level)
        {
            return _resolve(override_key, null, null, null, null, MappingLevel.Current, transient_level, ResolveMode.Transient);
        }

        public T ResolveTransient<T>(TransientCreationLevel transient_level, IMappingProvider mapping_provider, MappingLevel mapping_level)
        {
            var _obj = ResolveTransient(typeof(T), transient_level, mapping_provider, mapping_level);
            return (T)_obj;
        }

        public object ResolveTransient(Type contract_type, TransientCreationLevel transient_level, IMappingProvider mapping_provider, MappingLevel mapping_level)
        {
            return _resolve(null, null, contract_type, null, mapping_provider, mapping_level, transient_level, ResolveMode.Transient);
        }

        public object ResolveTransient(string override_key, TransientCreationLevel transient_level, IMappingProvider mapping_provider, MappingLevel mapping_level)
        {
            return _resolve(override_key, null, null, null, mapping_provider, mapping_level, transient_level, ResolveMode.Transient);
        }

        #endregion

        public DIContainer() 
        {
            overwrite_if_registered = false;
            ignore_if_registered = false; 
        }
    }
}
