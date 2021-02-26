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

namespace Haley.WPF.BaseControls
{
    public class PlainExpander : Expander , IShadow, ICornerRadius
    {
        static PlainExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainExpander), new FrameworkPropertyMetadata(typeof(PlainExpander)));
        }



        #region Shadow
        public bool ShadowOnlyOnMouseOver
        {
            get { return (bool)GetValue(ShadowOnlyOnMouseOverProperty); }
            set { SetValue(ShadowOnlyOnMouseOverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowOnlyOnMouseOver.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowOnlyOnMouseOverProperty =
            DependencyProperty.Register(nameof(ShadowOnlyOnMouseOver), typeof(bool), typeof(PlainExpander), new PropertyMetadata(true));

        public bool ShowShadow
        {
            get { return (bool)GetValue(ShowShadowProperty); }
            set { SetValue(ShowShadowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowShadow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowShadowProperty =
            DependencyProperty.Register(nameof(ShowShadow), typeof(bool), typeof(PlainExpander), new FrameworkPropertyMetadata(false));

        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShadowColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register(nameof(ShadowColor), typeof(Color), typeof(PlainExpander), new FrameworkPropertyMetadata(GlobalProps.shadowColor));
        #endregion

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainExpander), new FrameworkPropertyMetadata(GlobalProps.cornerRadius));
    }
}
