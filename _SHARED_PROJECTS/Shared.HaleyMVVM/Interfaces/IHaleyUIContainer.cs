using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.Abstractions
{
    public interface IHaleyUIContainer<BaseVMType,BaseViewType> 
    {
        #region registration methods
        string register<VMType, ViewType>(VMType InputViewModel=null, bool use_vm_as_key = true, bool is_singleton = true)
            where VMType : class, BaseVMType
            where ViewType : BaseViewType;
        string register<VMType, ViewType>(string key, VMType InputViewModel=null, bool is_singleton = true)
            where VMType : class, BaseVMType
            where ViewType : BaseViewType;
        string register<VMType, ViewType>(Enum key, VMType InputViewModel=null, bool is_singleton = true)
            where VMType : class, BaseVMType
            where ViewType : BaseViewType;

        #endregion

        #region View Generation Methods
        BaseViewType generateView<VMType>(VMType InputViewModel=null, bool generate_vm_instance = false) where VMType : class, BaseVMType;
        BaseViewType generateView(string key, object InputViewModel = null, bool generate_vm_instance = false);
        BaseViewType generateView(Enum key, object InputViewModel = null,bool generate_vm_instance = false);
        #endregion

        #region ViewModel Generation methods
        BaseVMType generateViewModel(Enum @enum, bool generate_vm_instance = false);
        BaseVMType generateViewModel(string key, bool generate_vm_instance = false);
        (Type viewmodel_type, Type view_type, bool is_singleton) getMappingValue(Enum @enum);
        (Type viewmodel_type, Type view_type, bool is_singleton) getMappingValue(string key);
        #endregion
    }
}
