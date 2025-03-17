using System.Windows;
using File_Processor.Models;

namespace File_Processor.Views
{
    /// <summary>
    /// Interaction logic for MaliciousWindow.xaml
    /// </summary>
    public partial class MaliciousWindow : Window
    {
        private FileLogModel log;
        private bool properClosing;
        public MaliciousWindow(FileModel file, FileLogModel log)
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            MaliciousLable.Content = "File: \"" + file.fileName + "\" has been flagged as\nMalicious. What to do with the file?";
            this.log = log;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!properClosing)
            {
                log.maliciousAction = 0;
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            log.maliciousAction = 2;
            properClosing = true;
            Close();
        }

        private void ProcessFile_Click(object sender, RoutedEventArgs e)
        {
            log.maliciousAction = 1;
            properClosing = true;
            Close();
        }

        private void IgnoreFile_Click(object sender, RoutedEventArgs e)
        {
            log.maliciousAction = 0;
            properClosing = true;
            Close();
        }
    }
}
