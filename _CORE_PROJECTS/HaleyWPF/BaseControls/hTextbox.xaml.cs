using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Security;

namespace Haley.Wpf.BaseControls
{
    /// <summary>
    /// Interaction logic for hTextbox.xaml
    /// </summary>
    public partial class hTextbox : UserControl
    {
        public static Thickness default_thickness = new Thickness(double.Parse("0.5"));

        public string PlaceHolderText
        {
            get { return (string)GetValue(PlaceHolderTextProperty); }
            set { SetValue(PlaceHolderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderTextProperty =
            DependencyProperty.Register(nameof(PlaceHolderText), typeof(string), typeof(hTextbox), new PropertyMetadata("Empty", PlaceHolderTextChanged));

        private static void PlaceHolderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hTextbox ido = d as hTextbox;
            if (ido != null)
            {
                ido.PlaceHolderText = (string)e.NewValue;
            }
        }

        public string WorkingText
        {
            get { return (string)GetValue(WorkingTextProperty); }
            set { SetValue(WorkingTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkingText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WorkingTextProperty =
            DependencyProperty.Register(nameof(WorkingText), typeof(string), typeof(hTextbox), new PropertyMetadata(null, WorkingTextChanged));

        private static void WorkingTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hTextbox ido = d as hTextbox;
            if (ido != null)
            {
                ido.WorkingText = (string)e.NewValue;
            }
        }

        public SolidColorBrush CustomBorderBrush
        {
            get { return (SolidColorBrush)GetValue(CustomBorderBrushProperty); }
            set { SetValue(CustomBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomBorderBrushProperty =
            DependencyProperty.Register(nameof(CustomBorderBrush), typeof(SolidColorBrush), typeof(hTextbox), new PropertyMetadata(null, new PropertyChangedCallback(CustomBorderBrushPropertyChanged)));

        private static void CustomBorderBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hTextbox ido = d as hTextbox;
            if (ido != null)
            {
                ido.CustomBorderBrush = (SolidColorBrush)e.NewValue;
            }
        }

        public Thickness CustomBorderThickness
        {
            get { return (Thickness)GetValue(CustomBorderThicknessProperty); }
            set { SetValue(CustomBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomBorderThicknessProperty =
            DependencyProperty.Register(nameof(CustomBorderThickness), typeof(Thickness), typeof(hTextbox), new PropertyMetadata(default_thickness, new PropertyChangedCallback(CustomBorderThicknessPropertyChanged)));

        private static void CustomBorderThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hTextbox ido = d as hTextbox;
            if (ido != null)
            {
                ido.CustomBorderThickness = (Thickness)e.NewValue;
            }
        }

        public hTextbox()
        {
            InitializeComponent();
        }
    }

    public class ConverterhTextboxPlaceHolder : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool object_is_empty = (bool)values[0];
            bool object_has_focus = (bool)values[1];

            //Either the texbox/hPasswordBox has focus or value, then collapse the underlying placeholder textblock.

            if (object_is_empty == false || object_has_focus == true)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
