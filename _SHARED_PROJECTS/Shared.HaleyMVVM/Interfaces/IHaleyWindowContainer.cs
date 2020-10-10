using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.Abstractions
{
    public interface IHaleyWindowContainer<BaseVMType, BaseViewType> : IHaleyUIContainer<BaseVMType, BaseViewType>
    {
        #region ShowDialog Methods
        bool? showDialog<VMType>(VMType InputViewModel = null, bool generate_vm_instance = false) where VMType : class, BaseVMType;
        bool? showDialog(string key, object InputViewModel = null, bool generate_vm_instance = false);
        bool? showDialog(Enum key, object InputViewModel = null, bool generate_vm_instance = false);

        #endregion

        #region Show Methods
        void show<VMType>(VMType InputViewModel = null, bool generate_vm_instance = false) where VMType : class, BaseVMType;
        void show(string key, object InputViewModel = null, bool generate_vm_instance = false);
        void show(Enum key, object InputViewModel = null, bool generate_vm_instance = false);
        #endregion
    }
}
