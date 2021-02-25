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
using System.Security;

namespace Haley.WPF.BaseControls
{
    /// <summary>
    /// Sealed class that cannot be inherited.
    /// </summary>
    public sealed class PlainPasswordBox : PlainTextBox
    {
        //Passwordbox wll take the style template of the plaintext box.

        public PlainPasswordBox() 
        {
            base.TextChanged += PlainPasswordBox_TextChanged;
        }

        public SecureString SecurePassword { get; private set; }

        private void PlainPasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            //TO DO . Based on the text change, get each character that is typed and change append it with a secure password.
            //base.Text = "*";
        }
    }
}
