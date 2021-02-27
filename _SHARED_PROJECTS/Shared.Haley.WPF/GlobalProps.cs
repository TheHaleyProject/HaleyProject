using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Haley.WPF
{
    internal static class GlobalProps
    {
        public static SolidColorBrush default_oncolor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF75B075");
        public static SolidColorBrush default_offcolor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFB07575");
        public static SolidColorBrush hoverBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#7ea0d6"); //Blue
        public static CornerRadius cornerRadius = new CornerRadius(0.0);
        public static SolidColorBrush shadowColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#575552"); //Gray
    }
}
