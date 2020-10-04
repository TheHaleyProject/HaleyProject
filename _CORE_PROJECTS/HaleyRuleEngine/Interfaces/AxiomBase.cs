using Haley.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;

namespace Haley.Abstractions
{
    public abstract class AxiomBase :IAxiom
    {
        public string id { get; }
        public string description { get; }
        public object value { get; }
        public AxiomOperator @operator { get; }
        public bool ignore_case { get; set; }
        public AxiomBase(AxiomOperator _operator, object _value, string _description = null) 
        {
            id = Guid.NewGuid().ToString();
            @operator = _operator;
            value = _value;
            description = _description;
            ignore_case = true;
        }
    }
}
