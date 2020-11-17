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
        Default,
        Transient
    }
    public enum TransientCreationLevel
    {
        None,
        Current,
        CurrentWithProperties,
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
        CurrentWithProperties,
        CascadeAll
    }
}
