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
using Haley.Flipper.MVVM.Interfaces;
using DevelopmentWPF.ViewModels;

namespace DevelopmentWPF.Controls
{
    /// <summary>
    /// Interaction logic for ctrl02.xaml
    /// </summary>
    public partial class ctrl02 : UserControl, IFlipperControl
    {
        public ctrl02()
        {
            InitializeComponent();
        }
    }
}
