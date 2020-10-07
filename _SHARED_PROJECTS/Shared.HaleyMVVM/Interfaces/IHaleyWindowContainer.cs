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
        bool? showDialog<ViewModelType>(ViewModelType InputViewModel = null, bool create_new_vm = false) 
                where ViewModelType: class, IHaleyWindowVM;
        void show<ViewModelType>(ViewModelType InputViewModel = null, bool create_new_vm = false)
          where ViewModelType : class, IHaleyWindowVM;
    }

}
