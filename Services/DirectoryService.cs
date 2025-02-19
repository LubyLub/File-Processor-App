using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Models;

namespace File_Processor.Services
{
    internal class DirectoryService
    {
        public bool AddDirectoryToDb(DirectoryModel dir)
        {
            //Returns true if item did not exist and was succesfully added, else returns false
            bool result = false;
            using (var db = new DbDefinition())
            {
                //var entityToUpdate = (from c in db.Categories
                //                      where c.category.Equals(cat.category)
                //                      select c).FirstOrDefault();
                var entityToUpdate = db.Directories.FirstOrDefault(d => d.directoryPath.Equals(dir.directoryPath));

                if (entityToUpdate == null)
                {
                    db.Directories.Add(dir); result = true;
                }

                try { db.SaveChanges(); } catch { result = false; }
            }
            return result;
        }

        public bool DeleteDirectoryFromDb(DirectoryModel dir)
        {
            //Returns True if item existed and was deleted, else returns false
            bool result = false;
            using (var db = new DbDefinition())
            {
                var entityToDelete = db.Directories.FirstOrDefault(d => d.directoryPath.Equals(dir.directoryPath));

                if (entityToDelete != null)
                {
                    db.Remove(entityToDelete);
                    result = true;
                }

                try { db.SaveChanges(); } catch { result = false; }
            }

            return result;
        }

        public List<DirectoryModel> GetDirectoriesFromDb()
        {
            List<DirectoryModel> directories = null;
            using (var context = new DbDefinition())
            {
                directories = context.Directories.ToList();

            }
            return directories;
        }
    }
}
