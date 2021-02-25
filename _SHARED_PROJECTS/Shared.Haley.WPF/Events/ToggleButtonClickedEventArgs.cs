using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Haley.WPF.Events
{
    public class ToggleButtonClickedEventArgs : RoutedEventArgs
    {
        public bool value { get; set; }
        public ToggleButtonClickedEventArgs(RoutedEvent base_event,object sender) : base(base_event,sender)
        {
        }
    }
}
