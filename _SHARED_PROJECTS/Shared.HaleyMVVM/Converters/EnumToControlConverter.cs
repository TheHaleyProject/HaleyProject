using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Reflection;
using System.ComponentModel;
using Haley.Abstractions;
using Haley.Enums;

namespace Haley.MVVM.Converters
{
    public class EnumToControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int param = 0; //Sometimes users can choose not to enter parameter value, in such cases, we make 1 as default.
                if (parameter != null) int.TryParse((string)parameter, out param);
                InstanceGeneration _newinstance = InstanceGeneration.None;
                switch(param)
                {
                    //None
                    case 0:
                        _newinstance = InstanceGeneration.None;
                        break;
                        //TargetOnly
                    case 1:
                        _newinstance = InstanceGeneration.TargetOnly;
                        break;
                    case 2:
                        _newinstance = InstanceGeneration.TargetWithProperties;
                        break;
                    //All level
                    case 3:
                        _newinstance = InstanceGeneration.TargetWithAllDependencies;
                        break;
                }
                return ContainerStore.Singleton.controls.generateView((Enum)value, instance_level: _newinstance);
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
