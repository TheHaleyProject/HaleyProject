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
    /// Interaction logic for hBtnWImage.xaml
    /// </summary>
    public partial class hBtnWImage : UserControl
    {
        public hBtnWImage()
        {
            InitializeComponent();
        }

        #region Common Dependency Properties

        public ImageSource SourcePrimaryImage
        {
            get { return (ImageSource)GetValue(SourcePrimaryImageProperty); }
            set { SetValue(SourcePrimaryImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourcePrimaryImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourcePrimaryImageProperty =
            DependencyProperty.Register(nameof(SourcePrimaryImage), typeof(ImageSource), typeof(hBtnWImage), new PropertyMetadata(null, SourcePrimaryImagePropertyChanged));

        private static void SourcePrimaryImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hBtnWImage ido = d as hBtnWImage;
            if (ido != null)
            {
                ido.SourcePrimaryImage = (ImageSource)e.NewValue;
            }
        }


        public ImageSource SourceSecondaryImage
        {
            get { return (ImageSource)GetValue(SourceSecondaryImageProperty); }
            set { SetValue(SourceSecondaryImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceSecondaryImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceSecondaryImageProperty =
            DependencyProperty.Register(nameof(SourceSecondaryImage), typeof(ImageSource), typeof(hBtnWImage), new PropertyMetadata(null, SourceSecondaryImagePropertyChanged));

        private static void SourceSecondaryImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hBtnWImage ido = d as hBtnWImage;
            if (ido != null)
            {
                ido.SourceSecondaryImage = (ImageSource)e.NewValue;
            }
        }


        #endregion

        #region Command Dependency Properties

        public ICommand UserCommand
        {
            get { return (ICommand)GetValue(UserCommandProperty); }
            set { SetValue(UserCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for UserCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserCommandProperty =
            DependencyProperty.Register(nameof(UserCommand), typeof(ICommand), typeof(hBtnWImage), new PropertyMetadata(null));

        public object UserCommandParameter
        {
            get { return (object)GetValue(UserCommandParameterProperty); }
            set { SetValue(UserCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserCommandParameterProperty =
            DependencyProperty.Register(nameof(UserCommandParameter), typeof(object), typeof(hBtnWImage), new PropertyMetadata(null));

        #endregion

        public event RoutedEventHandler Click; //Routed event is raised when the button is clicked. The same will be handled by this handler. Make this event as public to be accessible from outside.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
