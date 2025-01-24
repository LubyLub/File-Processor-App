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
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private CategoryController categoryController;
        private CategoryClassificationController categoryClassificationController;
        private Page2 _page;
        public AddCategoryWindow(Page2 page2)
        {
            InitializeComponent();
            categoryController = new CategoryController();
            categoryClassificationController = new CategoryClassificationController();
            _page = page2;
        }

        public AddCategoryWindow(Page2 page2, String category, String filePath)
        {
            InitializeComponent();
            categoryController = new CategoryController();
            categoryClassificationController = new CategoryClassificationController();
            _page = page2;

            this.Title = "Edit Category";
            this.process.Content = "Edit";
            this.categoryName.Text = category;
            this.categoryFilePath.Text = filePath;
        }

        private void Add_Category(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgResult = 0;
            string givenPath = categoryFilePath.Text; // Grabs Text in a TextBox named categoryFilePath
            string category = categoryName.Text;
            string classification = inputClassification.Text;
            // Adding Category
            int result = categoryController.addCategory(givenPath, category);
            // Adding Category Classificagtion
            String[] parsedClassification = parseClassificationText(classification);
            bool classificationResult = Add_CategoryClassification(category, parsedClassification);
            // Closing Act
            if (result == 1) { msgResult = MessageBox.Show("Category \'" + category + ":" + givenPath + "\' Successfuly Added"); }
            else if (result == 2) { msgResult = MessageBox.Show("Category \'" + category + ":" + givenPath + "\' Updated"); }
            else if (result == 0 || !classificationResult) { msgResult = MessageBox.Show("Error incured"); }
            if (msgResult != MessageBoxResult.None) 
            {
                Close();
            }
        }

        private String[] parseClassificationText(String str)
        {
            String[] output;
            output = str.Split(',');
            return output;
        }

        private bool Add_CategoryClassification(string cat, String[] patternList)
        {
            // Implement the addition of categories 
            bool result = true;
            foreach (String pattern in patternList)
            {
                result = result && categoryClassificationController.AddCategoryClassification(cat, pattern);
                if (result == false) { break; }
            }
            return result;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)this.Owner;
            mainWindow.UnlockWindow();
            _page.LoadCategories();
        }
    }
}
