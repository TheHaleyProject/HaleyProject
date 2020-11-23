using System;
using Haley.Enums;
using Haley.Models;

namespace Haley.Abstractions
{
    public interface IHaleyDIContainer
    {
        bool ignore_if_registered { get; set; }
        bool overwrite_if_registered { get; set; }

        #region Validation Methods
        (bool status, Type registered_type, string message,RegisterMode mode) checkIfRegistered(Type contract_type,string priority_key);
        (bool status, Type registered_type, string message, RegisterMode mode) checkIfRegistered<TContract>(string priority_key);
        (bool status, Type registered_type, string message, RegisterMode mode) checkIfRegistered(KeyBase key);
        #endregion

        #region Register Methods
        void Register<TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        void Register<TConcrete>(TConcrete instance) where TConcrete : class;
        void Register<TConcrete>(IMappingProvider dependencyProvider,MappingLevel mapping_level ) where TConcrete : class;
        void Register<TContract, TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        void Register<TContract, TConcrete>(TConcrete instance) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        void Register<TContract, TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        #endregion

        #region TryRegister Methods
        bool TryRegister<TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        bool TryRegister<TConcrete>(TConcrete instance) where TConcrete : class;
        bool TryRegister<TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class;
        bool TryRegister<TContract, TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        bool TryRegister<TContract, TConcrete>(TConcrete instance) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        bool TryRegister<TContract, TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        #endregion

        #region RegisterWithKey Methods
        bool RegisterWithKey<TConcrete>(string priority_key, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        bool RegisterWithKey<TConcrete>(string priority_key, TConcrete instance) where TConcrete : class;
        bool RegisterWithKey<TConcrete>(string priority_key, IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class;
        bool RegisterWithKey<TContract, TConcrete>(string priority_key, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        bool RegisterWithKey<TContract, TConcrete>(string priority_key, TConcrete instance) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        bool RegisterWithKey<TContract, TConcrete>(string priority_key, IMappingProvider dependencyProvider, MappingLevel mapping_level) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        #endregion

        #region Resolve Methods
        T Resolve<T>(ResolveMode mode = ResolveMode.AsRegistered);
        T Resolve<T>(string priority_key, ResolveMode mode = ResolveMode.AsRegistered);
        object Resolve(Type contract_type, ResolveMode mode = ResolveMode.AsRegistered);
        object Resolve(string priority_key, Type contract_type, ResolveMode mode = ResolveMode.AsRegistered);
        T Resolve<T>(IMappingProvider mapping_provider, ResolveMode mode = ResolveMode.AsRegistered, bool currentOnlyAsTransient = false);
        object Resolve(Type contract_type, IMappingProvider mapping_provider, ResolveMode mode = ResolveMode.AsRegistered, bool currentOnlyAsTransient = false);
        object Resolve(string priority_key, Type contract_type, IMappingProvider mapping_provider, ResolveMode mode = ResolveMode.AsRegistered, bool currentOnlyAsTransient = false);
        #endregion

        #region TryResolve Methods
        bool TryResolve(Type contract_type, out object concrete_instance, ResolveMode mode = ResolveMode.AsRegistered);
        bool TryResolve(string priority_key, Type contract_type, out object concrete_instance, ResolveMode mode = ResolveMode.AsRegistered);
        bool TryResolve(Type contract_type, IMappingProvider mapping_provider,out object concrete_instance, ResolveMode mode = ResolveMode.AsRegistered, bool currentOnlyAsTransient = false);
        bool TryResolve(string priority_key, Type contract_type, IMappingProvider mapping_provider,out object concrete_instance, ResolveMode mode = ResolveMode.AsRegistered, bool currentOnlyAsTransient = false);
        #endregion

        #region ResolveTransient Methods
        T ResolveTransient<T>(TransientCreationLevel transient_level);
        T ResolveTransient<T>(string priority_key, TransientCreationLevel transient_level);
        object ResolveTransient(Type contract_type, TransientCreationLevel transient_level);
        object ResolveTransient(string priority_key, Type contract_type,TransientCreationLevel transient_level);
        T ResolveTransient<T>(IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithDependencies);
        object ResolveTransient(Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithDependencies);
        object ResolveTransient(string priority_key, Type contract_type,IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithDependencies);
        #endregion
    }
}
