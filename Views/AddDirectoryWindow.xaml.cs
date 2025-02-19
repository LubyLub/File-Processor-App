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
using File_Processor.Controllers;

namespace File_Processor.Views
{
    /// <summary>
    /// Interaction logic for AddDirectoryWindow.xaml
    /// </summary>
    public partial class AddDirectoryWindow : Window
    {
        private DirectoryController directoryController;
        private Page2 _page;
        public AddDirectoryWindow(Page2 page2)
        {
            InitializeComponent();
            directoryController = new DirectoryController();
            _page = page2;
        }

        private void AddDirectory(object sender, RoutedEventArgs e)
        {
            string name = directoryName.Text;
            string path = directoryPath.Text;
            bool result = directoryController.AddDirectory(path, name);
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)this.Owner;
            mainWindow.UnlockWindow();
            _page.LoadDirectories();
        }
    }
}
