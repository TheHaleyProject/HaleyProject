using System;
using System.Collections.Generic;
using System.Text;

namespace Haley.MVVM
{
    [AttributeUsage(AttributeTargets.Constructor,AllowMultiple =false)]
    public class HaleyInjectAttribute : Attribute
    {
        public string name { get; set; }
        public HaleyInjectAttribute() { }
    }
}
