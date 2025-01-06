using System.IO;
using System.Text;
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
using File_Processor.Views;

namespace File_Processor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private CategoryController categoryController;
        //private CategoryClassificationController categoryClassificationController;
        public MainWindow()
        {
            InitializeComponent();
            MainPage.Source = new Uri("Page1.xaml", UriKind.Relative);
            //categoryController = new CategoryController(this);
        }

        //private void Add_Category(object sender, RoutedEventArgs e)
        //{
        //    string givenPath = categoryFilePath.Text; // Grabs Text in a TextBox named manualFilePath"
        //    string category = categoryName.Text;
        //    // Adding Category
        //    int result = categoryController.addCategory(givenPath, category);
        //    if (result == 1) { MessageBox.Show("Category \'" + category + ":" + givenPath + "\' Successfuly Added"); }
        //    else if (result == 2) { MessageBox.Show("Category \'" + category + ":" + givenPath + "\' Updated"); }
        //    else { MessageBox.Show("Category not added"); }
        //}
        
        //private void Add_CategoryClassification(object sender, RoutedEventArgs e)
        //{ 
        //    // Implement the addition of categories    
        //}

        //private void Change_To_Setting_Page(object sender, RoutedEventArgs e) 
        //{
            
        //}
    }
}