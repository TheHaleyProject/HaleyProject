using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using System.Reflection;
using System.Configuration;
using System.CodeDom;
using System.Collections.Concurrent;
using System.ComponentModel;
using Haley.Enums;
using System.Runtime.InteropServices;
using System.Windows.Documents;

namespace Haley.MVVM.Containers
{
    public sealed class DIContainer : IHaleyDIContainer
    {
        #region ATTRIBUTES
        private readonly ConcurrentDictionary<string, (Type contract_type, Type concrete_type, object concrete_instance)> _mappings = new ConcurrentDictionary<string, (Type contract_type, Type concrete_type, object concrete_instance)>();
        #endregion

        #region Properties
        public bool ignore_if_registered { get; set; }
        public bool overwrite_if_registered { get; set; }
        #endregion

        #region Validations
        public (bool status, Type registered_type, string message) checkIfRegistered(string key)
        {
            (Type contract_type, Type concrete_type, object concrete_instance) _registered_tuple = (null, null, null);

            string _message = null;
            bool _is_registered = false;

            if (_mappings.ContainsKey(key))
            {
                _is_registered = true;
                _mappings.TryGetValue(key, out _registered_tuple);
                _message = $@"The key : {key} is already registered against the type {_registered_tuple.concrete_type}.";
            }

            return (_is_registered, _registered_tuple.concrete_type, _message);
        }
        public (bool status, Type registered_type, string message) checkIfRegistered(Type contract_type)
        {
            return checkIfRegistered(contract_type?.ToString());
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

        #region Register Methods

        private void _register(Type contract_type, Type concrete_type, object concrete_instance,IMappingProvider dependencyProvider, string custom_key)
        {
            //Validate if the contract type is alredy registered. If so, against correct concrete type.
            bool _exists = _validateExistence(contract_type, concrete_type);

            //If it already exists and we should not over write, then do not proceed.
            if (_exists && (!overwrite_if_registered)) return;

            //Validate if the concrete type can be registered
            _validateConcreteType(concrete_type);

            //Generate instance only if the provided value is null
            if (concrete_instance == null )
            {
                concrete_instance = _createInstance(concrete_type, dependencyProvider, GenerateNewInstanceFor.TargetObjectOnly); //Create instance resolving all dependencies
            }

            //Process the registry key
            if (string.IsNullOrEmpty(custom_key) || string.IsNullOrWhiteSpace(custom_key))
            {
                custom_key = contract_type.ToString();
            }

            //Generate new tuple
            var _new_tuple = (contract_type, concrete_type, concrete_instance);

            //We have already validate if overwrite is required or not. If we reach this point, then overwrite is required.
            if (_exists)
            {
                //Overwrite the existing value 
                (Type contract_type, Type concrete_type, object concrete_instance) _current_tuple;
                _mappings.TryGetValue(custom_key, out _current_tuple);
                _mappings.TryUpdate(custom_key, _new_tuple, _current_tuple); //Remember to assign the instance
            }
            else
            { 
               _mappings.TryAdd(custom_key, _new_tuple);
            }
        }
        public void Register<TConcrete>(string key = null) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            _register(typeof(TConcrete), typeof(TConcrete), null, null, key);
        }
        public void Register<TConcrete>(TConcrete instance, string key = null) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            _register(typeof(TConcrete), typeof(TConcrete), instance, null, key);
        }
        public void Register<TConcrete>(IMappingProvider dependencyProvider, string key = null) where TConcrete : class
        {
            //For this method, both contract and concrete type are same.
            _register(typeof(TConcrete), typeof(TConcrete), null,dependencyProvider, key);
        }
        public void Register<TContract, TConcrete>(TConcrete instance, string key = null) where TConcrete : class, TContract
        {
            _register(typeof(TContract), typeof(TConcrete), instance, null, key);
        }
        public void Register<TContract, TConcrete>(string key = null) where TConcrete : class, TContract
        {
            _register(typeof(TContract), typeof(TConcrete), null, null, key);
        }
        public void Register<TContract, TConcrete>(IMappingProvider dependencyProvider, string key = null) where TConcrete : class, TContract
        {
            _register(typeof(TContract), typeof(TConcrete), null, dependencyProvider, key);
        }

        #endregion

