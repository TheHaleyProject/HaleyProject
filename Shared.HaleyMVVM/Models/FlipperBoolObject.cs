using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Haley.MVVM.Interfaces;

namespace Haley.MVVM.Models
{
    /// <summary>
    /// Can be used to identify IsSelected for custom objects.
    /// </summary>
    public class FlipperBoolObject : ChangeNotifier, ICheckedItem
    {
        private bool _is_checked;
        public bool is_checked
        {
            get { return _is_checked; }
            set { _is_checked = value; onPropertyChanged(); }
        }

        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; onPropertyChanged(); }
        }

        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; onPropertyChanged(); }
        }

        public FlipperBoolObject() { _is_checked = false; }
    }
}
