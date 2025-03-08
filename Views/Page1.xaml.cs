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
        private DirectoryController directoryController;
        private FileController fileController;
        public Page1()
        {
            InitializeComponent();
            files = new List<FileModel>();
            directoryController = new DirectoryController();
            fileController = new FileController();
            lastRefreshed = Properties.Settings.Default.LastChecked;
            fileDataGrid.AutoGenerateColumns = false;

            LoadFiles();
        }

        private void Change_To_Setting_Page(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.MainFrame.Source = new Uri("Page2.xaml", UriKind.Relative);
        }

        private void Refresh_Files(object sender, RoutedEventArgs e)
        {
            LoadFiles();
        }
        internal void LoadFiles()
        {
            DateTime temp = DateTime.Now;
            GetFiles(lastRefreshed);
            lastRefreshed = temp;
            fileDataGrid.ItemsSource = null;
            fileDataGrid.ItemsSource = files;
        }

        private void GetFiles(DateTime dateTime)
        {
            List<DirectoryModel> directories = directoryController.GetDirectories();
            DateTime dateTime1 = DateTime.MinValue;

            foreach (DirectoryModel directory in directories)
            {
                var newFiles = Directory.GetFiles(directory.directoryPath)
                .Select(f => new FileInfo(f))
                .Where(f => f.LastWriteTime >= lastRefreshed)
                .ToList();

                foreach (var file in newFiles)
                {
                    files.Add(fileController.FileToFileModel(file));
                }
            }
        }

        private void IgnoreFile_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.Source;
            if (checkBox != null)
            {
                DataGridRow row = GetParent<DataGridRow>(checkBox);
                FileModel rowData = (FileModel)row.DataContext;

                if (rowData != null)
                {
                    rowData.ignore = !rowData.ignore;
                    fileDataGrid.ItemsSource = null;
                    fileDataGrid.ItemsSource = files;
                }
            }
        }

        private TargetType GetParent<TargetType>(DependencyObject o)
            where TargetType : DependencyObject
        {
            if (o == null || o is TargetType) return (TargetType)o;
            return GetParent<TargetType>(VisualTreeHelper.GetParent(o));
        }

        private async void ProcessFiles_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTimeOfClick = DateTime.Now;
            DateTime minDate = DateTime.MinValue;
            //Process the Files
            while (files.Count > 0)
            {
                FileModel file = files.First();
                if (!file.ignore)
                {
                    var log = await fileController.ProcessFileStage1(file);

                    //Choose destination path of file based on return categories (log.flaggedCategories)
                    if (log.flaggedCategories.Count > 1)
                    {
                        //Create and await for window to select among multiple categories
                    }
                    else if (log.flaggedCategories.Count == 1) { log.destinationPath = log.flaggedCategories[0].filePath + "\\" + file.fileName; }
                    else { log.destinationPath = log.sourcePath; }
                    //End of destination path decision

                    if (!log.error) { fileController.ProcessFileStage2(file, log); }
                    //If either stage1 or stage2 cause a log.error, break
                    if (log.error)
                    {
                        Console.WriteLine("hello");
                        break;
                    }
                }
                files.Remove(file);
            }

            //Reset File List, last refreshed and the user setting last checked
            if (files.Count != 0)
            {
                foreach (var file in files)
                {
                    if (file.lastModified < minDate) { minDate = file.lastModified; }
                }
                Properties.Settings.Default.LastChecked = minDate;
            }
            else
            {
                files = new List<FileModel>();
                Properties.Settings.Default.LastChecked = dateTimeOfClick;
            }
            lastRefreshed = Properties.Settings.Default.LastChecked;
            LoadFiles();
            //Properties.Settings.Default.Save();
        }
    }
}
