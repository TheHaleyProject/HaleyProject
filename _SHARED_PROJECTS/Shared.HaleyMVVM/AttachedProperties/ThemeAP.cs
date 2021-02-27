using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Haley.Abstractions;
using System.Windows;
using System.Windows.Controls;
using Haley.MVVM;
using Haley.Enums;
using Haley.Utils;

namespace Haley.Models
{
    public static class ThemeAP
    {
        public static Theme GetActiveTheme(DependencyObject obj)
        {
            return (Theme)obj.GetValue(ActiveThemeProperty);
        }

        public static void SetActiveTheme(DependencyObject obj, Theme value)
        {
            obj.SetValue(ActiveThemeProperty, value);
        }

        // Using a DependencyProperty as the backing store for ActiveTheme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveThemeProperty =
            DependencyProperty.RegisterAttached("ActiveTheme", typeof(Theme), typeof(ThemeAP), new PropertyMetadata(null,ActiveThemePropertyChanged));

        private static void ActiveThemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null || e.NewValue == null) return;
            Theme active = e.NewValue as Theme;
            if (active == null || active.new_theme_PackURI == null || active.old_theme_name == null || active.base_dictionary_name == null) return;

            ThemeLoader.changeTheme(d,active);
        }
    }
}
