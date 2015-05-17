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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for AddDownload.xaml
    /// </summary>
    public partial class AddDownload : Window
    {
        MainWindow prevWindow;

        public AddDownload(MainWindow prev)
        {
            InitializeComponent();
            prevWindow = prev;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            prevWindow.AddDownload(Url.Text);
            this.Close();
        }
    }
}
