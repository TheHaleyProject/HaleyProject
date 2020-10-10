using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using Haley.Events;
using System.Reflection;
using System.Configuration;
using System.CodeDom;
using System.Collections.Concurrent;

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

        #region Public Helper Methods
        public (bool status, Type registered_type, string message) checkIfRegistered(Type input_type)
        {
            //Check if the provided input is present in any of the repository. If yes, then return error stating that it is already registered.
            (Type _type, object _inst) _registered_tuple = (null,null);
            string _message = null;
            bool _is_registered = false;

            //CHECK ABSTRACT SINGLETON REPOSITORY
            if (abstract_type_mappings.ContainsKey(input_type))
            {
                _is_registered = true;
                abstract_type_mappings.TryGetValue(input_type, out _registered_tuple);
                _message = $@"The {input_type} is already registered against the type {_registered_tuple._type}.";
            }

            //CHECK CONCRETE SINGLETON REPOSITORY
            if (concrete_mappings.ContainsKey(input_type))
            {
                _is_registered = true;
                _registered_tuple = (input_type, null); //Not returning the value.
                _message = $@"The concrete type: {input_type} is already registered.";
            }

            return (_is_registered, _registered_tuple._type, _message);
        }

        #endregion

        #region Private Methods
        private object _createInstance(Type concrete_type)
        {
            _validateConcreteType(concrete_type);
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
                constructor = constructors[0];
            }

            //Resolve the param arugments for the constructor.
            ParameterInfo[] constructor_params = constructor.GetParameters();
            //If parameter less construction, return a new creation.
            if (constructor_params.Length == 0)
            {
                return Activator.CreateInstance(concrete_type);
            }

            List<object> parameters = new List<object>(constructor_params.Length);
            foreach (ParameterInfo pinfo in constructor_params)
            {
                Type _paramtype = pinfo.ParameterType;
                if (_paramtype == typeof(string) || _paramtype.IsValueType || _paramtype.IsByRef)
                {
                    throw new ArgumentException($@"Value type dependency error. The constructor for {concrete_type.Name} depends on a value type {pinfo.Name}. Constructor cannot have value type parameters.");
                }
                //recursively resolve the references
                parameters.Add(Resolve(pinfo.ParameterType));
            }
            return constructor.Invoke(parameters.ToArray());
        }
        private bool _validateExistence(Type key, Type value)
        {
            var _status = checkIfRegistered(key);
            //If registered and also ignore
            if (_status.status)
            {
                if (!ignore_if_registered)
                {
                    //Throw the error, only if you should not ignore the registered status.
                    if (_status.registered_type != value)
                    {
                        throw new ArgumentException(_status.message);
                    }
                }
            }
            return _status.status; //Returns if registered.
        }
        private object _getObject(Type input_type)
        {
            if (input_type == null) throw new ArgumentNullException(nameof(input_type));
            object _output = null;
            //If any singleton repository contains the type, return the value.
            if (abstract_type_mappings.ContainsKey(input_type))
            {
                (Type _type, object _inst) _registered_tuple = (null, null);
                abstract_type_mappings.TryGetValue(input_type, out _registered_tuple);
                _output = _registered_tuple._inst; //Sending the singleton instance value.
            }

            if (concrete_mappings.ContainsKey(input_type))
            {
                concrete_mappings.TryGetValue(input_type, out _output);
            }

            if (_output != null) return _output;
            return _createInstance(input_type);
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
        public void Register<TContract, TConcrete>(TConcrete instance = null) where TConcrete : class, TContract  //TImplementation should either implement or inherit from TContract
        {
            Type _key = typeof(TContract);
            Type _value_type = typeof(TConcrete);
            
            //Validate
            _validateConcreteType(_value_type);

            //Generate or add instance
            if (instance == null)
            {
                instance = (TConcrete)_createInstance(_value_type); //Create instance resolving all dependencies
            }
            (Type _type, object _inst) _new_tuple = (_value_type, instance);

            //Validate existing presence
            if (_validateExistence(_key, _value_type))
            {
                if (!overwrite_if_registered) return;
                
                (Type _type, object _inst) _current_tuple;
                abstract_type_mappings.TryGetValue(_key, out _current_tuple);
                abstract_type_mappings.TryUpdate(_key,_new_tuple, _current_tuple); //Remember to assign the instance
            }
            else
            {
                abstract_type_mappings.TryAdd(_key, _new_tuple);
            }
        }
        public void Register<TConcrete>(TConcrete instance = null) where TConcrete : class  //TImplementation should either implement or inherit from TContract
        {
            Type _key = typeof(TConcrete);
            _validateConcreteType(_key);

            if (instance == null)
            {
                instance = (TConcrete)_createInstance(typeof(TConcrete)); //Create instance resolving all dependencies
            }

            if (_validateExistence(_key, _key))
            {
                if (!overwrite_if_registered) return;
                object _current_value;
                concrete_mappings.TryGetValue(_key, out _current_value);
                concrete_mappings.TryUpdate(_key, instance, _current_value); //Remember to assign the instance
            }
            else
            {
                concrete_mappings.TryAdd(_key, instance);
            }
        }

        #endregion

        #region Resolution Methods
       
        public T Resolve<T>(bool generate_new_instance = false)
        {
            var _obj = Resolve(typeof(T), generate_new_instance);
            return (T) _obj;
        }
        public object Resolve(Type input_type,bool generate_new_instance = false)
        {
            if (generate_new_instance) return _createInstance(input_type);
            var _obj = _getObject(input_type);
            return _obj;
        }
        #endregion

        public DIContainer() 
        {
            overwrite_if_registered = false;
            ignore_if_registered = false; 
        }
    }
}
