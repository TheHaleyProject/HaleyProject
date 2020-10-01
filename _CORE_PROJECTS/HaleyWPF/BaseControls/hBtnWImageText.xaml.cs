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
    /// Interaction logic for hBtnWImageText.xaml
    /// </summary>
    public partial class hBtnWImageText : UserControl
    {
        public hBtnWImageText()
        {
            InitializeComponent();
        }

        #region Common Dependency Properties

        public string CustomContent
        {
            get { return (string)GetValue(CustomContentProperty); }
            set { SetValue(CustomContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomContentProperty =
            DependencyProperty.Register(nameof(CustomContent), typeof(string), typeof(hBtnWImageText), new PropertyMetadata(null, CustomContentPropertyChanged));

        private static void CustomContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hBtnWImageText ido = d as hBtnWImageText;
            if (ido != null)
            {
                ido.CustomContent = (string)e.NewValue;
            }
        }

        public bool ImageOnLeft
        {
            get { return (bool)GetValue(ImageOnLeftProperty); }
            set { SetValue(ImageOnLeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageOnLeft.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageOnLeftProperty =
            DependencyProperty.Register(nameof(ImageOnLeft), typeof(bool), typeof(hBtnWImageText), new PropertyMetadata(true, ImageOnLeftPropertyChanged));

        private static void ImageOnLeftPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hBtnWImageText ido = d as hBtnWImageText;
            if (ido != null)
            {
                ido.ImageOnLeft = (bool)e.NewValue;
            }
        }

        public bool IsImageVisible
        {
            get { return (bool)GetValue(IsImageVisibleProperty); }
            set { SetValue(IsImageVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsImageVisible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsImageVisibleProperty =
            DependencyProperty.Register(nameof(IsImageVisible), typeof(bool), typeof(hBtnWImageText), new PropertyMetadata(true, IsImageVisiblePropertyChanged));

        private static void IsImageVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hBtnWImageText ido = d as hBtnWImageText;
            if (ido != null)
            {
                ido.IsImageVisible = (bool)e.NewValue;
            }
        }
        #endregion

        #region Image Dependency Properties
        public ImageSource ButtonImage
        {
            get { return (ImageSource)GetValue(ButtonImageProperty); }
            set { SetValue(ButtonImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourcePrimaryImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonImageProperty =
            DependencyProperty.Register(nameof(ButtonImage), typeof(ImageSource), typeof(hBtnWImageText), new PropertyMetadata(null, ButtonImagePropertyChanged));

        private static void ButtonImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hBtnWImageText ido = d as hBtnWImageText;
            if (ido != null)
            {
                ido.ButtonImage = (ImageSource)e.NewValue;
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
            DependencyProperty.Register(nameof(UserCommand), typeof(ICommand), typeof(hBtnWImageText), new PropertyMetadata(null));

        public object UserCommandParameter
        {
            get { return (object)GetValue(UserCommandParameterProperty); }
            set { SetValue(UserCommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UserCommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UserCommandParameterProperty =
            DependencyProperty.Register(nameof(UserCommandParameter), typeof(object), typeof(hBtnWImageText), new PropertyMetadata(null));

        #endregion

        #region Color Dependency Properties

        public SolidColorBrush CustomBackGround
        {
            get { return (SolidColorBrush)GetValue(CustomBackGroundProperty); }
            set { SetValue(CustomBackGroundProperty, value); }
        }
        // Using a DependencyProperty as the backing store for CustomBackGround.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomBackGroundProperty =
            DependencyProperty.Register(nameof(CustomBackGround), typeof(SolidColorBrush), typeof(hBtnWImageText), new PropertyMetadata(null, CustomBackGroundPropertyChanged));
        private static void CustomBackGroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hBtnWImageText ido = d as hBtnWImageText;
            if (ido != null)
            {
                ido.CustomBackGround = e.NewValue as SolidColorBrush;
            }
        }

        public SolidColorBrush CustomForeGround
        {
            get { return (SolidColorBrush)GetValue(CustomForeGroundProperty); }
            set { SetValue(CustomForeGroundProperty, value); }
        }
        // Using a DependencyProperty as the backing store for CustomForeGround.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomForeGroundProperty =
            DependencyProperty.Register(nameof(CustomForeGround), typeof(SolidColorBrush), typeof(hBtnWImageText), new PropertyMetadata(null, CustomForeGroundPropertyChanged));
        private static void CustomForeGroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hBtnWImageText ido = d as hBtnWImageText;
            if (ido != null)
            {
                ido.CustomForeGround = e.NewValue as SolidColorBrush;
            }
        }

        public SolidColorBrush CustomBorderHighlight
        {
            get { return (SolidColorBrush)GetValue(CustomBorderHighlightProperty); }
            set { SetValue(CustomBorderHighlightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomBorderHighlight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomBorderHighlightProperty =
            DependencyProperty.Register(nameof(CustomBorderHighlight), typeof(SolidColorBrush), typeof(hBtnWImageText), new PropertyMetadata(null));


        #endregion

        public event RoutedEventHandler Click; //Routed event is raised when the button is clicked. The same will be handled by this handler.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
