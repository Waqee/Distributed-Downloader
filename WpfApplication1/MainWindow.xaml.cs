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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Task> tasks = new List<Task>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddDownload(String url)
        {

            var item2 = new TreeViewItem();
            var panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            AddDownloadDetail(panel, url);
            item2.Header = panel;
            DownloadTree.Items.Add(item2);

            panel.Name = "asdf";

            Task.Run(() => new Downloader(this, panel).Download(
                url,
                "download.zip"
                )); 
            
        }
        
        public void UpdateDownloadProgress(StackPanel item, String _progress)
        {
            Label progress = item.Children.OfType<Label>().ElementAt(2);
            progress.Content = _progress;
        }

        void UpdateDownloadDetail(StackPanel item, String _size, String _progress)
        {
            Label progress = item.Children.OfType<Label>().ElementAt(2);
            progress.Content = _progress;

            Label size = item.Children.OfType<Label>().ElementAt(1);
            size.Content = _size;
        }

        void AddDownloadDetail(StackPanel item, String _name)
        {
            StackPanel panel = item;
            
            var name = new Label();
            name.Content = _name;
            name.Width = 200;

            var size = new Label();
            size.Content = "";
            size.Width = 200;

            var progress = new Label();
            progress.Content = "";
            progress.Width = 200;

            var speed = new Label();
            speed.Content = "";
            speed.Width = 100;

            panel.Children.Add(name);
            panel.Children.Add(size);
            panel.Children.Add(progress);
            panel.Children.Add(speed);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newForm = new AddDownload(this); //create your new form.
            newForm.Show();
        }

    }
}
