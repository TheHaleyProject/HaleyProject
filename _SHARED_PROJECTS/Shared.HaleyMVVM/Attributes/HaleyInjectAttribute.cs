using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.MVVM
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property,AllowMultiple =false)]
    public class HaleyInjectAttribute : Attribute
    {
        public string name { get; set; }
        public HaleyInjectAttribute() { }
        public HaleyInjectAttribute(string _name) { name = _name; }
    }
}
