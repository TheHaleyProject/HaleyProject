using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Haley.Abstractions;
using Haley.MVVM;
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
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ContainerStore.Singleton.windows.register<CoreVM, MainWindow>();
            ContainerStore.Singleton.controls.register<VMSubMain, ctrl02>(TestApp.control02);
            ContainerStore.Singleton.controls.register<VMMain, ctrl01>(TestApp.control01);
            ContainerStore.Singleton.controls.register<VMSubMain, ctrl03>();

            ContainerStore.Singleton.windows.showDialog<CoreVM>();
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
        Control4,
        control5
    }

    
}
