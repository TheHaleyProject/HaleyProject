using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Haley.Enums;

namespace Haley.Abstractions
{
    public interface IHaleyWindowContainer<BaseVMType, BaseViewType> : IHaleyUIContainer<BaseVMType, BaseViewType>
    {
        #region ShowDialog Methods
        bool? showDialog<VMType>(VMType InputViewModel = null, InstanceGeneration instance_level = InstanceGeneration.None) where VMType : class, BaseVMType;
        bool? showDialog<ViewType>(InstanceGeneration instance_level = InstanceGeneration.None) where ViewType : BaseViewType;
        bool? showDialog(string key, object InputViewModel = null, InstanceGeneration instance_level = InstanceGeneration.None);
        bool? showDialog(Enum key, object InputViewModel = null, InstanceGeneration instance_level = InstanceGeneration.None);

        #endregion

        #region Show Methods
        void show<VMType>(VMType InputViewModel = null, InstanceGeneration instance_level = InstanceGeneration.None) where VMType : class, BaseVMType;
        void show<ViewType>(InstanceGeneration instance_level = InstanceGeneration.None) where ViewType : BaseViewType;
        void show(string key, object InputViewModel = null, InstanceGeneration instance_level = InstanceGeneration.None);
        void show(Enum key, object InputViewModel = null, InstanceGeneration instance_level = InstanceGeneration.None);
        #endregion
    }
}
