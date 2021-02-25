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
using Haley.Enums;
namespace Haley.Models
{
    /// <summary>
    ///To restrict the entry to only allow numbers
    /// </summary>
    public static class InputAP
    {
        public static bool GetOnlyNumbers(DependencyObject obj)
        {
            return (bool)obj.GetValue(OnlyNumbersProperty);
        }

        public static void SetOnlyNumbers(DependencyObject obj, bool value)
        {
            obj.SetValue(OnlyNumbersProperty, value);
        }

        // Using a DependencyProperty as the backing store for OnlyNumbers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OnlyNumbersProperty =
            DependencyProperty.RegisterAttached("OnlyNumbers", typeof(bool), typeof(InputAP), new FrameworkPropertyMetadata(false,OnlyNumbersPropertyChanged));

        private static void OnlyNumbersPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           if (d is TextBox)
            {
                ((TextBox)d).TextChanged += InputAP_TextChanged;
            }
        }

        private static void InputAP_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Process it accordingly to ensure that the text is only numeric.
        }
    }
}
