using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.Abstractions
{
  public interface IHaleyWindowContainer 
        {
      void register<ViewModelType, ViewType>(ViewModelType instance = null)
                    where ViewModelType : class, IHaleyWindowVM
                    where ViewType : IHaleyWindow;
        bool? showDialog<ViewModelType>(ViewModelType InputViewModel = null, bool generate_vm_instance = false) 
                where ViewModelType: class, IHaleyWindowVM;
        void show<ViewModelType>(ViewModelType InputViewModel = null, bool generate_vm_instance = false)
          where ViewModelType : class, IHaleyWindowVM;
        IHaleyWindowVM generateViewModel(Type key, bool generate_vm_instance = false);
    }

}
