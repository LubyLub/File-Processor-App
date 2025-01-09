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
using File_Processor.Controllers;

namespace File_Processor.Views
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        //private CategoryController categoryController;
        public Page2()
        {
            InitializeComponent();
            //categoryController = new CategoryController(this);
        }

        private void Change_To_Main_Page(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.MainFrame.Source = new Uri("Page1.xaml", UriKind.Relative);
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow win = new AddCategoryWindow();
            win.Show();
        }
    }
}
