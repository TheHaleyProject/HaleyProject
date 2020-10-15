using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System.Reflection;
using System.Windows;
using Microsoft.Xaml.Behaviors.Core;
using System.Collections.Concurrent;
using Haley.Enums;

namespace Haley.Abstractions
{
   public interface IMappingProvider
    {
        //Readonly dictionary
        ConcurrentDictionary<string, (object _instance, InjectionTarget _target)> _mappings { get; }

        #region Add
        bool Add(string name, object instance,Type parent = null , InjectionTarget target = InjectionTarget.All);
        bool Add(object instance, Type parent = null, InjectionTarget target = InjectionTarget.All);
        bool Add<T>(T instance, Type parent = null ,InjectionTarget target = InjectionTarget.All);
        #endregion

        #region Remove
        bool Remove(string name, Type instance_type =null, Type parent = null);
        bool Remove<T>(Type parent = null);
        bool Remove(Type instance_type, Type parent = null);
        #endregion

        #region Resolve
        (T instance,InjectionTarget target) Resolve<T>(Type parent = null);
        (object instance, InjectionTarget target) Resolve(string name, Type instance_type = null,Type parent = null);
        (object instance, InjectionTarget target) Resolve(Type instance_type, Type parent = null);
        #endregion
    }
}
