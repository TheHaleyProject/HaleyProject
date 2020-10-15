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
        /// <summary>
        /// To register a given abstractable type or interface and its implementation
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <param name="is_singleton"></param>
        void Register<TContract, TConcrete>(TConcrete instance = null, IMappingProvider dependencyProvider = null) where TConcrete : class, TContract;  //TImplementation should either implement or inherit from TContract
        void Register<TConcrete>(TConcrete instance = null, IMappingProvider dependencyProvider = null) where TConcrete : class;  
        T Resolve<T>(IMappingProvider dependency_provider = null, GenerateNewInstance instance_level = GenerateNewInstance.None);
        object Resolve(Type input_type, IMappingProvider dependency_provider = null, GenerateNewInstance instance_level = GenerateNewInstance.None);
    }
}
