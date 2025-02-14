using System;
using System.Collections.Generic;
using System.Data;
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
        private CategoryMergedController _mergedController;
        private CategoryController _categoryController;
        private DbDefinition _context;
        public Page2()
        {
            InitializeComponent();
            _context = new DbDefinition();
            _mergedController = new CategoryMergedController();
            _categoryController = new CategoryController();

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
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            win.Owner = mainWindow;
            mainWindow.LockWindow();
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
        

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.Source;
            if (button != null)
            {
                DataGridRow row = GetParent<DataGridRow>(button);
                CategoryMergedViewModel rowData = (CategoryMergedViewModel)row.DataContext;
                if (rowData != null)
                {
                    String path = rowData.filePath;
                    String cat = rowData.category;
                    bool result = _categoryController.RemoveCategory(path, cat);
                    if (result)
                    {
                        LoadCategories();
                    }
                }
            }
        }

        private void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.Source;
            if (button != null)
            {
                DataGridRow row = GetParent<DataGridRow>(button);
                CategoryMergedViewModel rowData = (CategoryMergedViewModel)row.DataContext;
                if (rowData != null)
                {
                    String path = rowData.filePath;
                    String cat = rowData.category;
                    Priority priority = (Priority) rowData.priority;
                    // Open Add Category Window but change titles
                    AddCategoryWindow win = new AddCategoryWindow(this, cat, path, priority);
                    MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                    win.Owner = mainWindow;
                    mainWindow.LockWindow();
                    win.Show();
                }
            }
        }

        private TargetType GetParent<TargetType>(DependencyObject o)
            where TargetType : DependencyObject
        {
            if (o == null || o is TargetType) return (TargetType)o;
            return GetParent<TargetType>(VisualTreeHelper.GetParent(o));
        }
    }
}
