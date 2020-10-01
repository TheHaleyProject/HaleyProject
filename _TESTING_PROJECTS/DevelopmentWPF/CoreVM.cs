using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Flipper.MVVM.Models;
using Haley.Flipper.MVVM.IOC;
using Haley.Flipper.MVVM.Interfaces;
using DevelopmentWPF.ViewModels;
using System.ComponentModel;

namespace DevelopmentWPF
{
    public class CoreVM : ChangeNotifierModel
    {
        private TestApp _controlEnum;
        public TestApp controlEnum
        {
            get { return _controlEnum; }
            set { _controlEnum = value; onPropertyChanged(); }
        }

        private IFlipperViewModel _current_viewModel;
        public IFlipperViewModel current_viewModel
        {
            get { return _current_viewModel; }
            set { _current_viewModel = value; onPropertyChanged(); }
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

        public CoreVM()
        {
            ischecked = false;
            content = $@"this is from {nameof(CoreVM)}";
            current_viewModel = null;
            this.PropertyChanged += CoreVM_PropertyChanged;
        }

        private void CoreVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(controlEnum))
            {
                current_viewModel = (IFlipperViewModel) App.ControlIOCService.obtainVMInstance(controlEnum);
            }
        }
    }
}
