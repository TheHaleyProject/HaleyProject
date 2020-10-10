using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Models;
using Haley.MVVM;
using Haley.Abstractions;
using DevelopmentWPF.ViewModels;
using System.ComponentModel;
using Haley.Events;

namespace DevelopmentWPF
{
    public class CoreVM : ChangeNotifier, IHaleyWindowVM
    {
        private TestApp _controlEnum;
        public TestApp controlEnum
        {
            get { return _controlEnum; }
            set { _controlEnum = value; onPropertyChanged(); }
        }

        private IHaleyControlVM _current_viewModel;
        public IHaleyControlVM current_viewModel
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

        public event EventHandler<FrameClosingEventArgs> OnWindowsClosed;

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
            //this.PropertyChanged += CoreVM_PropertyChanged;
        }

        private void CoreVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(controlEnum))
            {
                var _vm = ContainerStore.Singleton.controls.generateViewModel(controlEnum,false);
                current_viewModel = _vm;
            }
        }
    }
}
