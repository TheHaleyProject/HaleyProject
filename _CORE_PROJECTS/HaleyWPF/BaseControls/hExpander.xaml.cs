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
using System.ComponentModel;

namespace Haley.Wpf.BaseControls
{
    /// <summary>
    /// Interaction logic for hExpander.xaml
    /// </summary>
    public partial class hExpander : UserControl
    {
        #region Common Properties

        #endregion

        #region Common Dependencies

        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register(nameof(HeaderHeight), typeof(double), typeof(hExpander), new PropertyMetadata(double.Parse("30"), HeaderHeightPropertyChanged));

        private static void HeaderHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;
            if (ido != null)
            {
                ido.HeaderHeight = (double)e.NewValue;
            }
        }

        #endregion

        #region Command Properties

       
        #endregion

        #region Boolean Dependencies

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(hExpander), new PropertyMetadata(false,IsSelectedPropertyChanged));

        private static void IsSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;

            if (ido != null)
            {
                ido.IsSelected = (bool)e.NewValue;
            }
        }

        public bool HeaderHasControl
        {
            get { return (bool)GetValue(HeaderHasControlProperty); }
            set { SetValue(HeaderHasControlProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HeaderHasControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderHasControlProperty =
            DependencyProperty.Register(nameof(HeaderHasControl), typeof(bool), typeof(hExpander), new PropertyMetadata(false,HeaderHasControlPropertyChanged));
        private static void HeaderHasControlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;

            if (ido != null)
            {
                ido.HeaderHasControl = (bool)e.NewValue;
            }
        }

        #endregion

        #region Image Dependencies

        public ImageSource DisplayImage //For Default
        {
            get { return (ImageSource)GetValue(DisplayImageProperty); }
            set { SetValue(DisplayImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayImageProperty =
            DependencyProperty.Register(nameof(DisplayImage), typeof(ImageSource), typeof(hExpander), new PropertyMetadata(null, DisplayImagePropertyChanged));

        private static void DisplayImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;

            if (ido != null)
            {
                ido.DisplayImage = (ImageSource)e.NewValue;
            }
        }

        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        } //For Hover

        // Using a DependencyProperty as the backing store for HoverImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register(nameof(HoverImage), typeof(ImageSource), typeof(hExpander), new PropertyMetadata(null, HoverImagePropertyChanged));

        private static void HoverImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;

            if (ido != null)
            {
                ido.HoverImage = (ImageSource)e.NewValue;
            }
        }

        public ImageSource SelectedImage //For Selected.
        {
            get { return (ImageSource)GetValue(SelectedImageProperty); }
            set { SetValue(SelectedImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedImageProperty =
            DependencyProperty.Register(nameof(SelectedImage), typeof(ImageSource), typeof(hExpander), new PropertyMetadata(null,SelectedImagePropertyChanged));

        private static void SelectedImagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;

            if (ido != null)
            {
                ido.SelectedImage = (ImageSource)e.NewValue;
            }
        }

        #endregion

        #region Color Dependencies

        public SolidColorBrush BGHeader
        {
            get { return (SolidColorBrush)GetValue(BGHeaderProperty); }
            set { SetValue(BGHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BGHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BGHeaderProperty =
            DependencyProperty.Register(nameof(BGHeader), typeof(SolidColorBrush), typeof(hExpander), new PropertyMetadata(null, BGHeaderPropertyChanged));

        private static void BGHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;
            if (ido != null)
            {
                ido.BGHeader = (SolidColorBrush)e.NewValue;
            }
        }

        public SolidColorBrush BGHeaderSelected
        {
            get { return (SolidColorBrush)GetValue(BGHeaderSelectedProperty); }
            set { SetValue(BGHeaderSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BGHeaderSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BGHeaderSelectedProperty =
            DependencyProperty.Register(nameof(BGHeaderSelected), typeof(SolidColorBrush), typeof(hExpander), new PropertyMetadata(null, BGHeaderSelectedPropertyChanged));

        private static void BGHeaderSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;
            if (ido != null)
            {
                ido.BGHeaderSelected = (SolidColorBrush)e.NewValue;
            }
        }

        public SolidColorBrush BGHeaderHover
        {
            get { return (SolidColorBrush)GetValue(BGHeaderHoverProperty); }
            set { SetValue(BGHeaderHoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BGHeaderHover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BGHeaderHoverProperty =
            DependencyProperty.Register(nameof(BGHeaderHover), typeof(SolidColorBrush), typeof(hExpander), new PropertyMetadata(null, BGHeaderHoverPropertyChanged));

        private static void BGHeaderHoverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;
            if (ido != null)
            {
                ido.BGHeaderHover = (SolidColorBrush)e.NewValue;
            }
        }

        public SolidColorBrush FGHeader
        {
            get { return (SolidColorBrush)GetValue(FGHeaderProperty); }
            set { SetValue(FGHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FGHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FGHeaderProperty =
            DependencyProperty.Register(nameof(FGHeader), typeof(SolidColorBrush), typeof(hExpander), new PropertyMetadata(null, FGHeaderPropertyChanged));

        private static void FGHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;
            if (ido != null)
            {
                ido.FGHeader = (SolidColorBrush)e.NewValue;
            }
        }

        public SolidColorBrush FGHeaderSelected
        {
            get { return (SolidColorBrush)GetValue(FGHeaderSelectedProperty); }
            set { SetValue(FGHeaderSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FGHeaderSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FGHeaderSelectedProperty =
            DependencyProperty.Register(nameof(FGHeaderSelected), typeof(SolidColorBrush), typeof(hExpander), new PropertyMetadata(null, FGHeaderSelectedPropertyChanged));

        private static void FGHeaderSelectedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;
            if (ido != null)
            {
                ido.FGHeaderSelected = (SolidColorBrush)e.NewValue;
            }
        }

        public SolidColorBrush FGHeaderHover
        {
            get { return (SolidColorBrush)GetValue(FGHeaderHoverProperty); }
            set { SetValue(FGHeaderHoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FGHeaderHover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FGHeaderHoverProperty =
            DependencyProperty.Register(nameof(FGHeaderHover), typeof(SolidColorBrush), typeof(hExpander), new PropertyMetadata(null, FGHeaderHoverPropertyChanged));

        private static void FGHeaderHoverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;
            if (ido != null)
            {
                ido.FGHeaderHover = (SolidColorBrush)e.NewValue;
            }
        }

        public SolidColorBrush BGPopUp
        {
            get { return (SolidColorBrush)GetValue(BGPopUpProperty); }
            set { SetValue(BGPopUpProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BGPopUp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BGPopUpProperty =
            DependencyProperty.Register(nameof(BGPopUp), typeof(SolidColorBrush), typeof(hExpander), new PropertyMetadata(null, BGPopUpPropertyChanged));

        private static void BGPopUpPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;
            if (ido != null)
            {
                ido.BGPopUp = (SolidColorBrush)e.NewValue;
            }
        }

        #endregion

        #region String Dependencies

        public string PrimaryTitle
        {
            get { return (string)GetValue(PrimaryTitleProperty); }
            set { SetValue(PrimaryTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrimaryTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PrimaryTitleProperty =
            DependencyProperty.Register(nameof(PrimaryTitle), typeof(string), typeof(hExpander), new PropertyMetadata(null, PrimaryTitlePropertyChanged));

        private static void PrimaryTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;

            if (ido != null)
            {
                ido.PrimaryTitle = (string)e.NewValue;
            }
        }

        public string SecondaryTitle
        {
            get { return (string)GetValue(SecondaryTitleProperty); }
            set { SetValue(SecondaryTitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondaryTitle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondaryTitleProperty =
            DependencyProperty.Register(nameof(SecondaryTitle), typeof(string), typeof(hExpander), new PropertyMetadata(null));

        private static void SecondaryTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;

            if (ido != null)
            {
                ido.SecondaryTitle = (string)e.NewValue;
            }
        }

        #endregion

        #region Object Dependencies

        public object ContentMain
        {
            get { return (object)GetValue(ContentMainProperty); }
            set { SetValue(ContentMainProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentMain.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentMainProperty =
            DependencyProperty.Register(nameof(ContentMain), typeof(object), typeof(hExpander), new PropertyMetadata(null, ContentMainPropertyChanged));

        private static void ContentMainPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;
            if (ido != null)
            {
                ido.ContentMain = e.NewValue;
            }
        }

        public object ContentHeader
        {
            get { return (object)GetValue(ContentHeaderProperty); }
            set { SetValue(ContentHeaderProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ContentHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentHeaderProperty =
            DependencyProperty.Register(nameof(ContentHeader), typeof(object), typeof(hExpander), new PropertyMetadata(ContentHeaderPropertyChanged));
        private static void ContentHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            hExpander ido = d as hExpander;

            if (ido != null)
            {
                ido.ContentHeader = e.NewValue;
            }
        }



        #endregion

        public hExpander()
        {
            InitializeComponent();
        }

        #region Events

        private void BtnToggle_Click(object sender, RoutedEventArgs e)
        {

            ToggleSelection();
            e.Handled = true; //Ensure that rest of the click events are not invoked.
        }

        #endregion

        #region Command Methods

        private void ToggleSelection()
        {
            switch(IsSelected)
            {
                case true:
                    IsSelected = false;
                    break;
                case false:
                    IsSelected = true;
                    break;
            }
        }

        #endregion
    }
}
