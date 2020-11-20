using System;
using System.Linq;

namespace Haley.Enums
{
    public enum RegisterMode
    {
        Singleton,
        Transient
    }

    public enum ResolveMode
    {
        AsRegistered,
        Transient
    }
    public enum TransientCreationLevel
    {
        None,
        Current,
        CurrentWithDependencies,
        CascadeAll
    }

    public enum InjectionTarget
    {
        All,
        Constructor,
        Property
    }

    public enum MappingLevel
    {
        None,
        Current,
        CurrentWithDependencies,
        CascadeAll
    }
}
