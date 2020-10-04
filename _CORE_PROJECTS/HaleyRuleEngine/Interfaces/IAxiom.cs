using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Haley.Enums;

namespace Haley.Abstractions
{
    public interface IAxiom
    {
        string id { get;}
        string description { get; }
        object value { get;}
        AxiomOperator @operator { get; }
        bool ignore_case { get; set; }
    }
}
