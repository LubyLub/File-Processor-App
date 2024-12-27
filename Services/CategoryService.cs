using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Models;

namespace File_Processor.Services
{
    class CategoryService
    {
        public CategoryService() { }

        public int addCategoryToDb(CategoryModel cat)
        {
            int result = 0;
            using (var db = new DbDefinition())
            {
                //var entityToUpdate = (from c in db.Categories
                //                      where c.category.Equals(cat.category)
                //                      select c).FirstOrDefault();

                var entityToUpdate = db.Categories.FirstOrDefault(e => e.category.Equals(cat.category));

                if (entityToUpdate != null)
                {
                    entityToUpdate.filePath = cat.filePath;
                    result = 2;
                }
                else 
                {
                    db.Add(cat); result = 1;
                }
                try { db.SaveChanges(); } catch { }
            }
            return result;
        }
    }
}
