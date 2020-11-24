using System;
using System.Collections.Generic;
using System.Text;
using Haley.Abstractions;
using Haley.Enums;
using Haley.Utils;

namespace Haley.Models
{
    public class PropertyAxiom : BinaryAxiom, IAxiomProp
    {
        public string primary_property { get; }

        public override string ToString()
        {
            StringBuilder strbldr = new StringBuilder();
            strbldr.Append($@"The value of the property ""{primary_property}"" ");
            strbldr.Append($@"{ @operator.getDescription()} {value}.)");
            return strbldr.ToString();
        }
        public PropertyAxiom(AxiomOperator _operator,string _property_name,  object _value, string _description = null) : base(_operator, _value, _description)
        {
            primary_property = _property_name;
        } 
    }
    public class PropertyAxiom<T> : PropertyAxiom, IAxiomPropGeneric<T>
    {
        public Func<T, string, object> getPropertyDelegate { get; }
   
        public PropertyAxiom(AxiomOperator _operator, string _property_name, object _value, Func<T, string, object> _getPropertyDelegate,  string _description = null) : base(_operator, _property_name, _value, _description) 
        {
            getPropertyDelegate = _getPropertyDelegate;
        }
    }
}
