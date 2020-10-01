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
using System.Reflection;
using System.IO;
using Haley.Carnivora.Engima.Enums;
using Haley.Carnivora.Engima;

#pragma warning disable IDE1006 // Naming Styles
namespace HaleyTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void grdAssemblyInfo_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string[] _drop_data = (string[]) e.Data.GetData(DataFormats.FileDrop, false) ;

                var _extension = Path.GetExtension(_drop_data[0]);

                //Get PUBLIC KEY
                if (_extension == ".dll" || _extension == ".exe")
                {
                    Assembly _assembly = Assembly.LoadFrom(_drop_data[0]);
                    this.tblock_byte.Text = Convert.ToBase64String(_assembly.GetName().GetPublicKey());
                }
                else if(_extension ==".snk")
                {
                   byte[] _snk_bytes = File.ReadAllBytes(_drop_data[0]);
                    var snkpair = new StrongNameKeyPair(_snk_bytes);
                    var public_key = snkpair.PublicKey;
                    this.tblock_byte.Text = Convert.ToBase64String(public_key);
                }

                //GENERATE CHECK SUM
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(this.tblock_byte.Text);
        }

        private void btn_copy_file_hash_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_generate_bytes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _count;
                int.TryParse(this.tbox_bytes_count.Text, out _count);
                var kvp = EngimaAPI.Common.getBytes(_count);
                this.tblock_string_random_bytes.Text = Convert.ToBase64String(kvp.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_copy_bytes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.Clear();
                Clipboard.SetText(this.tblock_string_random_bytes.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_copy_rotate_value_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_rotate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool is_reverse = this.btnReverse.IsChecked ?? this.btnReverse.IsChecked.Value;
                switch (this.rbtn_rotate.IsChecked)
                {
                    case true:
                        this.tblock_rotateValue.Text = EngimaAPI.Special.rotate(this.tblock_rotateValue.Text, long.Parse(this.tbox_key.Text), is_reverse);
                        break;
                    case false:
                        this.tblock_rotateValue.Text = EngimaAPI.Special.swap(this.tblock_rotateValue.Text, long.Parse(this.tbox_key.Text), is_reverse);
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
#pragma warning disable IDE1006 // Naming Styles