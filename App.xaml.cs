using System.Configuration;
using System.Data;
using System.Windows;
using File_Processor.Models;
using Microsoft.EntityFrameworkCore;

namespace File_Processor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeDb();
            SetLastModified();
        }

        protected void OnExit(object sender, ExitEventArgs e)
        {
            
        }

        private void InitializeDb()
        {
            using (var db = new DbDefinition())
            {
                db.Database.Migrate();
            } 
        }

        private void SetLastModified()
        {
            if (File_Processor.Properties.Settings.Default.LastChecked.Year == 1 )
            {
                File_Processor.Properties.Settings.Default.LastChecked = DateTime.Now;
                File_Processor.Properties.Settings.Default.Save();
            }
        }
    }

}
