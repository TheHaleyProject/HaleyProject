using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Flipper.MVVM.Interfaces;
using System.ComponentModel;
using Haley.Flipper.MVVM.Models;

namespace DevelopmentWPF.ViewModels
{
    public class VMSubMain : ChangeNotifierModel, IFlipperViewModel
    {
        public bool? event_result { get; set; }
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

        public VMSubMain() { ischecked = false; content = $@"this is from {nameof(VMSubMain)}"; }
    }
}
