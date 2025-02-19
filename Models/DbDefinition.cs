using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace File_Processor.Models
{
    internal class DbDefinition : DbContext
    {
        internal DbSet<CategoryModel> Categories { get; set; }
        internal DbSet<CategoryClassificationModel> CategoriesClassification { get; set; }
        internal DbSet<DirectoryModel> Directories { get; set; }

        internal string DbPath { get; }
        public DbDefinition() 
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            // var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "fileProcessor.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
            
        }
            

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryClassificationModel>()
                .HasOne(cc => cc.categoryModel)
                .WithMany(c => c.classifications)
                .HasForeignKey(cc => cc.category)
                .IsRequired();
        }
    }
}
