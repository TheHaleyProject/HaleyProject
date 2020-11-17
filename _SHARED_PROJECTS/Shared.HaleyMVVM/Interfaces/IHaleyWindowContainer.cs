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
        bool? showDialog<VMType>(VMType InputViewModel = null, ResolveMode resolve_mode = ResolveMode.Default) where VMType : class, BaseVMType;
        bool? showDialog<ViewType>(ResolveMode resolve_mode = ResolveMode.Default) where ViewType : BaseViewType;
        bool? showDialog(string key, object InputViewModel = null, ResolveMode resolve_mode = ResolveMode.Default);
        bool? showDialog(Enum key, object InputViewModel = null, ResolveMode resolve_mode = ResolveMode.Default);

        #endregion

        #region Show Methods
        void show<VMType>(VMType InputViewModel = null, ResolveMode resolve_mode = ResolveMode.Default) where VMType : class, BaseVMType;
        void show<ViewType>(ResolveMode resolve_mode = ResolveMode.Default) where ViewType : BaseViewType;
        void show(string key, object InputViewModel = null, ResolveMode resolve_mode = ResolveMode.Default);
        void show(Enum key, object InputViewModel = null, ResolveMode resolve_mode = ResolveMode.Default);
        #endregion
    }
}
