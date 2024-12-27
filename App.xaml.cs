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
        }

        private void InitializeDb()
        {
            using (var db = new DbDefinition())
            {
                db.Database.Migrate();
            } 
        }
    }

}
