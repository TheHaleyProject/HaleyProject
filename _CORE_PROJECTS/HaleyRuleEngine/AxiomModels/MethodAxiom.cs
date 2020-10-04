using System;
using System.Collections.Generic;
using System.Text;
using Haley.Abstractions;
using Haley.Enums;

namespace Haley.Models
{
    public class MethodAxiom<T> : BinaryAxiom
    {
        private string method_name;
        public AxiomAction<T> action { get; }
        public object[] parameters { get; set; }
        public override string ToString()
        {
            StringBuilder strbldr = new StringBuilder();
            strbldr.Append($@"The custom validation with method {method_name}.");
            return strbldr.ToString();
        }
        public MethodAxiom(AxiomAction<T> _action,string _method_name, string _description = null,params object[] _parameters) : base(AxiomOperator.Empty,null,_description)
        { 
            action = _action; 
            method_name = _method_name;
            parameters = _parameters;
        }
    }
}
