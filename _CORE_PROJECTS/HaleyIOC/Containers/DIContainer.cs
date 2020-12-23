using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using Haley.Enums;
using System.Runtime.InteropServices;
using Haley.Models;
using Haley.Abstractions;
using Haley.Utils;

namespace Haley.IOC
{
    public sealed class DIContainer : IHaleyDIContainer
    {
        #region ATTRIBUTES
        private readonly ConcurrentDictionary<KeyBase, RegisterLoad> _mappings = new ConcurrentDictionary<KeyBase, RegisterLoad>();
        #endregion

        #region Properties
        public bool ignore_if_registered { get; set; }
        public bool overwrite_if_registered { get; set; }
        #endregion

        #region PRIVATE METHODS

        #region Helpers
        private TransientCreationLevel _convertToTransientLevel(MappingLevel mapping_level)
        {
            TransientCreationLevel _transient_level = TransientCreationLevel.None;
            switch (mapping_level)
            {
                case MappingLevel.Current:
                    _transient_level = TransientCreationLevel.Current;
                    break;
                case MappingLevel.CurrentWithDependencies:
                    _transient_level = TransientCreationLevel.CurrentWithDependencies;
                    break;
                case MappingLevel.CascadeAll:
                    _transient_level = TransientCreationLevel.CascadeAll;
                    break;
            }
            return _transient_level;
        }
        private bool _validateExistence(RegisterLoad register_load)
        {
            var _status = checkIfRegistered(register_load.contract_type, register_load.priority_key);
            //If registered and also ignore
            if (_status.status)
            {
                if (!ignore_if_registered)
                {
                    //Throw the error, only if you should not ignore the registered status.
                    if (_status.registered_type != register_load.concrete_type)
                    {
                        throw new ArgumentException(_status.message);
                    }
                }
            }
            return _status.status; //Returns if registered.
        }
        private void _validateConcreteType(Type concrete_type)
        {
            if (concrete_type == null || concrete_type.IsAbstract || concrete_type.IsEnum || concrete_type.IsInterface || concrete_type.IsArray || concrete_type.IsList() || concrete_type.IsEnumerable() || concrete_type.IsDictionary() || concrete_type.IsCollection() )
            {
                throw new ArgumentException($@"Concrete type cannot be null, abstract, enum, interface, array, list, enumerable, dictionary, or collection. {concrete_type} is not a valid concrete type.");
            }
        }
        private List<RegisterLoad> _getAllMappings(Type contract_type)
        {
            //For the given type, get all the mappings.
            var _keys = _mappings.Keys.Where(_key => _key.contract_type == contract_type);
            if (_keys.Count() == 0) return null;

            List<RegisterLoad> _result = new List<RegisterLoad>();

            foreach (var _key in _keys)
            {
                RegisterLoad _load;
                _mappings.TryGetValue(_key, out _load);
                _result.Add (_load);
            }
            return _result;
        }
        private (bool exists, RegisterLoad load) _getMapping(string priority_key, Type contract_type)
        {

            //Always give importance to priority key. If priority key combination is not found, the go with contract type alone.
            bool _exists = false;

            RegisterLoad _existing = new RegisterLoad();
            var _key = new KeyBase(contract_type, priority_key);

            //Preference to prioritykey/contract_type combination.
            if (_mappings.ContainsKey(_key))
            {
                _exists = true;
                _mappings.TryGetValue(_key, out _existing);
            }
            if (!_exists)
            {
                //Reassigning the key with contract type name.
                _key = new KeyBase(contract_type,null);
                if (_mappings.ContainsKey(_key))
                {
                    _exists = true;
                    _mappings.TryGetValue(_key, out _existing);
                }
            }

            return (_exists, _existing);
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
        #endregion

        #region Creation Methods
        private bool _register(RegisterLoad register_load,MappingLoad mapping_load)
        {
            //Validate if the contract type is alredy registered. If so, against correct concrete type.
            bool _exists = _validateExistence(register_load);

            //If it already exists and we should not over write, then do not proceed.
            if (_exists && (!overwrite_if_registered)) return false;

            //Validate if the concrete type can be registered
            _validateConcreteType(register_load.concrete_type);

            //Generate instance only if the provided value is null and also singleton. Only if it is singleton, we create an instance and store. Else we store only the concrete type and save instance as it is (even if is null).
            if (register_load.concrete_instance == null && register_load.mode == RegisterMode.Singleton)
            {
                ResolveLoad resolve_load = register_load.convert(null, null, ResolveMode.AsRegistered);
                register_load.concrete_instance = _createInstance(resolve_load, mapping_load); //Create instance resolving all dependencies
            }

            //Get the key to register.
            var _key = new KeyBase(register_load.contract_type, register_load.priority_key);

            //We have already validate if overwrite is required or not. If we reach this point, then overwrite is required.
            if (_exists)
            {
                //Update the existing value 
                RegisterLoad _existing_value;
                _mappings.TryGetValue(_key, out _existing_value);
                _mappings.TryUpdate(_key, register_load, _existing_value); //Remember to assign the instance
            }
            else
            {
                _mappings.TryAdd(_key, register_load);
            }
            return true;
        }
        private object _createInstance(ResolveLoad resolve_load,MappingLoad mapping_load)
        {
            //If transient creation is current level only, then further dependencies should not generate new instance.
            if (resolve_load.transient_level == TransientCreationLevel.Current) resolve_load.transient_level = TransientCreationLevel.None;

            object concrete_instance = null;
            _validateConcreteType(resolve_load.concrete_type);
            ConstructorInfo constructor = _getConstructor(resolve_load.concrete_type);
            _resolveConstructorParameters(ref constructor, resolve_load,mapping_load, ref concrete_instance);
            _resolveProperties(resolve_load,mapping_load, ref concrete_instance);
            return concrete_instance;
        }
        private void _resolveConstructorParameters(ref ConstructorInfo constructor, ResolveLoad resolve_load, MappingLoad mapping_load, ref object concrete_instance)
        {
            //If creation is current with properties, then constructor and props should generate new instance. Rest should be resolved.
            if (resolve_load.transient_level == TransientCreationLevel.CurrentWithDependencies)
            { resolve_load.transient_level = TransientCreationLevel.Current; }

            //TODO: CHECK IF BELOW REASSIGNMENT IS REQUIRED.
            if (mapping_load?.level == MappingLevel.CurrentWithDependencies)
            { mapping_load.level = MappingLevel.Current; }

            //Resolve the param arugments for the constructor.
            ParameterInfo[] constructor_params = constructor.GetParameters();

            //If parameter less construction, return a new creation.
            if (constructor_params.Length == 0)
            {
                concrete_instance = Activator.CreateInstance(resolve_load.concrete_type);
            }
            else
            {
                List<object> parameters = new List<object>(constructor_params.Length);
                foreach (ParameterInfo pinfo in constructor_params)
                {
                    //New resolve and mapping load.
                    ResolveLoad _new_res_load = new ResolveLoad(resolve_load.mode, resolve_load.priority_key, pinfo.Name, pinfo.ParameterType, resolve_load.concrete_type, null, resolve_load.transient_level);

                    MappingLoad _new_map_load = new MappingLoad(mapping_load.provider, mapping_load.level, InjectionTarget.Constructor);

                    parameters.Add(_mainResolve(_new_res_load,_new_map_load));
                }
                concrete_instance = constructor.Invoke(parameters.ToArray());
            }

        }
        private void _resolveProperties(ResolveLoad resolve_load, MappingLoad mapping_load, ref object concrete_instance)
        {
            //If creation is current with properties, then constructor and props should generate new instance. Rest should be resolved.
            if (resolve_load.transient_level == TransientCreationLevel.CurrentWithDependencies) resolve_load.transient_level = TransientCreationLevel.Current;
            if (mapping_load?.level == MappingLevel.CurrentWithDependencies) mapping_load.level = MappingLevel.Current;

            //Resolve only properties that are of type Haley inject and also ignore if it has haleyignore
            var _props = resolve_load.concrete_type.GetProperties().Where(
                p => Attribute.IsDefined(p, typeof(HaleyInjectAttribute)));

            if (_props.Count() > 0)
            {
                foreach (PropertyInfo pinfo in _props)
                {
                    try
                    {
                        //New resolve and mapping load.
                        ResolveLoad _new_res_load = new ResolveLoad(resolve_load.mode, resolve_load.priority_key, pinfo.Name, pinfo.PropertyType, resolve_load.concrete_type, null, resolve_load.transient_level);

                        MappingLoad _new_map_load = new MappingLoad(mapping_load.provider, mapping_load.level, InjectionTarget.Property);

                        var resolved_value = _mainResolve(_new_res_load, _new_map_load);
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
        private object _mainResolve(ResolveLoad resolve_load,MappingLoad mapping_load)
        {
            object concrete_instance = null;

            switch(resolve_load.mode)
            {
                case ResolveMode.AsRegistered: //This can be transient or singleton.
                    _resolveAsRegistered(resolve_load,mapping_load, out concrete_instance);
                    break;
                case ResolveMode.Transient: //This creates new instance.
                    _resolveAsTransient(resolve_load, mapping_load, out concrete_instance);
                    break;
            }

            return concrete_instance;
        }
        private void _resolveWithMappingProvider(ResolveLoad resolve_load,ref MappingLoad mapping_load, out object concrete_instance)
        {
            //Begin with null output.
            concrete_instance = null;

            //Mapping level defines until which stage or level , the mapping should be applied. If mapping provider is null or mapping level is none, don't proceed. 
            if (mapping_load.provider == null || mapping_load.level == MappingLevel.None) return;

            if (mapping_load.level == MappingLevel.Current)
            { mapping_load.level = MappingLevel.None; }

            if (resolve_load.contract_type == null) { throw new ArgumentNullException(nameof(resolve_load.contract_type)); }

            var _dip_values = mapping_load.provider.Resolve(resolve_load.contract_type, resolve_load.contract_name, resolve_load.contract_parent);

            //if external mapping resolves to a value, ensure that this injection is for suitable target or it should be for all
            if (_dip_values.concrete_instance != null && (mapping_load.injection == _dip_values.injection || mapping_load.injection == InjectionTarget.All))
            {
                concrete_instance = _dip_values.concrete_instance;
            }
        }
        private void _resolveAsRegistered(ResolveLoad resolve_load, MappingLoad mapping_load, out object concrete_instance)
        {
            concrete_instance = null;
            Type current_contract_type = resolve_load.contract_type;
            if (current_contract_type == null) throw new ArgumentNullException(nameof(current_contract_type));

            //Try to resolve multiple params if needed.
            _resolveArrayTypes(resolve_load,mapping_load, out concrete_instance);

            if (concrete_instance != null) return;
            var _registered = _getMapping(resolve_load.priority_key, current_contract_type);

            //Try to resolve with mapping provider before anything.
            _resolveWithMappingProvider(resolve_load,ref mapping_load, out concrete_instance);

            if (concrete_instance != null) return;

            //If it is registered, then resolve it else re send request as transient.
            if (_registered.exists)
            {
                //If already exists, then fetch the concrete type. Also, if a concrete type is registered, we can be confident that it has already passed the concrete type validation.
                resolve_load.concrete_type = _registered.load.concrete_type ?? resolve_load.concrete_type ?? current_contract_type;

                switch (_registered.load.mode)
                {
                    case RegisterMode.Singleton:
                        concrete_instance = _registered.load.concrete_instance;
                        break;
                    case RegisterMode.Transient:
                        concrete_instance = _createInstance(resolve_load, mapping_load);
                        break;
                }
            }
            else // It is not registered. So, we reassign as transient resolution.
            {
                resolve_load.mode = ResolveMode.Transient;
                if (resolve_load.transient_level == TransientCreationLevel.None) { resolve_load.transient_level = TransientCreationLevel.Current; }

                //todo: Should we reset the mapping level as well??
                concrete_instance = _mainResolve(resolve_load, mapping_load);
            }
        }
        private void _resolveAsTransient(ResolveLoad resolve_load, MappingLoad mapping_load, out object concrete_instance)
        {
            concrete_instance = null;
            //Try to resolve multiple params if needed.
            _resolveArrayTypes(resolve_load, mapping_load, out concrete_instance);
            if (concrete_instance != null) return;

            var _registered = _getMapping(resolve_load.priority_key, resolve_load.contract_type);
            //By default, create instance for the contract type.
            if (resolve_load.concrete_type == null)
            { resolve_load.concrete_type = resolve_load.contract_type; }
            //If a mapping already exists, then create instance for the concrete type in mapping.
            if (_registered.exists)
            { resolve_load.concrete_type = _registered.load.concrete_type; }

            //Try to resolve with mapping provider before anything.
            _resolveWithMappingProvider(resolve_load, ref mapping_load, out concrete_instance);

            if (concrete_instance != null) return;

            //Validate concrete type.
            if (resolve_load.concrete_type == typeof(string) || resolve_load.concrete_type.IsValueType )
            {
                throw new ArgumentException($@"Value type dependency error. The {resolve_load.contract_parent ?? resolve_load.contract_type} with contract name {resolve_load.contract_name ?? "#NotFound#"} contains a value dependency {resolve_load.concrete_type}. Try adding a mapping provider for injecting value types.");
            }

            //If transient is not none, try to create new instance. If none, then go with as registered.
            if (resolve_load.transient_level != TransientCreationLevel.None)
            {
                 concrete_instance = _createInstance(resolve_load,mapping_load);
            }
            else
            {
                resolve_load.mode = ResolveMode.AsRegistered;
                concrete_instance = _mainResolve(resolve_load,mapping_load);
            }
        }
        private void _resolveArrayTypes(ResolveLoad resolve_load,MappingLoad mapping_load, out object concrete_instance)
        {
            concrete_instance = null;
            Type array_contract_type = null;
            //If contracttype is of list or enumerable or array or collection, then return all the registered values for the generictypedefinition
            if (resolve_load.contract_type.IsList())
            {
                //We need to check the generic type.
                array_contract_type = resolve_load.contract_type.GetGenericArguments()[0];
            }
            else if (resolve_load.contract_type.IsArray)
            {
                array_contract_type = resolve_load.contract_type.GetElementType();
            }

            if (array_contract_type == null) return; //Then this value is null and unable to resolve.

            List<RegisterLoad> _registrations = new List<RegisterLoad>();
            _registrations = _getAllMappings(array_contract_type) ?? new List<RegisterLoad>();
            List<object> _instances_list = new List<object>();

            if (_registrations.Count > 0)
            {
                foreach (var _registration in _registrations)
                {
                    try
                    {
                        ResolveLoad _new_resolve_load = _registration.convert(resolve_load.contract_name, resolve_load.contract_parent, resolve_load.mode);
                        _new_resolve_load.transient_level = resolve_load.transient_level;
                        var _current_instance = _mainResolve(_new_resolve_load, mapping_load);
                        _instances_list.Add(_current_instance);
                    }
                    catch (Exception)
                    {
                        // Don't throw, continue
                        continue; //Implement a logger to capture the details and return back to the user.
                    }
                }
                concrete_instance =  _instances_list.changeType(resolve_load.contract_type); //Convert to the contract type.
            }
        }
        #endregion

        #endregion

        #region PUBLIC METHODS

        #region Validations
        public (bool status, Type registered_type, string message, RegisterMode mode) checkIfRegistered(KeyBase key)
        {
            RegisterLoad _current_load = new RegisterLoad();

            string _message = null;
            bool _is_registered = false;

            if (_mappings.ContainsKey(key))
            {
                _is_registered = true;
                _mappings.TryGetValue(key, out _current_load);
                _message = $@"The key : {key} is already registered against the type {_current_load.concrete_type}.";
            }

            return (_is_registered, _current_load?.concrete_type, _message, _current_load.mode );
        }
        public (bool status, Type registered_type, string message, RegisterMode mode) checkIfRegistered(Type contract_type, string priority_key)
        {
            return checkIfRegistered(new KeyBase(contract_type, priority_key));
        }
        public (bool status, Type registered_type, string message, RegisterMode mode) checkIfRegistered<Tcontract>(string priority_key)
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
            RegisterLoad _reg_load = new RegisterLoad(mode, priority_key, typeof(TConcrete), typeof(TConcrete),null);
            MappingLoad _map_load = new MappingLoad();
            //For this method, both contract and concrete type are same.
            return _register(_reg_load,_map_load);
        }
        public bool RegisterWithKey<TConcrete>(string priority_key, TConcrete instance) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            //If we have an instance, then obviously it is of singleton registration type.
            RegisterLoad _reg_load = new RegisterLoad(RegisterMode.Singleton, priority_key, typeof(TConcrete), typeof(TConcrete), instance);
            MappingLoad _map_load = new MappingLoad();
            return _register(_reg_load,_map_load);
        }
        public bool RegisterWithKey<TConcrete>(string priority_key, IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            RegisterLoad _reg_load = new RegisterLoad(RegisterMode.Singleton, priority_key, typeof(TConcrete), typeof(TConcrete), null);
            MappingLoad _map_load = new MappingLoad(dependencyProvider,mapping_level);
            return _register(_reg_load,_map_load);
        }
        public bool RegisterWithKey<TContract, TConcrete>(string priority_key, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract
        {
            RegisterLoad _reg_load = new RegisterLoad(mode, priority_key, typeof(TContract), typeof(TConcrete), null);
            MappingLoad _map_load = new MappingLoad();
            return _register(_reg_load,_map_load);
        }
        public bool RegisterWithKey<TContract, TConcrete>(string priority_key, TConcrete instance) where TConcrete : class, TContract
        {
            RegisterLoad _reg_load = new RegisterLoad(RegisterMode.Singleton, priority_key, typeof(TContract), typeof(TConcrete), instance);
            MappingLoad _map_load = new MappingLoad();
            return _register(_reg_load,_map_load);
        }
        public bool RegisterWithKey<TContract, TConcrete>(string priority_key, IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class, TContract
        {
            RegisterLoad _reg_load = new RegisterLoad(RegisterMode.Singleton, priority_key, typeof(TContract), typeof(TConcrete), null);
            MappingLoad _map_load = new MappingLoad(dependencyProvider,mapping_level);
            return _register(_reg_load,_map_load);
        }
        #endregion

        #region Resolve Methods
        public T Resolve<T>(ResolveMode mode = ResolveMode.AsRegistered)
        {
            var _obj = Resolve(typeof(T), mode);
            return (T)_obj.changeType<T>();
        }

        public T Resolve<T>(string priority_key, ResolveMode mode = ResolveMode.AsRegistered)
        {
            var _obj = Resolve(priority_key, typeof(T), mode);
            return (T)_obj.changeType<T>();
        }

        public object Resolve(Type contract_type, ResolveMode mode = ResolveMode.AsRegistered)
        {
            TransientCreationLevel _tlevel = TransientCreationLevel.None;
            if (mode == ResolveMode.Transient) _tlevel = TransientCreationLevel.Current;
            ResolveLoad _request = new ResolveLoad(mode, null, null, contract_type, null, null,_transient_level: _tlevel);
            return _mainResolve(_request,new MappingLoad());
        }
        public object Resolve(string priority_key, Type contract_type, ResolveMode mode = ResolveMode.AsRegistered)
        {
            TransientCreationLevel _tlevel = TransientCreationLevel.None;
            if (mode == ResolveMode.Transient) _tlevel = TransientCreationLevel.Current;
            ResolveLoad _request = new ResolveLoad(mode, priority_key, null, contract_type, null, null, _transient_level: _tlevel);
            return _mainResolve(_request, new MappingLoad());
        }
        public T Resolve<T>(IMappingProvider mapping_provider, ResolveMode mode = ResolveMode.AsRegistered,bool currentOnlyAsTransient = false)
        {
            var _obj = Resolve(typeof(T),mapping_provider, mode, currentOnlyAsTransient);
            return (T)_obj.changeType<T>();
        }

        public object Resolve(Type contract_type, IMappingProvider mapping_provider, ResolveMode mode = ResolveMode.AsRegistered, bool currentOnlyAsTransient = false)
        {
            TransientCreationLevel _tlevel = TransientCreationLevel.Current;
            ResolveLoad _request = new ResolveLoad(mode, null, null, contract_type, null, contract_type, _transient_level: _tlevel);
            MappingLoad _map_load = new MappingLoad(mapping_provider, MappingLevel.CurrentWithDependencies);
            if (mode == ResolveMode.AsRegistered && currentOnlyAsTransient)
            {
                return _createInstance(_request, _map_load); //This ensures that the first level is created as transient, irrespective of the resolve mode.
            }
            return _mainResolve(_request, _map_load);
        }

        public object Resolve(string priority_key, Type contract_type, IMappingProvider mapping_provider, ResolveMode mode = ResolveMode.AsRegistered, bool currentOnlyAsTransient = false)
        {
            TransientCreationLevel _tlevel = TransientCreationLevel.Current;
            ResolveLoad _request = new ResolveLoad(mode, priority_key, null, contract_type, null, null, _transient_level: _tlevel);
            MappingLoad _map_load = new MappingLoad(mapping_provider, MappingLevel.CurrentWithDependencies);

            if (mode == ResolveMode.AsRegistered && currentOnlyAsTransient)
            {
                return _createInstance(_request, _map_load); //This ensures that the first level is created as transient, irrespective of the resolve mode.
            }
            return _mainResolve(_request, _map_load);
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

        public bool TryResolve(Type contract_type, IMappingProvider mapping_provider, out object concrete_instance, ResolveMode mode = ResolveMode.AsRegistered, bool currentOnlyAsTransient = false)
        {
            try
            {
                concrete_instance = Resolve(contract_type,mapping_provider, mode, currentOnlyAsTransient);
                return true;
            }
            catch (Exception)
            {
                concrete_instance = null;
                return false;
            }
        }

        public bool TryResolve(string priority_key, Type contract_type, IMappingProvider mapping_provider, out object concrete_instance, ResolveMode mode = ResolveMode.AsRegistered, bool currentOnlyAsTransient = false)
        {
            try
            {
                concrete_instance = Resolve(priority_key, contract_type,mapping_provider, mode,currentOnlyAsTransient);
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
            return (T)_obj.changeType<T>();
        }
        public T ResolveTransient<T>(string priority_key,TransientCreationLevel transient_level)
        {
            var _obj = ResolveTransient(priority_key, typeof(T), transient_level);
            return (T)_obj.changeType<T>();
        }
        public object ResolveTransient(Type contract_type, TransientCreationLevel transient_level)
        {
            ResolveLoad _res_load = new ResolveLoad(ResolveMode.Transient, null, null, contract_type, null, null, transient_level);
            return _mainResolve(_res_load,new MappingLoad());
        }
        public object ResolveTransient(string priority_key, Type contract_type, TransientCreationLevel transient_level)
        {
            ResolveLoad _res_load = new ResolveLoad(ResolveMode.Transient, priority_key, null, contract_type, null, null, transient_level);
            return _mainResolve(_res_load,new MappingLoad());
        }

        public T ResolveTransient<T>(IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithDependencies)
        {
            var _obj = ResolveTransient(typeof(T), mapping_provider, mapping_level);
            return (T)_obj.changeType<T>();
        }
        public object ResolveTransient(Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithDependencies)
        {
            ResolveLoad _res_load = new ResolveLoad(ResolveMode.Transient, null, null, contract_type, null, null, _convertToTransientLevel(mapping_level));
            MappingLoad _map_load = new MappingLoad(mapping_provider, mapping_level);
            //Change below method.
            return _mainResolve(_res_load,_map_load);
        }
        public object ResolveTransient(string priority_key, Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithDependencies)
        {
            ResolveLoad _res_load = new ResolveLoad(ResolveMode.Transient, priority_key, null, contract_type, null, null, _convertToTransientLevel(mapping_level));
            MappingLoad _map_load = new MappingLoad(mapping_provider, mapping_level);
            //Change below method.
            return _mainResolve(_res_load,_map_load);
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
