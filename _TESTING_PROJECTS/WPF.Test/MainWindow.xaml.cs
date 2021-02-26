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
using Haley.Utils;
using Haley.Models;
using Haley.Enums;

namespace WPF.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleButton_OnClicked(object sender, RoutedEventArgs e)
        {

        }

        private void PlainButton_Click(object sender, RoutedEventArgs e)
        {
            int _red = string.IsNullOrEmpty(redValue.Text) ?  0 : int.Parse(redValue.Text);
            int _green = string.IsNullOrEmpty(greenValue.Text) ? 0 : int.Parse(greenValue.Text);
            int _blue = string.IsNullOrEmpty(blueValue.Text) ? 0 : int.Parse(blueValue.Text);

            var _source = imgeChanger.Source;

            var _imageinfo = ImageUtils.getImageInfo(_source);
            var _newsource = ImageUtils.changeImageColor(_imageinfo, _red, _green, _blue);
            imgeChanger.Source = _newsource;

        }
    }
}
