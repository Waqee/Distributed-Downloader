using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
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
    public class Downloader
    {
        MainWindow window;
        StackPanel panel;

        public Downloader(MainWindow win, StackPanel pan)
        {
            window = win;
            panel = pan;
        }

        public async Task Download(string url, string saveAs)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
            var parallelDownloadSuported = response.Headers.AcceptRanges.Contains("bytes");
            var contentLength = response.Content.Headers.ContentLength ?? 0;

            if (parallelDownloadSuported)
            {
                const double numberOfParts = 5.0;
                var tasks = new List<Task>();
                var partSize = (long)Math.Ceiling(contentLength / numberOfParts);

                File.Create(saveAs).Dispose();

                for (var i = 0; i < numberOfParts; i++)
                {
                    var start = i * partSize + Math.Min(1, i);
                    var end = Math.Min((i + 1) * partSize, contentLength);

                    tasks.Add(
                        Task.Run(() => DownloadPart(url, saveAs, start, end))
                        );
                }
                await Task.WhenAll(tasks);

                Label progress = panel.Children.OfType<Label>().ElementAt(2);
                await progress.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() {progress.Content = "Completed"; }));

                progress = panel.Children.OfType<Label>().ElementAt(0);
                await progress.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { progress.Content = "Completed"; }));

                progress = panel.Children.OfType<Label>().ElementAt(1);
                await progress.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate() { progress.Content = "Completed"; }));
            }
        }

        private async void DownloadPart(string url, string saveAs, long start, long end)
        {
            using (var httpClient = new HttpClient())
            using (var fileStream = new FileStream(saveAs, FileMode.Open, FileAccess.Write, FileShare.Write))
            {
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                message.Headers.Add("Range", string.Format("bytes={0}-{1}", start, end));

                fileStream.Position = start;
                await httpClient.SendAsync(message).Result.Content.CopyToAsync(fileStream);
            }
        }
    }
}
