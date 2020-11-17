using System;
using Haley.Enums;

namespace Haley.Abstractions
{
    public interface IHaleyDIContainer
    {
        bool ignore_if_registered { get; set; }
        bool overwrite_if_registered { get; set; }

        #region Validation Methods
        (bool status, Type registered_type, string message,bool is_singleton) checkIfRegistered(Type contract_type);
        (bool status, Type registered_type, string message, bool is_singleton) checkIfRegistered<TContract>();
        (bool status, Type registered_type, string message, bool is_singleton) checkIfRegistered(string key);
        #endregion

        #region Register Methods
        void Register<TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        void Register<TConcrete>(TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        void Register<TConcrete>(IMappingProvider dependencyProvider,MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton ) where TConcrete : class;
        void Register<TContract, TConcrete>(TConcrete instance, RegisterMode mode = RegisterMode.Singleton ) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        void Register<TContract, TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        void Register<TContract, TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        #endregion

        #region TryRegister Methods
        bool TryRegister<TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        bool TryRegister<TConcrete>(TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        bool TryRegister<TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        bool TryRegister<TContract, TConcrete>(TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        bool TryRegister<TContract, TConcrete>(RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        bool TryRegister<TContract, TConcrete>(IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        #endregion

        #region RegisterWithKey Methods
        bool RegisterWithKey<TConcrete>(string override_key, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        bool RegisterWithKey<TConcrete>(string override_key, TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        bool RegisterWithKey<TConcrete>(string override_key, IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class;
        bool RegisterWithKey<TContract, TConcrete>(string override_key, TConcrete instance, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        bool RegisterWithKey<TContract, TConcrete>(string override_key, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        bool RegisterWithKey<TContract, TConcrete>(string override_key, IMappingProvider dependencyProvider, MappingLevel mapping_level, RegisterMode mode = RegisterMode.Singleton) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        #endregion

        #region Resolve Methods
        T Resolve<T>(ResolveMode mode = ResolveMode.Default);
        object Resolve(Type contract_type, ResolveMode mode = ResolveMode.Default);
        object Resolve(string override_key, ResolveMode mode = ResolveMode.Default);
        #endregion

        #region TryResolve Methods
        bool TryResolve(Type contract_type, out object concrete_instance, ResolveMode mode = ResolveMode.Default);
        bool TryResolve(string override_key, out object concrete_instance, ResolveMode mode = ResolveMode.Default);
        #endregion

        #region ResolveTransient Methods
        T ResolveTransient<T>(TransientCreationLevel transient_level);
        object ResolveTransient(Type contract_type, TransientCreationLevel transient_level);
        object ResolveTransient(string override_key, TransientCreationLevel transient_level);
        T ResolveTransient<T>(IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithProperties);
        object ResolveTransient(Type contract_type, IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithProperties);
        object ResolveTransient(string override_key, IMappingProvider mapping_provider, MappingLevel mapping_level = MappingLevel.CurrentWithProperties);
        #endregion
    }
}
