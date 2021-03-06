using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Haley.Utils
{
    public static class AdditionalCommands
    {
        public readonly static RoutedUICommand ChangeCount = new RoutedUICommand("To change count of a value", nameof(ChangeCount), typeof(AdditionalCommands));
        public readonly static RoutedUICommand ChangeSelection = new RoutedUICommand("To change the selection", nameof(ChangeSelection), typeof(AdditionalCommands));
        public readonly static RoutedUICommand Highlight = new RoutedUICommand("To highlight something", nameof(Highlight), typeof(AdditionalCommands));
    }
}
