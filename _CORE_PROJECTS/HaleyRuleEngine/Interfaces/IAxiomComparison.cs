using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Haley.Enums;

namespace Haley.Abstractions
{
    public interface IAxiomComparison : IAxiomProp
    {
      string secondary_property { get; }
}
}
