using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using File_Processor.Controllers;
using File_Processor.Models;

namespace File_Processor.Views
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private List<FileModel> files;
        private DateTime lastRefreshed;
        public Page1()
        {
            InitializeComponent();
            files = new List<FileModel>();
            lastRefreshed = Properties.Settings.Default.LastChecked;
        }

        private void Change_To_Setting_Page(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.MainFrame.Source = new Uri("Page2.xaml", UriKind.Relative);
        }

        private void Refresh_Files(object sender, RoutedEventArgs e)
        {
            DateTime temp = DateTime.Now;
            GetFiles(lastRefreshed);
            lastRefreshed = temp;
            //foreach (var file in files)
            //{
            //    Console.WriteLine(file);
            //}
        }
        
        private void GetFiles(DateTime dateTime)
        {
            string directory = @"C:\Users\LubLub\Downloads";
            DateTime dateTime1 = DateTime.MinValue;
            
            var newFiles = Directory.GetFiles(directory)
                .Select(f => new FileInfo(f))
                .Where(f => f.LastWriteTime >= lastRefreshed)
                .ToList();

            foreach (var file in newFiles)
            {
                Console.WriteLine(file.FullName);
            }
            //foreach (var file in newFiles)
            //{
            //    files.Add(file);
            //}
        }


    }
}
