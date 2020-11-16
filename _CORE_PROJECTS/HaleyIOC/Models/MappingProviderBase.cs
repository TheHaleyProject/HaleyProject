using System;
using System.Collections.Concurrent;
using Haley.Abstractions;
using Haley.Enums;

namespace Haley.Models
{
    public class MappingProviderBase : IMappingProvider
    {
        public MappingProviderBase() { _mappings = new ConcurrentDictionary<string, (object concrete_instance, InjectionTarget _target)>(); }

        #region ATTRIBUTES
        //Add parameter or property values.
        //Key = $contractParentType.$contractType.$contractName
        public ConcurrentDictionary<string, (object concrete_instance, InjectionTarget _target)> _mappings { get; }
        #endregion

        #region Private Methods
        private string _getKey(string contract_name, Type contract_type, Type contract_parent)
        {
            //Key = $contractParentType.$contractType.$contractName
            string _key = "";
            if (contract_parent != null)
            {
                _key = $@"{contract_parent.ToString()}";
            }

            if (contract_type != null)
            {
                if (!string.IsNullOrEmpty(_key)) _key += "##";
                _key += $@"{contract_type.ToString()}";
            }

            if (!string.IsNullOrEmpty(contract_name))
            {
                if (!string.IsNullOrEmpty(_key)) _key += "##";
                _key += contract_name;
            }
            return _key;
        }

        private (object concrete_instance, InjectionTarget target) _getValue(string _key)
        {
            (object concrete_instance, InjectionTarget _target) output_tuple = (null, InjectionTarget.All);
            _mappings.TryGetValue(_key, out output_tuple);
            return output_tuple;
        }
        #endregion

        #region Add
        public bool Add<TConcrete>(string contract_name, TConcrete concrete_instance, Type contract_parent = null, InjectionTarget target = InjectionTarget.All)
        {
            return Add(contract_name, concrete_instance, typeof(TConcrete), contract_parent, target); 
        }

        public bool Add<TContract, TConcrete>(TConcrete concrete_instance, Type contract_parent = null, InjectionTarget target = InjectionTarget.All) where TConcrete : TContract
        {
            return Add(null, concrete_instance, typeof(TContract), contract_parent: contract_parent, target: target); //Here key is null, because, it should take $parentType$.$instanceType$ as key.
        }

        public bool Add(string contract_name, object concrete_instance, Type contract_type = null, Type contract_parent = null, InjectionTarget target = InjectionTarget.All)
        {
            var key = _getKey(contract_name, contract_type ?? concrete_instance.GetType(), contract_parent);
            return _mappings.TryAdd(key, (concrete_instance, target));
        }

        #endregion

        #region Remove
        public bool Remove(string contract_name, Type contract_type = null, Type contract_parent = null)
        {
            //For this concrete type is the contract type.
            (object concrete_instance, InjectionTarget _target) _removed_tuple = (null, InjectionTarget.All);
            var key = _getKey(contract_name, contract_type, contract_parent);
            return _mappings.TryRemove(key, out _removed_tuple);
        }

        public bool Remove<T>(Type contract_parent = null)
        {
            return Remove(null, typeof(T), contract_parent);
        }

        public bool Remove(Type contract_type, Type contract_parent = null)
        {
            return Remove(null, contract_type, contract_parent);
        }
        #endregion

        #region Resolve

        public (object concrete_instance, InjectionTarget injection) Resolve<TContract>(string contract_name =null,  Type contract_parent = null)
        {
            return Resolve(typeof(TContract),contract_name,contract_parent);
        }

        public (object concrete_instance, InjectionTarget injection) Resolve(Type contract_type, string contract_name = null, Type contract_parent = null)
        {
            (object concrete_instance, InjectionTarget target) _output_tuple = (null, InjectionTarget.All);
            var key = _getKey(contract_name, contract_type, contract_parent);

            //First try to get the value for whole key
            _mappings.TryGetValue(key, out _output_tuple);
            if (_output_tuple.concrete_instance == null)
            {
                //Try to check if we can get value for name
                if (!string.IsNullOrEmpty(contract_name))
                {
                    _mappings.TryGetValue(contract_name, out _output_tuple);
                }
            }
            return _output_tuple;
        }
        #endregion


    }
}
