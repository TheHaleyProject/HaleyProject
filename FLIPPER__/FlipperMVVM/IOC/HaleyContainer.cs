using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.MVVM.Interfaces;
using Haley.MVVM.EventArguments;

namespace Haley.MVVM.IOC
{
    public class HaleyContainer
    {
        private IHaleyWindowContainer _window_container;
        private IHaleyControlContainer _control_container;

        public HaleyContainer() { _window_container = new WindowContainer(); _control_container = new ControlContainer(); }

        #region Control Container
        public IHaleyControl obtainControl<T>(T InputViewModel) where T : IHaleyControlVM
        {
            return _control_container.obtainControl(InputViewModel);
        }

        public IHaleyControl obtainControl(object InputViewModel, Enum @enum)
        {
            return _control_container.obtainControl(InputViewModel, @enum);
        }

        public object obtainControlVM(Enum @enum)
        {
            return _control_container.obtainVM(@enum);
        }

        public void registerControl<ViewModelType, ControlType>()
            where ViewModelType : IHaleyControlVM,new()
            where ControlType : IHaleyControl
        {
            _control_container.register<ViewModelType, ControlType>();
        }

        public void registerControl<ViewModelType, ControlType>(Enum @enum)
            where ViewModelType : IHaleyControlVM, new()
            where ControlType : IHaleyControl
        {
            _control_container.register<ViewModelType, ControlType>(@enum);
        }
       
        #endregion

        #region Window Container
        public void registerWindow<ViewModelType, ViewType>()
            where ViewModelType : IHaleyWindowVM, new()
            where ViewType : IHaleyWindow
        {
            _window_container.register<ViewModelType, ViewType>();
        }

        public void show<ViewModelType>(ViewModelType InputViewModel) where ViewModelType : IHaleyWindowVM
        {
            _window_container.show(InputViewModel);
        }

        public bool? showDialog<ViewModelType>(ViewModelType InputViewModel) where ViewModelType : IHaleyWindowVM
        {
            return _window_container.showDialog(InputViewModel);
        }
        #endregion
    }
}
