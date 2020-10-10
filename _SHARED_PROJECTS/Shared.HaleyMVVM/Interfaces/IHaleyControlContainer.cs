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
        #region registration methods
        void register<ViewModelType, ControlType>(ViewModelType InputViewModel = null, bool use_vm_as_key = false, bool is_singleton = true)
           where ControlType : IHaleyControl
           where ViewModelType : class, IHaleyControlVM;
        void register<ViewModelType, ControlType>(string key, ViewModelType InputViewModel = null, bool is_singleton = true)
           where ViewModelType : class, IHaleyControlVM
           where ControlType : IHaleyControl;
        void register<ViewModelType, ControlType>(Enum key, ViewModelType InputViewModel = null, bool is_singleton = true)
           where ViewModelType : class, IHaleyControlVM
           where ControlType : IHaleyControl;
        #endregion

        #region Control ObtainMethods
        IHaleyControl obtainControl<ViewModelType>(ViewModelType InputViewModel = null) where ViewModelType : class, IHaleyControlVM;
        IHaleyControl obtainControl(string key, object InputViewModel = null);
        IHaleyControl obtainControl(Enum key, object InputViewModel = null);
        IHaleyControl obtainControl<ViewModelType>(bool generate_vm_instance = false) where ViewModelType : class, IHaleyControlVM;
        IHaleyControl obtainControl(string key,bool generate_vm_instance = false);
        IHaleyControl obtainControl(Enum key, bool generate_vm_instance = false);
        #endregion

        #region VM ObtainMethods
        IHaleyControlVM generateViewModel(Enum @enum, bool generate_vm_instance = false);
        IHaleyControlVM generateViewModel(string key, bool generate_vm_instance = false);
        (Type viewmodel_type, Type view_type, bool is_singleton) getMappingValue(string key);
        #endregion
    }
}
