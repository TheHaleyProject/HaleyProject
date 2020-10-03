using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Haley.Enums;

namespace Haley.Abstractions
{
    public interface IAxiomPropGeneric<T>
    {
        Func<T,string, object> getPropertyDelegate { get; }
    }
}
