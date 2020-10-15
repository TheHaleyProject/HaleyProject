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

        #region Register Methods
        /// <summary>
        /// To register a given abstractable type or interface and its implementation
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <param name="is_singleton"></param>
        void Register<TContract, TConcrete>(TConcrete instance = null) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        void Register<TConcrete>(TConcrete instance = null) where TConcrete : class;
        void Register<TContract, TConcrete>(IMappingProvider dependencyProvider) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        void Register<TConcrete>(IMappingProvider dependencyProvider) where TConcrete : class;
        #endregion


        #region Resolution Methods
        T Resolve<T>(GenerateNewInstance instance_level = GenerateNewInstance.None);
        object Resolve(Type input_type, GenerateNewInstance instance_level = GenerateNewInstance.None);

        T Resolve<T>(IMappingProvider dependency_provider, GenerateNewInstance instance_level = GenerateNewInstance.TargetOnly);
        object Resolve(Type input_type, IMappingProvider dependency_provider, GenerateNewInstance instance_level = GenerateNewInstance.TargetOnly);
        #endregion

    }
}
