using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.MVVM.Interfaces
{
    public interface ICheckedItem : INotifyPropertyChanged
    {
        bool is_checked { get; set; }
        String name { get; set; }
        int id { get; set; }
    }

}
