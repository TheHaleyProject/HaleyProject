using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.Abstractions
{
    public interface IHaleyControlContainer<BaseVMType, BaseViewType> : IHaleyUIContainer<BaseVMType, BaseViewType> 
    {
        new string register<VMType, ViewType>(VMType InputViewModel = null, bool use_vm_as_key = false, bool is_singleton = true)
           where VMType : class, BaseVMType
           where ViewType : BaseViewType;
    }
}
