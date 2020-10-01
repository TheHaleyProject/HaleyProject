using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Flipper.MVVM.Interfaces;
using Haley.Flipper.MVVM.Models;

namespace DevelopmentWPF.ViewModels
{
    public class VMMain : ChangeNotifierModel, IFlipperViewModel
    {
        public bool? event_result { get ; set ; }
        public void seed(object parameter)
        {

        }

        private bool _ischecked;
        public bool ischecked
        {
            get { return _ischecked; }
            set { _ischecked = value; onPropertyChanged(); }
        }

        private string _content;
        public string content
        {
            get { return _content; }
            set { _content = value; onPropertyChanged(); }
        }

        public VMMain() { ischecked = true; content = $@"this is from {nameof(VMMain)}"; }
    }
}
