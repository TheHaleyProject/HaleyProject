using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.MVVM.Interfaces
{
    public interface IHaleyControlContainer
    {
        void register<ViewModelType, ControlType>()
            where ControlType : IHaleyControl
            where ViewModelType : IHaleyControlVM, new(); 

        void register<ViewModelType, ControlType>(Enum @enum)
           where ViewModelType : IHaleyControlVM,new()
           where ControlType : IHaleyControl;

        IHaleyControl obtainControl<ViewModelType>(ViewModelType InputViewModel)
        where ViewModelType : IHaleyControlVM;

        IHaleyControl obtainControl(object InputViewModel, Enum @enum);

        object obtainVM(Enum @enum);
    }
}
