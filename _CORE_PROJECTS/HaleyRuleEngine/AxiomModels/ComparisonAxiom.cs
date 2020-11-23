using System;
using System.Collections.Generic;
using System.Text;
using Haley.Abstractions;
using Haley.Utils;
using Haley.Enums;

namespace Haley.Models
{
    public class ComparisonAxiom: BinaryAxiom, IAxiomComparison
    {
        public string primary_property { get; }
        public string secondary_property { get; }
        public override string ToString()
        {
            StringBuilder strbldr = new StringBuilder();
            strbldr.Append($@"The value of the property ""{primary_property}""");
            strbldr.Append($@" {@operator.getDescription()} )");
            strbldr.Append($@"the value of the property ""{secondary_property}"".");
            return strbldr.ToString();
        }
        public ComparisonAxiom(AxiomOperator _operator, string _primary_property, string _secondary_property, string _description = null) : base(AxiomOperator.Empty, null, _description)
        {
            primary_property = _primary_property;
            secondary_property = _secondary_property;
        }
    }
    public class ComparisonAxiom<T> : ComparisonAxiom, IAxiomPropGeneric<T>
    {
        public Func<T,string, object> getPropertyDelegate { get; }
        public ComparisonAxiom(AxiomOperator _operator, string _primary_property, string _secondary_property, Func<T, string, object> _getPropertyDelegate, string _description = null) : base(_operator, _primary_property, _secondary_property,_description)
        {
            getPropertyDelegate = _getPropertyDelegate;
        }
    }
}
