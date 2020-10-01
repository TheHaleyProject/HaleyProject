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
using System.Globalization;
using System.Security;


namespace Haley.Wpf.BaseControls
{
    /// <summary>
    /// Interaction logic for hPasswordBox.xaml
    /// </summary>
    public partial class hPasswordBox : UserControl
    {
        public hPasswordBox()
        {
            InitializeComponent();
        }

        public static Thickness default_thickness = new Thickness(double.Parse("0.5"));

        public string PlaceHolderText
        {
            get { return (string)GetValue(PlaceHolderTextProperty); }
            set { SetValue(PlaceHolderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderTextProperty =
            DependencyProperty.Register(nameof(PlaceHolderText), typeof(string), typeof(hPasswordBox), new PropertyMetadata("Enter Password"));

        public SolidColorBrush CustomBorderBrush
        {
            get { return (SolidColorBrush)GetValue(CustomBorderBrushProperty); }
            set { SetValue(CustomBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomBorderBrushProperty =
            DependencyProperty.Register(nameof(CustomBorderBrush), typeof(SolidColorBrush), typeof(hPasswordBox), new PropertyMetadata(null));


        public Thickness CustomBorderThickness
        {
            get { return (Thickness)GetValue(CustomBorderThicknessProperty); }
            set { SetValue(CustomBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomBorderThicknessProperty =
            DependencyProperty.Register(nameof(CustomBorderThickness), typeof(Thickness), typeof(hPasswordBox), new PropertyMetadata(default_thickness));

        public SecureString WorkingSSTR
        {
            get { return (SecureString)GetValue(WorkingSSTRProperty); }
            set { SetValue(WorkingSSTRProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkingSSTR.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WorkingSSTRProperty =
            DependencyProperty.Register(nameof(WorkingSSTR), typeof(SecureString), typeof(hPasswordBox), new PropertyMetadata(null));

        public bool HasPassword
        {
            get { return (bool)GetValue(HasPasswordProperty); }
            set { SetValue(HasPasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasPassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasPasswordProperty =
            DependencyProperty.Register(nameof(HasPassword), typeof(bool), typeof(hPasswordBox), new PropertyMetadata(false));

        private void passchanged_event(object sender, RoutedEventArgs e)
        {
            try
            {
                
                WorkingSSTR = (sender as PasswordBox).SecurePassword; //get the password
                // for the lenght of password
                HasPassword = false; //the moment, user tries to enter some value, the store password should be ignored.
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public class ConverterhPasswordBoxPlaceHolder : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            int param = 1;
            if (parameter != null) param = int.Parse((string)parameter);

            PasswordBox pbox_object = (PasswordBox)values[0];
            bool object_has_focus = (bool)values[1];
            bool has_password = (bool)values[2];

            //Under anycase, if the password box has the focus or has value, nothing should be visible
            if (object_has_focus == true || string.IsNullOrWhiteSpace(pbox_object.Password) == false) return Visibility.Collapsed;

            switch (param)
            {
                case 1: //Coming from Place holder text. If password is present, it should not be visible
                    if (has_password) return Visibility.Collapsed;
                    break;
                case 2: //Coming from Masking text. If password is present then, maskin text should be invoked.
                    if (!has_password) return Visibility.Collapsed;
                    break;
            }
            return Visibility.Visible;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}

