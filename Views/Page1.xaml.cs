using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            validateExistingFiles();
            LoadFiles();
        }
        internal void validateExistingFiles()
        {
            for (int i = files.Count - 1; i >= 0; i--)
            {
                FileModel file = files[i];
                if (!fileController.validateFile(file))
                {
                    files.RemoveAt(i);
                }
            }
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
                bool skipMaliciousFile = true;
                if (!file.ignore)
                {
                    var log = await fileController.ProcessFileStage1(file);

                    if (!log.error)
                    {
                        if (log.isMalicious)
                        {
                            bool result = await maliciousWindow(file, log);
                            if (result)
                            {
                                if (log.maliciousAction == 1) { skipMaliciousFile = false; }
                                if (log.maliciousAction == 2)
                                {
                                    if (!fileController.deleteMaliciousFile(file))
                                    {
                                        MessageBox.Show("File Could not be deleted");
                                    }
                                }
                            }
                        }
                        else { skipMaliciousFile = false; }
                    }


                    if (!skipMaliciousFile)
                    {
                        if (!log.error) { fileController.ProcessFileStage2(file, log); }

                        //Choose destination path of file based on return categories (log.flaggedCategories)
                        if (!log.error)
                        {
                            if (log.flaggedCategories.Count > 1)
                            {
                                //Create and await for window to select among multiple categories
                                bool result = await multipleCategoryWindow(file, log);
                                if (!result) { log.error = true; }
                            }
                            else if (log.flaggedCategories.Count == 1) { log.destinationPath = log.flaggedCategories[0].filePath; }
                            else { log.destinationPath = log.sourcePath; }
                        }
                        //End of destination path decision

                        if (!log.error)
                        {
                            fileController.ProcessFileStage3(file, log);
                            adjustFilesList(log);
                        }

                        //If either stage2 or stage3 cause a log.error, break
                        if (log.error)
                        {
                            MessageBox.Show("Error encountered while processing " + file.fileName);
                            break;
                        }
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
                Properties.Settings.Default.LastChecked = dateTimeOfClick;
            }
            files = new List<FileModel>();
            lastRefreshed = Properties.Settings.Default.LastChecked;
            LoadFiles();
            Properties.Settings.Default.Save();
        }

        private void adjustFilesList(FileLogModel log)
        {
            //Adjusts Files list based on deduplication preformed
            foreach (FileModel deletedFile in log.deleteFiles) {
                files.RemoveAll(f => new FileModelPathComparer().Equals(f, deletedFile));
                Console.WriteLine("Deleted " + deletedFile.filePath);
            }
        }

        private Task<bool> maliciousWindow(FileModel file, FileLogModel log)
        {
            MaliciousWindow win = new MaliciousWindow(file, log);
            var task = new TaskCompletionSource<bool>();
            win.Owner = Application.Current.MainWindow;
            win.Closed += (s, a) => { task.SetResult(true); };
            win.Show();
            win.Focus();
            return task.Task;
        }

        private Task<bool> multipleCategoryWindow(FileModel file, FileLogModel log)
        {
            DestinationCategoryWindow win = new DestinationCategoryWindow(file, log);
            var task = new TaskCompletionSource<bool>();
            win.Owner = Application.Current.MainWindow;
            win.Closed += (s, a) => { task.SetResult(true); };
            win.Show();
            win.Focus();
            return task.Task;
        }
    }
}
