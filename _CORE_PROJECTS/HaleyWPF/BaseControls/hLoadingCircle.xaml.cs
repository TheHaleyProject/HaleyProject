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

namespace Haley.Wpf.BaseControls
{
    /// <summary>
    /// Interaction logic for hLoadingCircle.xaml
    /// </summary>
    public partial class hLoadingCircle : UserControl
    {
        private static SolidColorBrush default_color;
        public SolidColorBrush base_color
        {
            get { return (SolidColorBrush)GetValue(base_colorProperty); }
            set { SetValue(base_colorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for base_color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty base_colorProperty =
            DependencyProperty.Register("base_color", typeof(SolidColorBrush), typeof(hLoadingCircle), new PropertyMetadata(default_color));

        public hLoadingCircle()
        {
            InitializeComponent();
            default_color = new SolidColorBrush(Color.FromRgb(255, 233, 178));
        }
    }
}
