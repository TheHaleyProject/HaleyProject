using System;

namespace Haley.Models
{
    [AttributeUsage(AttributeTargets.Constructor,AllowMultiple =false)]
    public class HaleyIgnoreAttribute : Attribute
    {
        public string name { get; set; }
        public HaleyIgnoreAttribute() { }
    }
}
