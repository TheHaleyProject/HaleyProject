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
    public abstract class ChangeNotifierModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void onPropertyChanged([CallerMemberName] string propname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propname));
        }
        public ChangeNotifierModel() { }
    }

    /// <summary>
    /// It should be in the form of Dictionary<Object, bool> 
    /// </summary>
    public class FlipperBoolDictionary : ChangeNotifierModel
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

    /// <summary>
    /// Can be used to identify IsSelected for custom objects.
    /// </summary>
    public class FlipperBoolObject : ChangeNotifierModel, ICheckedItem
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
