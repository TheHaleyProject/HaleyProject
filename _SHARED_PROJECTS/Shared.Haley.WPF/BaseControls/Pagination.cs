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
using Haley.Events;
using Haley.Abstractions;
using Haley.Utils;

namespace Haley.WPF.BaseControls
{
    [TemplatePart(Name = UIElementButtonFirst,Type = typeof(PlainButton))]
    [TemplatePart(Name = UIElementButtonLast, Type = typeof(PlainButton))]
    [TemplatePart(Name = UIElementButtonLeft, Type = typeof(PlainButton))]
    [TemplatePart(Name = UIElementButtonRight, Type = typeof(PlainButton))]
    [TemplatePart(Name = UIElementButtonMoreLeft, Type = typeof(PlainButton))]
    [TemplatePart(Name = UIElementButtonMoreRight, Type = typeof(PlainButton))]
    [TemplatePart(Name = UIElementButtonJump, Type = typeof(PlainButton))]
    public class Pagination : Control
    {
        #region Attributes
        private const string UIElementButtonLeft = "PART_btn_left";
        private const string UIElementButtonRight = "PART_btn_right";
        private const string UIElementButtonJump = "PART_btn_jump";
        private const string UIElementButtonFirst = "PART_btn_first";
        private const string UIElementButtonLast = "PART_btn_last";
        private const string UIElementButtonMoreLeft = "PART_btn_more_left";
        private const string UIElementButtonMoreRight = "PART_btn_more_right";
        #endregion

        #region Events
        public static readonly RoutedEvent UpdatedEvent = EventManager.RegisterRoutedEvent(nameof(Updated), RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pagination));

        public event RoutedEventHandler Updated
        {
            add { AddHandler(UpdatedEvent, value); }
            remove { RemoveHandler(UpdatedEvent, value); }
        }
        #endregion

        static Pagination()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pagination), new FrameworkPropertyMetadata(typeof(Pagination)));
        }

        public Pagination() {}
         
    }
}
