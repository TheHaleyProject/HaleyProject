using Haley.Abstractions;
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
using Haley.MVVM;
using Test.Events;
using Test.ViewModels;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IHaleyWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            EventStore.Singleton.GetEvent<ObjectDeletedEvent>().subscribe(_somesubscription);
        }
        void _somesubscription(string obj)
        {
            var _thisvm = (MainVM)this.DataContext;
            _thisvm.message += "I'm called from windows";
        }
    }
}
