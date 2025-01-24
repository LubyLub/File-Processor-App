using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Models;

namespace File_Processor.Services
{
    internal class CategoryClassificationService
    {
        public CategoryClassificationService() { }

        public bool AddCategoryClassificationToDb(string category, string pattern) 
        {
            // Implement adding of category classification
            // Make sure to restrict foreign key policy and add 1 always for key and 2 for regex
            bool result = false;
            using (var db = new DbDefinition())
            {
                var categoryExists = db.Categories.Find(category);
                if (categoryExists == null) { return false; }

                db.Add(new CategoryClassificationModel(category, pattern));
                result = true;

                try { db.SaveChanges(); } catch { result = false; }
            }

            return result;
        }

        public bool DeleteCategoryClassificationFromDb(string category, string pattern) 
        { 
            bool result = true;
            using (var db = new DbDefinition())
            {
                var entityToDelete = db.CategoriesClassification.Find(category, pattern);
                if (entityToDelete != null)
                {
                    db.Remove(entityToDelete);
                }

                try { db.SaveChanges(); } catch { result = false; }
            }

            return result;
        }

        public bool DeleteAllCategoryClassificationsFromDb(string categoryName)
        {
            bool result = false;
            using (var db = new DbDefinition())
            {
                var entitiesToDelete = db.CategoriesClassification.Where(e => e.category.Equals(categoryName)).ToList();
                if (entitiesToDelete != null && entitiesToDelete.Count != 0) 
                {
                    db.CategoriesClassification.RemoveRange(entitiesToDelete);
                    result = true;
                }

                try { db.SaveChanges(); } catch { result = false; }
            }

            return result;
        }

        public List<CategoryClassificationModel> getClassficationsFromDb(string categoryName)
        {
            List<CategoryClassificationModel> classificationData = null;
            using (var context = new DbDefinition())
            {
                classificationData = context.CategoriesClassification.Where(c => c.category.Equals(categoryName)).ToList();
                 
            }
            return classificationData;
        }
    }
}