        #region Resolution Methods
        public T Resolve<T>(GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None)
        {
            var _obj = Resolve(typeof(T), instance_level);
            return (T)_obj;
        }
        public object Resolve(Type input_type, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None)
        {
            return _resolve(null,input_type,null, null, instance_level, InjectionTarget.All);
        }
        public T Resolve<T>(IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.TargetObjectWithParameters)
        {
            var _obj = Resolve(typeof(T), mapping_provider, instance_level);
            return (T)_obj;
        }
        public object Resolve(Type input_type, IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.TargetObjectWithParameters)
        {
            if (instance_level == GenerateNewInstanceFor.None)
            { instance_level = GenerateNewInstanceFor.TargetObjectWithParameters; }
            return _resolve(null, input_type, null, mapping_provider, instance_level, InjectionTarget.All);
        }
        public object Resolve(string custom_key, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None)
        {
            throw new NotImplementedException();
        }
        public object Resolve(string custom_key, IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.TargetObjectWithParameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Creation Methods
        private object _resolve(string contract_name, Type contract_type, Type contract_parent, IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level, InjectionTarget targetInjection)
        {
            object concrete_instance = null;

            //If dependency provider is not null, then try to resolve using external mapping.
            _externalMappingResolve(mapping_provider, contract_name, contract_type, contract_parent, instance_level, targetInjection, out concrete_instance);

            //If still object is null, try resolving using internal mapping.
            if (concrete_instance == null)
            {
                if (contract_type == typeof(string) || contract_type.IsValueType)
                {
                    throw new ArgumentException($@"Value type dependency error. The {contract_parent ?? contract_type} contains a value dependency {contract_name ?? ""}. Try adding a mapping provider for injecting value types.");
                }
                _internalMappingResolve(contract_type, mapping_provider, instance_level, out concrete_instance);
            }

            return concrete_instance;
        }
        private void _externalMappingResolve(IMappingProvider mapping_provider, string contract_name, Type contract_type, Type contract_parent, GenerateNewInstanceFor instance_level, InjectionTarget targetType, out object concrete_instance)
        {
            //Begin with null output.
            concrete_instance = null;
            if (contract_type == null) { throw new ArgumentNullException(nameof(contract_type)); }

            //if external mapping is null, no point in proceeding.
            if (mapping_provider == null) return;
            var _dip_values = mapping_provider.Resolve(contract_type,contract_name, contract_parent);

            //if external mapping resolves to a value, ensure that this injection is for suitable target or it should be for all
            if (_dip_values.concrete_instance != null && (targetType == _dip_values.target || targetType == InjectionTarget.All))
            {
                concrete_instance = _dip_values.concrete_instance;
            }
        }
        private void _internalMappingResolve(Type contract_type, IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level, out object concrete_instance, string custom_key = null)
        {
            concrete_instance = null;
            if (string.IsNullOrEmpty(custom_key) || string.IsNullOrWhiteSpace(custom_key))
            {
                custom_key = contract_type.ToString();
            }

            if (contract_type == null) throw new ArgumentNullException(nameof(contract_type));

            if (concrete_instance == null && _mappings.ContainsKey(custom_key))
            {
                (Type _contract_type, Type _concrete_type, object _concrete_instance) _registered_tuple = (null, null,null);
                _mappings.TryGetValue(custom_key, out _registered_tuple);

                //if the request doesn't specify instance creation, then we return the singleton object.
                if (instance_level == GenerateNewInstanceFor.None)
                {
                    concrete_instance = _registered_tuple._concrete_instance;
                }
                else
                {
                    concrete_instance = _createInstance(_registered_tuple._concrete_type, mapping_provider, instance_level);
                }
            }
        }
        private object _createInstance(Type concrete_type, IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level)
        {
            if (instance_level == GenerateNewInstanceFor.TargetObjectOnly) instance_level = GenerateNewInstanceFor.None; //If creation is target only, then further dependencies should not generate instance.
            object _instance = null;
            _validateConcreteType(concrete_type);
            ConstructorInfo constructor = _getConstructor(concrete_type);
            _resolveConstructorParameters(ref constructor, concrete_type, mapping_provider, instance_level, ref _instance);
            _resolveProperties(concrete_type, mapping_provider, instance_level, ref _instance);
            return _instance;
        }
        #endregion

        #region Private Resolution Methods
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
        private void _resolveConstructorParameters(ref ConstructorInfo constructor, Type concrete_type, IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level, ref object _instance)
        {
            //Resolve the param arugments for the constructor.
            ParameterInfo[] constructor_params = constructor.GetParameters();

            //If parameter less construction, return a new creation.
            if (constructor_params.Length == 0)
            {
                _instance = Activator.CreateInstance(concrete_type);
            }
            else
            {
                List<object> parameters = new List<object>(constructor_params.Length);
                foreach (ParameterInfo pinfo in constructor_params)
                {
                    Type _paramtype = pinfo.ParameterType;

                    //If dependency provider is not null, then try to rsolve using it. if this value is also null, then try to resolve using actual provider

                    //RESOLVE RECURSIVELY
                    parameters.Add(_resolve(pinfo.Name, pinfo.ParameterType, concrete_type, mapping_provider, instance_level, InjectionTarget.Constructor));
                }
                _instance = constructor.Invoke(parameters.ToArray());
            }

        }
        private void _resolveProperties(Type concrete_type, IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level, ref object _instance)
        {
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

                        var resolved_value = _resolve(pinfo.Name, _prop_type, concrete_type, mapping_provider, instance_level, InjectionTarget.Property);
                        if (resolved_value != null) pinfo.SetValue(_instance, resolved_value);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }
        #endregion
        public DIContainer() 
        {
            overwrite_if_registered = false;
            ignore_if_registered = false; 
        }
    }
}
