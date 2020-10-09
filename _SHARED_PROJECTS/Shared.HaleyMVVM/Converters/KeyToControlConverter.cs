using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Reflection;
using System.ComponentModel;

namespace Haley.MVVM.Converters
{
    public class KeyToControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value.GetType() == typeof(string))) return null;
                int param = 0; //Sometimes users can choose not to enter parameter value, in such cases, we make 1 as default.
                if (parameter != null) int.TryParse((string)parameter, out param);
                bool flag = (param != 0); //If param is not zero, then true
                return ContainerStore.Singleton.controls.obtainControl((string)value, flag);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
   
}
