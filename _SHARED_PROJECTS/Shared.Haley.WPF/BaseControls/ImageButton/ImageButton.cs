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
using Haley.WPF.Abstractions;
using Haley.Utils;
using Haley.Models;
using Haley.Enums;

namespace Haley.WPF.BaseControls
{
    public class ImageButton : PlainButton , IImageHolder
    {
        #region Constructors
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageButton()
        {

        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //Process Images
            if (DefaultImage == null) DefaultImage = new BitmapImage(new Uri("pack://application:,,,/Haley.WPF;component/Images/empty_image.png"));
            if (HoverImage == null) HoverImage = DefaultImage;
            if (PressedImage == null) PressedImage = HoverImage;

            //TODO : REMEMBER TO IMPLEMENT A CACHE DICTIONARY TO ENSURE THAT THE IMAGES ARE NOT REPEATEDLY PROCESSED AND CREATED.
            //Process Image Colors
            if (DefaultImageColor != null)
            { DefaultImage = changeColor(DefaultImage,DefaultImageColor);}

            if (HoverImageColor != null)
            { HoverImage = changeColor(HoverImage, HoverImageColor); }

            if(PressedImageColor != null)
            { PressedImage = changeColor(PressedImage, PressedImageColor); }
        }

        #region Helpers
        private static ImageSource changeColor(ImageSource source,SolidColorBrush brush)
        {
            try
            {
                //IMPLEMENT A CACHE TO STORE ALREADY CHANGED IMAGES.
                var newcolor = brush.Color;
                var imageinfo = ImageUtils.getImageInfo(source);
                var res = ImageUtils.changeImageColor(imageinfo, int.Parse(newcolor.R.ToString()), int.Parse(newcolor.G.ToString()), int.Parse(newcolor.B.ToString()));
                return res;
            }
            catch (Exception ex)
            {
                return source; //In case of error, just reuse the source image itself.
            }
        }

        private static void changeColor(string propname, DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null) return;
            var _newcolor = e.NewValue as SolidColorBrush;
            ImageButton btn = d as ImageButton;
            if (_newcolor == null || btn == null) return;

            ImageSource _newsource = null;
            switch (propname)
            {
                case nameof(DefaultImage):
                    if (btn.DefaultImage != null)
                    {
                        _newsource=changeColor(btn.DefaultImage, _newcolor);
                        if (_newsource != null) btn.DefaultImage = _newsource;
                    }
                    break;
                case nameof(HoverImage):
                    if (btn.HoverImage != null)
                    {
                        _newsource = changeColor(btn.HoverImage, _newcolor);
                        if (_newsource != null) btn.HoverImage = _newsource;
                    }
                    break;
                case nameof(PressedImage):
                    if (btn.PressedImage != null)
                    {
                        _newsource = changeColor(btn.PressedImage, _newcolor);
                        if (_newsource != null) btn.PressedImage = _newsource;
                    }
                    break;
            }
          
        }
        #endregion

        #region Images
        public ImageSource DefaultImage
        {
            get { return (ImageSource)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultImageProperty =
            DependencyProperty.Register(nameof(DefaultImage), typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null));

        public ImageSource HoverImage
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.Register(nameof(HoverImage), typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null));

        public ImageSource PressedImage
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedImageProperty =
            DependencyProperty.Register(nameof(PressedImage), typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null));

        #endregion

        #region ImageColors
        public SolidColorBrush DefaultImageColor
        {
            get { return (SolidColorBrush)GetValue(DefaultImageColorProperty); }
            set { SetValue(DefaultImageColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultImageColorProperty =
            DependencyProperty.Register(nameof(DefaultImageColor), typeof(SolidColorBrush), typeof(ImageButton), new FrameworkPropertyMetadata(_defaultColorChanged));

        private static void _defaultColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            changeColor(nameof(DefaultImage), d, e);
        }

        public SolidColorBrush HoverImageColor
        {
            get { return (SolidColorBrush)GetValue(HoverImageColorProperty); }
            set { SetValue(HoverImageColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HoverImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HoverImageColorProperty =
            DependencyProperty.Register(nameof(HoverImageColor), typeof(SolidColorBrush), typeof(ImageButton), new FrameworkPropertyMetadata(_hoverColorChanged));

        private static void _hoverColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            changeColor(nameof(HoverImageColor), d, e);
        }

        public SolidColorBrush PressedImageColor
        {
            get { return (SolidColorBrush)GetValue(PressedImageColorProperty); }
            set { SetValue(PressedImageColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressedImageColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedImageColorProperty =
            DependencyProperty.Register(nameof(PressedImageColor), typeof(SolidColorBrush), typeof(ImageButton), new FrameworkPropertyMetadata(_pressedColorChanged));

        private static void _pressedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            changeColor(nameof(PressedImageColor), d, e);
        }
        #endregion
    }
}
