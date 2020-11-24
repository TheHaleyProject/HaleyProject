using System;
using System.Collections.Generic;
using System.Text;
using Haley.Enums;
using Haley.Abstractions;
using Haley.Utils;
using System.Net;

namespace Haley.Models
{
    public class BinaryAxiom : AxiomBase
    {
        public override string ToString()
        {
            StringBuilder strbldr = new StringBuilder();
            strbldr.Append("The input ");
            strbldr.Append($@"{ @operator.getDescription()} {value}.)");
            return strbldr.ToString();
        }

        public BinaryAxiom(AxiomOperator _operator, object _value, string _description = null) :base(_operator,_value,_description)
        { 
        }
    }
}
