using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.Abstractions
{
    public interface IHaleyControlContainer
    {
        void register<ViewModelType, ControlType>(ViewModelType InputViewModel = null, bool use_vm_as_key = false)
            where ControlType : IHaleyControl
            where ViewModelType :class, IHaleyControlVM; 
        void register<ViewModelType, ControlType>(string key,ViewModelType InputViewModel = null)
           where ViewModelType :class, IHaleyControlVM
           where ControlType : IHaleyControl;
        void register<ViewModelType, ControlType>(Enum key, ViewModelType InputViewModel = null)
           where ViewModelType : class, IHaleyControlVM
           where ControlType : IHaleyControl;

        IHaleyControl obtainControl<ViewModelType>(ViewModelType InputViewModel = null, bool create_new_vm = false) where ViewModelType : class, IHaleyControlVM;
        IHaleyControl obtainControl(string key, object InputViewModel = null, bool create_new_vm = false);
        IHaleyControl obtainControl(Enum key, object InputViewModel = null, bool create_new_vm = false);

        object obtainVM(string key, bool create_new_vm = false);
        object obtainVM(Enum key, bool create_new_vm = false);
    }
}
