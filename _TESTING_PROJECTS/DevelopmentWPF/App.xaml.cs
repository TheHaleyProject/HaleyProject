using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Haley.Flipper.MVVM.Interfaces;
using Haley.Flipper.MVVM.IOC;
using DevelopmentWPF.Controls;
using DevelopmentWPF.ViewModels;
using System.Windows.Data;
using System.Globalization;

namespace DevelopmentWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       public static UserControlService ControlIOCService { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ControlIOCService = new UserControlService();
            ControlIOCService.register<VMSubMain, ctrl01>(TestApp.control01);
            ControlIOCService.register<VMMain, ctrl02>(TestApp.control02);
            ControlIOCService.register<VMSubMain, ctrl03>(TestApp.control03);

            MainWindow wndiw = new MainWindow();
            wndiw.ShowDialog();
        }
    }

    public enum TestApp
    {
        none,
        control01,
        control02,
        control03
    }

    public enum TestApp02
    {
        helloVM,
        WhyVm
    }

    public class multiBindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return App.ControlIOCService.obtainControl(values[1], (Enum)values[0]);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
