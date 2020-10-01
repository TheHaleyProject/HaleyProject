using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Haley.MVVM;
using Haley.MVVM.Models;
using Test.Events;
using Test.ViewModels;

namespace Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MainVM _vm = new MainVM();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _configure();
            ContainerStore.windows.showDialog(_vm);
        }

        void objectdeleted(string obj)
        {
            _vm.message = $@"{obj} has been selected at {DateTime.Now.ToString()}";
        }
        void objectselected()
        {
            _vm.message = $@"Object has been selected at {DateTime.Now.ToString()}";
        }
        private void _configure()
        {
            //Log Configuration
            //Events Configuration
            EventStore.Singleton.GetEvent<ObjectDeletedEvent>().subscribe(objectdeleted);
            EventStore.Singleton.GetEvent<ObjectSelectedEvent>().subscribe(objectselected);
            //Container Configuration
            ContainerStore.windows.register<MainVM, MainWindow>();
        }
    }
}
