using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using Haley.Enums;

namespace Haley.Abstractions
{
   public interface IMappingProvider
    {
        //Readonly dictionary
        ConcurrentDictionary<string, (object concrete_instance, InjectionTarget _target)> _mappings { get; }

        #region Add
        bool Add<TConcrete>(string contract_name, TConcrete concrete_instance,Type contract_parent = null , InjectionTarget target = InjectionTarget.All);
        bool Add<TContract,TConcrete>(TConcrete concrete_instance, Type contract_parent = null ,InjectionTarget target = InjectionTarget.All) where TConcrete:TContract;
        bool Add(string contract_name, object concrete_instance, Type contract_type = null, Type contract_parent = null, InjectionTarget target = InjectionTarget.All);
        #endregion

        #region Remove
        bool Remove(string contract_name, Type concrete_type =null, Type contract_parent = null);
        bool Remove<T>(Type contract_parent = null);
        bool Remove(Type concrete_type, Type contract_parent = null);
        #endregion

        #region Resolve
        (object concrete_instance, InjectionTarget injection) Resolve<TContract>(string contract_name = null, Type contract_parent = null);
        (object concrete_instance, InjectionTarget injection) Resolve(Type contract_type, string contract_name = null, Type contract_parent = null);
        #endregion
    }
}
