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
using File_Processor.Models;

namespace File_Processor.Views
{
    /// <summary>
    /// Interaction logic for DestinationCategoryWindow.xaml
    /// </summary>
    public partial class DestinationCategoryWindow : Window
    {
        private FileLogModel log;
        private bool properClosing;
        public DestinationCategoryWindow(FileModel file, FileLogModel log)
        {
            InitializeComponent();
            this.log = log;
            Title = "Choose category for file: " + file.fileName;
            properClosing = false;
            flaggedCategoriesDataGrid.AutoGenerateColumns = false;
            flaggedCategoriesDataGrid.SelectionMode = DataGridSelectionMode.Single;
            loadFlaggedCategories(log.flaggedCategories);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!properClosing)
            {
                log.destinationPath = log.flaggedCategories[0].filePath;
            }
        }

        private void loadFlaggedCategories(List<CategoryMergedModel> categories)
        {
            flaggedCategoriesDataGrid.ItemsSource = categories;
        }

        private void categorySelection_Click(object sender, RoutedEventArgs e)
        {
            CategoryMergedModel selectedRow = (CategoryMergedModel)flaggedCategoriesDataGrid.SelectedItem;
            if (selectedRow != null)
            {
                log.destinationPath = selectedRow.filePath;
                properClosing = true;
                Close();
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
