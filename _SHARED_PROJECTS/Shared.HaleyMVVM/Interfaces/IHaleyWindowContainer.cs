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
        bool? showDialog<VMType>(VMType InputViewModel = null, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None) where VMType : class, BaseVMType;
        bool? showDialog<ViewType>(GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None) where ViewType : BaseViewType;
        bool? showDialog(string key, object InputViewModel = null, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None);
        bool? showDialog(Enum key, object InputViewModel = null, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None);

        #endregion

        #region Show Methods
        void show<VMType>(VMType InputViewModel = null, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None) where VMType : class, BaseVMType;
        void show<ViewType>(GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None) where ViewType : BaseViewType;
        void show(string key, object InputViewModel = null, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None);
        void show(Enum key, object InputViewModel = null, GenerateNewInstanceFor instance_level = GenerateNewInstanceFor.None);
        #endregion
    }
}
