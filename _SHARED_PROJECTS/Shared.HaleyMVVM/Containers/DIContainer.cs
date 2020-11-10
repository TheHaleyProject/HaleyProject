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

namespace Haley.MVVM.Containers
{
    public sealed class DIContainer : IHaleyDIContainer
    {
        #region ATTRIBUTES
        //This is where we store all our types. When request is made for an contract type (key), we create an instance based on the value concrete type(value) and return back
        private readonly ConcurrentDictionary<Type, (Type, object)> abstract_type_mappings = new ConcurrentDictionary<Type, (Type, object)>();
        private readonly ConcurrentDictionary<Type, object> concrete_mappings = new ConcurrentDictionary<Type, object>();
        #endregion

        #region Properties
        public bool ignore_if_registered { get; set; }
        public bool overwrite_if_registered { get; set; }
        #endregion

        #region Validations
        public (bool status, Type registered_type, string message) checkIfRegistered(Type input_type)
        {
            //Check if the provided input is present in any of the repository. If yes, then return error stating that it is already registered.
            (Type target_type, object instance) _registered_tuple = (null,null);
            string _message = null;
            bool _is_registered = false;

            //CHECK ABSTRACT SINGLETON REPOSITORY
            if (abstract_type_mappings.ContainsKey(input_type))
            {
                _is_registered = true;
                abstract_type_mappings.TryGetValue(input_type, out _registered_tuple);
                _message = $@"The {input_type} is already registered against the type {_registered_tuple.target_type}.";
            }

            //CHECK CONCRETE SINGLETON REPOSITORY
            if (concrete_mappings.ContainsKey(input_type))
            {
                _is_registered = true;
                _registered_tuple = (input_type, null); //Not returning the value.
                _message = $@"The concrete type: {input_type} is already registered.";
            }

            return (_is_registered, _registered_tuple.target_type, _message);
        }
        private bool _validateExistence(Type input_type, Type target_type)
        {
            var _status = checkIfRegistered(input_type);
            //If registered and also ignore
            if (_status.status)
            {
                if (!ignore_if_registered)
                {
                    //Throw the error, only if you should not ignore the registered status.
                    if (_status.registered_type != target_type)
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

        #region Private Creation Methods
        private object _resolve(string name, Type input_type, Type parent_type, IMappingProvider dependency_provider, GenerateNewInstance instance_level,InjectionTarget targetInjection)
        {
            object _obj = null;
            //Try to resolve using external mapping.
            _externalMappingResolve(dependency_provider, name, input_type, parent_type, instance_level, targetInjection, out _obj);

            //If still object is null, try resolving using internal mapping.
            if (_obj == null)
            {
                if (input_type == typeof(string) || input_type.IsValueType)
                {
                    throw new ArgumentException($@"Value type dependency error. The {parent_type ?? input_type} contains a value dependency {name ??""}. Try adding a mapping provider for injecting value types.");
                }
                _internalMappingResolve(input_type,dependency_provider, instance_level,out _obj);
            }

            return _obj;
        }
        private void _externalMappingResolve(IMappingProvider dependency_provider,string name, Type input_type, Type parent_type, GenerateNewInstance instance_level, InjectionTarget targetType, out object instance)
        {
            //Begin with null output.
            instance = null;
            if (input_type == null) { throw new ArgumentNullException(nameof(input_type)); }

            //if external mapping is null, no point in proceeding.
            if (dependency_provider == null) return;
            var _dip_values = dependency_provider.Resolve(name, input_type, parent_type);

            //if external mapping resolves to a value, ensure that this injection is for suitable target or it should be for all
            if (_dip_values.instance != null && (targetType ==_dip_values.target ||targetType == InjectionTarget.All))
            {
                instance = _dip_values.instance;
            }
        }
        private void _internalMappingResolve(Type input_type, IMappingProvider dependency_provider, GenerateNewInstance instance_level, out object instance)
        {
            instance = null;

            if (input_type == null) throw new ArgumentNullException(nameof(input_type));

            Type _concrete_type = input_type; //It is possible that inputtype could be an abstract type. Then in this case, we validate the type mappings and change this.

            //Priority 1. Type Mappings.
            if (instance == null && abstract_type_mappings.ContainsKey(input_type))
            {
                (Type target_type, object _inst) _registered_tuple = (null, null);
                abstract_type_mappings.TryGetValue(input_type, out _registered_tuple);
                //if the request was for an instance creation, then we return null.
                if (instance_level == GenerateNewInstance.None)
                {
                    instance = _registered_tuple._inst; //Sending the singleton instance value.
                }
                else
                {
                    instance = null;
                    _concrete_type = _registered_tuple.target_type; //This is the actual type for the provided input type.
                }
            }

            //Priority 2.Concrete Mappings.
            if (instance == null && instance_level == GenerateNewInstance.None &&  concrete_mappings.ContainsKey(input_type))
            {
                concrete_mappings.TryGetValue(input_type, out instance);
            }

            if (instance != null) return;

            //Here, we switch with concrete type.
            instance = _createInstance(_concrete_type, dependency_provider, instance_level);
        }
        private object _createInstance(Type concrete_type, IMappingProvider dependency_provider, GenerateNewInstance instance_level)
        {
            if (instance_level == GenerateNewInstance.TargetOnly) instance_level = GenerateNewInstance.None; //If creation is target only, then further dependencies should not generate instance.
            object _instance = null;
            _validateConcreteType(concrete_type);
            ConstructorInfo constructor = _getConstructor(concrete_type);
            _resolveConstructorParameters(ref constructor, concrete_type, dependency_provider, instance_level, ref _instance);
            _resolveProperties(concrete_type, dependency_provider, instance_level, ref _instance);
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
        private void _resolveConstructorParameters(ref ConstructorInfo constructor, Type concrete_type, IMappingProvider dependency_provider, GenerateNewInstance instance_level, ref object _instance)
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
                    parameters.Add(_resolve(pinfo.Name, pinfo.ParameterType, concrete_type, dependency_provider, instance_level,InjectionTarget.Constructor));
                }
                _instance = constructor.Invoke(parameters.ToArray());
            }

        }
        private void _resolveProperties(Type concrete_type, IMappingProvider dependency_provider, GenerateNewInstance instance_level, ref object _instance)
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

                        var resolved_value = _resolve(pinfo.Name, _prop_type, concrete_type, dependency_provider, instance_level,InjectionTarget.Property);
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

        #region Register Methods

        private void _register<TConcrete>(Type input_type,IMappingProvider dependencyProvider, object instance, bool is_type_register = true)
        {
            Type _target_type = typeof(TConcrete);

            //Check if already registered
            bool _exists = _validateExistence(input_type, _target_type);
            if (_exists && (!overwrite_if_registered)) return;

            //Validate
            _validateConcreteType(_target_type);

            //Generate or add instance
            if (instance == null)
            {
                instance = (TConcrete)_createInstance(_target_type, dependencyProvider, GenerateNewInstance.TargetOnly); //Create instance resolving all dependencies
            }

            switch (_exists)
            {
                case true: //Update
                    if (is_type_register)
                    {
                        (Type _type, object _inst) _current_tuple;
                        abstract_type_mappings.TryGetValue(input_type, out _current_tuple);
                        abstract_type_mappings.TryUpdate(input_type, (_target_type, instance), _current_tuple); //Remember to assign the instance
                    }
                    else
                    {
                        object _current_value;
                        concrete_mappings.TryGetValue(_target_type, out _current_value);
                        concrete_mappings.TryUpdate(_target_type, instance, _current_value); //Remember to assign the instance
                    }
                    break;
                case false: //Add
                    if (is_type_register)
                    {
                        abstract_type_mappings.TryAdd(input_type, (_target_type, instance));
                    }
                    else
                    {
                        concrete_mappings.TryAdd(_target_type, instance);
                    }
                    break;
            }
        }

        public void Register<TContract, TConcrete>(TConcrete instance = null) where TConcrete : class, TContract  //TImplementation should either implement or inherit from TContract
        {
            Type _input_type = typeof(TContract);

            _register<TConcrete>(_input_type, null, instance, true);

        }

        public void Register<TConcrete>(TConcrete instance = null) where TConcrete : class  //TImplementation should either implement or inherit from TContract
        {
            Type _input_type = typeof(TConcrete); //Both input and concrete type are same.

            _register<TConcrete>(_input_type, null, instance,false);
        }

        public void Register<TContract, TConcrete>(IMappingProvider dependencyProvider) where TConcrete : class, TContract  //TImplementation should either implement or inherit from TContract
        {
            Type _input_type = typeof(TContract);

            _register<TConcrete>(_input_type, dependencyProvider, null, true);

        }

        public void Register<TConcrete>( IMappingProvider dependencyProvider) where TConcrete : class  //TImplementation should either implement or inherit from TContract
        {
            Type _input_type = typeof(TConcrete); //Both input and concrete type are same.

            _register<TConcrete>(_input_type, dependencyProvider, null, false);
        }

        #endregion

        #region Resolution Methods

        public T Resolve<T>(GenerateNewInstance instance_level = GenerateNewInstance.None)
        {
            var _obj = Resolve(typeof(T), instance_level);
            return (T)_obj;
        }
        public object Resolve(Type input_type, GenerateNewInstance instance_level = GenerateNewInstance.None)
        {
            return _resolve(null,input_type,null, null, instance_level, InjectionTarget.All);
        }
        public T Resolve<T>(IMappingProvider dependency_provider, GenerateNewInstance instance_level = GenerateNewInstance.TargetOnly)
        {
            var _obj = Resolve(typeof(T), dependency_provider, instance_level);
            return (T)_obj;
        }
        public object Resolve(Type input_type, IMappingProvider dependency_provider, GenerateNewInstance instance_level = GenerateNewInstance.TargetOnly)
        {
            if (instance_level == GenerateNewInstance.None)
            { instance_level = GenerateNewInstance.TargetOnly; }
            return _resolve(null, input_type, null, dependency_provider, instance_level, InjectionTarget.All);
        }

        #endregion
        public DIContainer() 
        {
            overwrite_if_registered = false;
            ignore_if_registered = false; 
        }
    }
}
