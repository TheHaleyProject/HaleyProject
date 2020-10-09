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
using System.Threading;

namespace DevelopmentWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                MainWindow ms = new MainWindow();
                Thread t1 = new Thread(() => {
                    ContainerStore.Singleton.windows.register<CoreVM, MainWindow>();
                    ContainerStore.Singleton.controls.register<VMSubMain, ctrl02>(TestApp.control02);
                    ContainerStore.Singleton.controls.register<VMMain, ctrl01>(TestApp.control01);
                    ContainerStore.Singleton.controls.register<VMSubMain, ctrl03>();

                });
                t1.Start();
                t1.Join();

                Thread t3 = new Thread(() =>
                {
                    CoreVM vm = new CoreVM();
                    ContainerStore.Singleton.windows.showDialog<CoreVM>(vm);
                });
                t3.Start();
                t3.Join();

            }
            catch (Exception ex)
            {
                throw;
            }
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
