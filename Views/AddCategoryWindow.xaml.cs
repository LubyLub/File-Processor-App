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
using File_Processor.Models;

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
        private List<CategoryClassificationModel> dataList;
        private List<CategoryClassificationModel> addList;
        private List<CategoryClassificationModel> removeList;
        private readonly List<String> patternTypes = new List<string> { PatternType.Regex.ToString(), PatternType.Keyword.ToString() };
        private readonly List<String> priorityChoices = new List<string> { 0 + " - " + Priority.lowest, 1 + " - " + Priority.low, 2 + " - " + Priority.mid, 3 + " - " + Priority.high, 4 + " - " + Priority.highest, 5 + " - " + Priority.sensitive };
        public AddCategoryWindow(Page2 page2)
        {
            InitializeComponent();
            categoryController = new CategoryController();
            categoryClassificationController = new CategoryClassificationController();
            addList = new List<CategoryClassificationModel>();
            removeList = new List<CategoryClassificationModel>();
            patternType.ItemsSource = patternTypes;
            selectedPriority.ItemsSource = priorityChoices;
            selectedPriority.SelectedIndex = 0;
            _page = page2;

            GetClassifications();
        }

        public AddCategoryWindow(Page2 page2, String category, String filePath, Priority priority)
        {
            InitializeComponent();
            categoryController = new CategoryController();
            categoryClassificationController = new CategoryClassificationController();
            addList = new List<CategoryClassificationModel>();
            removeList = new List<CategoryClassificationModel>();
            patternType.ItemsSource = patternTypes;
            selectedPriority.ItemsSource = priorityChoices;
            _page = page2;

            this.Title = "Edit Category";
            this.process.Content = "Edit";
            this.categoryName.Text = category;
            this.categoryName.IsEnabled = false;
            this.categoryFilePath.Text = filePath;
            this.selectedPriority.SelectedIndex = (int) priority;
            GetClassifications();
        }

        private void Add_Category(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgResult = 0;
            string givenPath = categoryFilePath.Text; // Grabs Text in a TextBox named categoryFilePath
            string category = categoryName.Text;
            int priority = selectedPriority.SelectedIndex;
            // Adding Category
            int result = categoryController.addCategory(givenPath, category, priority);
            //Removing categories deleted
            bool classificationResult = RemoveCategoryClassifications();
            // Adding Category Classificagtion
            classificationResult = AddCategoryClassifications() && classificationResult;
            // Closing Act
            //if (result == 1) { msgResult = MessageBox.Show("Category \'" + category + ":" + givenPath + "\' Successfuly Added"); }
            //else if (result == 2) { msgResult = MessageBox.Show("Category \'" + category + ":" + givenPath + "\' Updated"); }
            //else if (result == 0 || !classificationResult) { msgResult = MessageBox.Show("Error incured"); }
            //if (msgResult != MessageBoxResult.None)
            //{
            //    Close();
            //}
            Close();
            //_page.LoadCategories();
        }
        private bool AddCategoryClassifications()
        {
            // Implement the addition of categories
            bool result = true;
            foreach (CategoryClassificationModel categoryClassification in addList)
            {
                result = result && categoryClassificationController.AddCategoryClassification(categoryClassification);
                if (result == false) { break; }
            }
            return result;
        }

        private bool RemoveCategoryClassifications()
        {
            bool result = true;
            foreach (CategoryClassificationModel categoryClassification in removeList)
            {
                result = result && categoryClassificationController.RemoveCategoryClassification(categoryClassification);
                if (result == false) { break; }
            }
            return result;
        }

        private String[] parseClassificationText(String str)
        {
            String[] output;
            output = str.Split(',');
            return output;
        }

        //private bool Add_CategoryClassification(string cat, String[] patternList)
        //{
        //    // Implement the addition of categories 
        //    bool result = true;
        //    foreach (String pattern in patternList)
        //    {
        //        result = result && categoryClassificationController.AddCategoryClassification(cat, pattern);
        //        if (result == false) { break; }
        //    }
        //    return result;
        //}

        private void GetClassifications()
        {
            String category = categoryName.Text;
            dataList = categoryClassificationController.getClassifications(category);
            LoadClassifications();
        }

        private void LoadClassifications()
        {
            classificationDataGrid.ItemsSource = null;
            classificationDataGrid.ItemsSource = dataList;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)this.Owner;
            mainWindow.UnlockWindow();
            _page.LoadCategories();
        }

        private void deleteClassification_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.Source;
            if (button != null)
            {
                DataGridRow row = GetParent<DataGridRow>(button);
                CategoryClassificationModel itemToRemove = (CategoryClassificationModel)row.DataContext;
                if (itemToRemove != null)
                {
                    //String cat = rowData.category;
                    //String pattern = rowData.pattern;
                    if (dataList.Contains(itemToRemove))
                    {
                        dataList.Remove(itemToRemove);
                        removeList.Add(itemToRemove);
                        addList.Remove(itemToRemove);
                    }
                    LoadClassifications();
                    //bool result = categoryClassificationController.RemoveCategoryClassification(cat, pattern);
                    //if (result)
                    //{
                    //    GetClassifications();
                    //}
                }
            }
        }
        private TargetType GetParent<TargetType>(DependencyObject o)
            where TargetType : DependencyObject
        {
            if (o == null || o is TargetType) return (TargetType)o;
            return GetParent<TargetType>(VisualTreeHelper.GetParent(o));
        }

        private void addPattern_Click(object sender, RoutedEventArgs e)
        {
            String category = categoryName.Text;
            PatternType type = (PatternType) this.patternType.SelectedIndex;
            String pattern = patternText.Text;
            CategoryClassificationModel itemToAdd = new CategoryClassificationModel(category, pattern, (int) type);
            if (!dataList.Contains(itemToAdd))
            {
                dataList.Add(itemToAdd);
                addList.Add(itemToAdd);
            }
            LoadClassifications();
            //bool result = categoryClassificationController.AddCategoryClassification(category, pattern);
            //if (!result)
            //{
            //    MessageBox.Show("Pattern cannot be added (Failed the process)");
            //}
        }

        private void test()
        {
            Console.WriteLine("dataList:\n=====================================");
            foreach (CategoryClassificationModel item in dataList)
            {
                Console.WriteLine(item.pattern);
            }
            Console.WriteLine("removeList:\n=====================================");

            foreach (CategoryClassificationModel item in removeList)
            {
                Console.WriteLine(item.pattern);
            }
            Console.WriteLine("addList:\n=====================================");
            foreach (CategoryClassificationModel item in addList)
            {
                Console.WriteLine(item.pattern);
            }
        }
    }
}
