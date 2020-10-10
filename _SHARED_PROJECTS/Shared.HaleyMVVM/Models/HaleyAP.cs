using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Haley.Abstractions;
using System.Windows;
using System.Windows.Controls;
using Haley.MVVM;

namespace Haley.Models
{
    public static class HaleyAP
    {
        #region Key

        public static string GetContainerKey(DependencyObject obj)
        {
            return (string)obj.GetValue(ContainerKeyProperty);
        }

        public static void SetContainerKey(DependencyObject obj, string value)
        {
            obj.SetValue(ContainerKeyProperty, value);
        }

        // Using a DependencyProperty as the backing store for ContainerKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContainerKeyProperty =
            DependencyProperty.RegisterAttached("ContainerKey", typeof(string), typeof(HaleyAP), new PropertyMetadata(null));

        #endregion

        #region CreateNewInstance
        public static bool GetCreateNewInstance(DependencyObject obj)
        {
            return (bool)obj.GetValue(CreateNewInstanceProperty);
        }

        public static void SetCreateNewInstance(DependencyObject obj, bool value)
        {
            obj.SetValue(CreateNewInstanceProperty, value);
        }

        // Using a DependencyProperty as the backing store for CreateNewInstance.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CreateNewInstanceProperty =
            DependencyProperty.RegisterAttached("CreateNewInstance", typeof(bool), typeof(HaleyAP), new PropertyMetadata(false));
        #endregion

        public static bool GetInjectVM(DependencyObject obj)
        {
            return (bool)obj.GetValue(InjectVMProperty);
        }

        public static void SetInjectVM(DependencyObject obj, bool value)
        {
            obj.SetValue(InjectVMProperty, value);
        }

        // Using a DependencyProperty as the backing store for InjectVM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InjectVMProperty =
            DependencyProperty.RegisterAttached("InjectVM", typeof(bool), typeof(HaleyAP), new PropertyMetadata(false,InjectVMPropertyChanged));

        private static void InjectVMPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //For controls
           if ((d is UserControl && d is IHaleyControl)|| (d is Window && d is IHaleyWindow))
            {
                try
                {
                    //If d is usercontrol and also implements ihaleycontrol, then resolve the viewmodel
                    string _key = GetContainerKey(d);
                    if (_key == null) _key = d.GetType().FullName;
                    if (d is IHaleyControl)
                    {
                        var _vm = ContainerStore.Singleton.controls.generateViewModel(_key, GetCreateNewInstance(d));
                        if (_vm != null) //Only if not null, assign it.
                        {
                            ((UserControl)d).DataContext = _vm;
                        }
                    }
                    else if (d is IHaleyWindow)
                    {
                        var _vm = ContainerStore.Singleton.windows.generateViewModel(_key, GetCreateNewInstance(d));
                        if (_vm != null) //Only if not null, assign it.
                        {
                            ((Window)d).DataContext = _vm;
                        }
                    }
                }
                catch (Exception)
                {
                    //Do not set the viewmodel
                }
            }
        }
    }
}
