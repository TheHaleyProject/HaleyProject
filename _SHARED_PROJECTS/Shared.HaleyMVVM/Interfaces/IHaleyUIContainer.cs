using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Haley.Enums;

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
        BaseViewType generateView<VMType>(VMType InputViewModel=null, InstanceGeneration instance_level = InstanceGeneration.None) where VMType : class, BaseVMType;
        BaseViewType generateView(string key, object InputViewModel = null, InstanceGeneration instance_level = InstanceGeneration.None);
        BaseViewType generateView(Enum key, object InputViewModel = null,InstanceGeneration instance_level = InstanceGeneration.None);
        #endregion

        #region ViewModel Generation methods
        BaseVMType generateViewModel(Enum @enum, InstanceGeneration instance_level = InstanceGeneration.None);
        BaseVMType generateViewModel(string key, InstanceGeneration instance_level = InstanceGeneration.None);
        (Type viewmodel_type, Type view_type, bool is_singleton) getMappingValue(Enum @enum);
        (Type viewmodel_type, Type view_type, bool is_singleton) getMappingValue(string key);
        #endregion
    }
}
