using System;

namespace Haley.Models
{
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property,AllowMultiple =false)]
    public class HaleyInjectAttribute : Attribute
    {
        public string name { get; set; }
        public HaleyInjectAttribute() { }
        public HaleyInjectAttribute(string _name) { name = _name; }
    }
}
