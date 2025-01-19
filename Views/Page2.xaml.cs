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
using File_Processor.Models;

namespace File_Processor.Views
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        //private CategoryController categoryController;
        private CategoryMergedController _mergedController;
        private DbDefinition _context;
        public Page2()
        {
            InitializeComponent();
            _context = new DbDefinition();
            _mergedController = new CategoryMergedController();
            //categoryController = new CategoryController(this);

            categoryDataGrid.AutoGenerateColumns = false;
            LoadCategories();
        }

        private void Change_To_Main_Page(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.MainFrame.Source = new Uri("Page1.xaml", UriKind.Relative);
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryWindow win = new AddCategoryWindow(this);
            win.Show();
        }

        internal void LoadCategories()
        {
            List<CategoryMergedViewModel> data = getFullCategory();
            categoryDataGrid.ItemsSource = data;
        }

        private List<CategoryMergedViewModel> getFullCategory()
        {
            return _mergedController.getCombinedData();
        }

        private void SettingTabChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = (TabControl) sender;
            if (tabControl != null) 
            {
                TabItem selectedTab = (TabItem) tabControl.SelectedItem;
                if (selectedTab != null) 
                {
                    switch (selectedTab.Header) 
                    {
                        case "Categories":
                            LoadCategories();
                            break;
                        case "Deduplication":
                            break;
                        case "Security":
                            break;
                        default:
                            break;
                    }
                }

            }
        }
    }
}
