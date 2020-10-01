using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Haley.Abstractions;

namespace Haley.Models
{
    /// <summary>
    /// It should be in the form of Dictionary<Object, bool> 
    /// </summary>
    public class FlipperBoolDictionary : ChangeNotifier
    {
        private object _key;
        public object key
        {
            get { return _key; }
            set { _key = value; onPropertyChanged(); }
        }

        private object _value;
        public object value
        {
            get { return _value; }
            set { _value = value; onPropertyChanged(); }
        }

        public FlipperBoolDictionary() { }
    }
    
}
