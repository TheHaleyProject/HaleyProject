using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Haley.Enums;

namespace Haley.Abstractions
{
    public interface IHaleyDIContainer
    {
        bool ignore_if_registered { get; set; }
        bool overwrite_if_registered { get; set; }
        (bool status, Type registered_type, string message) checkIfRegistered(Type input_type);
        (bool status, Type registered_type, string message) checkIfRegistered(string key);

        #region Register Methods
        void Register<TConcrete>(string custom_key=null) where TConcrete : class;
        void Register<TConcrete>(TConcrete instance,string custom_key = null) where TConcrete : class;
        void Register<TConcrete>(IMappingProvider dependencyProvider, string custom_key = null) where TConcrete : class;
        void Register<TContract, TConcrete>(TConcrete instance, string custom_key = null) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        void Register<TContract, TConcrete>(string custom_key = null) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        void Register<TContract, TConcrete>(IMappingProvider dependencyProvider, string custom_key = null) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        #endregion

        #region Resolution Methods
        T Resolve<T>(GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None);
        object Resolve(Type input_type, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None);
        object Resolve(string custom_key, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None);
        T Resolve<T>(IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.TargetObjectWithParameters);
        object Resolve(Type input_type, IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.TargetObjectWithParameters);
        object Resolve(string custom_key, IMappingProvider mapping_provider, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.TargetObjectWithParameters);
        #endregion

    }
}
