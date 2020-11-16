using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haley.Abstractions;
using Haley.Utils;
using System.Collections.Concurrent;
using Haley.Enums;
using Haley.IOC;

namespace Haley.MVVM.Containers
{
    public sealed class ControlContainer : UIContainerBase<IHaleyControlVM,IHaleyControl>, IHaleyControlContainer<IHaleyControlVM, IHaleyControl>
    {
        public ControlContainer(IHaleyDIContainer _injection_container):base(_injection_container) { }

        public override IHaleyControl generateView(string key, object InputViewModel = null, TransientAffectMode transient_mode = TransientAffectMode.None)
        {
            try
            {
                //If you receive a input viewmodel, obviously, then it is not a singleton approach. You take this inputviewmodel and assign it. so by default, the generate vm instance will not be taken into account.
                var _kvp = _generateValuePair(key, transient_mode);
                if (InputViewModel != null)
                {
                    _kvp.view.DataContext = InputViewModel; //Assinging actual viewmodel
                }
                else
                {
                    _kvp.view.DataContext = _kvp.view_model; //Assinging generated viewmodel
                }

                return _kvp.view;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

