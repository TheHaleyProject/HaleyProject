using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Haley.MVVM.Interfaces
{
  public interface IHaleyWindowContainer 
        {
            void register<ViewModelType, ViewType>() 
                where ViewModelType : IHaleyWindowVM, new()
                where ViewType : IHaleyWindow ; 

            bool? showDialog<ViewModelType>(ViewModelType InputViewModel) 
                where ViewModelType : IHaleyWindowVM;
            void show<ViewModelType>(ViewModelType InputViewModel) 
                where ViewModelType : IHaleyWindowVM;
    }

}
