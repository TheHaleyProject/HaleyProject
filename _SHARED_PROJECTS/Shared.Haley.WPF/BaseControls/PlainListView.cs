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
using Haley.Abstractions;
using Haley.Utils;
using Haley.Enums;
using System.Collections;
using System.ComponentModel;

namespace Haley.WPF.BaseControls
{
    public class PlainListView : ListView,ICornerRadius
    {
        static PlainListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlainListView), new FrameworkPropertyMetadata(typeof(PlainListView)));
        }

        public PlainListView() 
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlainListView), new PropertyMetadata(ResourceStore.cornerRadius));

        public Brush ItemSelectedColor
        {
            get { return (Brush)GetValue(ItemSelectedColorProperty); }
            set { SetValue(ItemSelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemSelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSelectedColorProperty =
            DependencyProperty.Register(nameof(ItemSelectedColor), typeof(Brush), typeof(PlainListView), new PropertyMetadata(null));

        public Brush ItemHoverColor
        {
            get { return (Brush)GetValue(ItemHoverColorProperty); }
            set { SetValue(ItemHoverColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemHoverColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemHoverColorProperty =
            DependencyProperty.Register(nameof(ItemHoverColor), typeof(Brush), typeof(PlainListView), new PropertyMetadata(null));

        //public IEnumerable ChoosenItems
        //{
        //    get { return (IEnumerable)GetValue(ChoosenItemsProperty); }
        //    set { SetValue(ChoosenItemsProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ChoosenItems.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ChoosenItemsProperty =
        //    DependencyProperty.Register(nameof(ChoosenItems), typeof(IEnumerable), typeof(PlainListView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ChoosenItemsPropertyChanged));

        //protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        //{
        //    base.OnSelectionChanged(e);
        //    ChoosenItems = null;
        //}
        //static void ChoosenItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var plsv = (PlainListView)d;
        //    if (plsv == null) return;
        //    if (plsv.ChoosenItems == plsv.SelectedItems) return;

        //    plsv.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => plsv.ChoosenItems = plsv.SelectedItems));
        //}
    }
}
