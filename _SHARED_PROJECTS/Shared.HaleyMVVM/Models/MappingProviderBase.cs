using System;
using System.Collections.Concurrent;
using Haley.Abstractions;
using Haley.Enums;

namespace Haley.Models
{
    public class MappingProviderBase : IMappingProvider
    {
        #region ATTRIBUTES
        //Add parameter or property values.
        //Key = $parentType$.$targetType$.$key$
        public ConcurrentDictionary<string, (object _instance, InjectionTarget _target)> _mappings { get; }
        #endregion

        #region Private Methods
        private string _getKey(string name, Type instance_type, Type parent)
        {
            string _key = "";
            if (parent != null)
            {
                _key = $@"{parent.ToString()}";
            }

            if (instance_type != null)
            {
                if (!string.IsNullOrEmpty(_key)) _key += "##";
                _key += $@"{instance_type.ToString()}";
            }

            if (!string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(_key)) _key += "##";
                _key += name;
            }

            return _key;
        }

        private (object instance, InjectionTarget target) _getValue(string _key)
        {
            return (null, InjectionTarget.All);
        }
        #endregion

        #region Add
        public bool Add<TConcrete>(string name, TConcrete instance, Type parent = null, InjectionTarget target = InjectionTarget.All)
        {
            return Add(name, instance,typeof(TConcrete), parent, target); //Here key is null, because, it should take $parentType$.$instanceType$ as key.
        }
        public bool Add<TContract, TConcrete>(TConcrete instance, Type parent = null, InjectionTarget target = InjectionTarget.All) where TConcrete : TContract
        {
            return Add(null,instance,typeof(TContract), parent: parent,target: target); //Here key is null, because, it should take $parentType$.$instanceType$ as key.
        }

        public bool Add(string name, object instance, Type target_type =null, Type parent = null, InjectionTarget target = InjectionTarget.All)
        {
            var key = _getKey(name,target_type ?? instance.GetType(),parent);
            return _mappings.TryAdd(key, (instance,target));
        }
        #endregion

        #region Remove
        public bool Remove<T>(Type parent = null)
        {
            return Remove(null,typeof(T), parent);
        }

        public bool Remove(Type instance_type, Type parent = null)
        {
            return Remove(null,instance_type, parent);
        }
        public bool Remove(string name, Type instance_type = null, Type parent = null)
        {
            (object, InjectionTarget) _newtuple = (null, InjectionTarget.All);
            var key = _getKey(name,instance_type, parent);
            return _mappings.TryRemove(key, out _newtuple);
        }
        #endregion

        #region Resolve
        public (object instance, InjectionTarget target) Resolve(string name, Type instance_type = null, Type parent = null)
        {
            (object, InjectionTarget) _newtuple = (null, InjectionTarget.All);
            var key = _getKey(name,instance_type, parent);
            //First try to get the value for whole key
            _mappings.TryGetValue(key, out _newtuple);
            if (_newtuple.Item1 == null)
            {
                //Try to check if we can get value for name
                if (!string.IsNullOrEmpty(name))
                {
                    _mappings.TryGetValue(name, out _newtuple);
                    //Ensure that the retrieved output (ifany) is of instance type
                    if (_newtuple.Item1 == null)
                    {

                    }
                }
            }
            return _newtuple;
        }

        public (object instance, InjectionTarget target) Resolve(Type instance_type, Type parent = null)
        {
            return Resolve(null,instance_type, parent);
        }
        public virtual (T instance, InjectionTarget target) Resolve<T>(Type parent = null)
        {
            var _result = Resolve(null,typeof(T), parent);
            return ((T)_result.instance, _result.target);
        }
        
        #endregion

        public MappingProviderBase() {_mappings= new ConcurrentDictionary<string, (object _instance, InjectionTarget _target)>(); }
    }
}
