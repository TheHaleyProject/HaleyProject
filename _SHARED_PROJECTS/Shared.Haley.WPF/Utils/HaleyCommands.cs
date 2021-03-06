using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Haley.Utils
{
    public static class AdditionalCommands
    {
        public readonly static RoutedUICommand ChangeCount = new RoutedUICommand("To change count of a value", nameof(ChangeCount), typeof(AdditionalCommands));
    }
}
