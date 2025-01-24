using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Services;
using File_Processor.Models;

namespace File_Processor.Controllers
{
    internal class CategoryClassificationController
    {
        //private MainWindow _view;
        private CategoryClassificationService _service;

        public CategoryClassificationController()
        {
            //_view = view;
            _service = new CategoryClassificationService();
        }

        public bool AddCategoryClassification(string category, string pattern)
        {
            return _service.AddCategoryClassificationToDb(category, pattern);
        }

        public bool RemoveCategoryClassification(string category, string pattern) 
        {
            return _service.DeleteCategoryClassificationFromDb(category, pattern);
        }

        public bool RemoveCategoryClassification(CategoryClassificationModel categoryClassification)
        {
            return _service.DeleteCategoryClassificationFromDb(categoryClassification.category, categoryClassification.pattern);
        }

        public List<CategoryClassificationModel> getClassifications(string category)
        {
            return _service.getClassficationsFromDb(category);
        }

        public bool AddCategoryClassification(CategoryClassificationModel categoryClassification)
        {
            return _service.AddCategoryClassificationToDb(categoryClassification.category, categoryClassification.pattern);
        }
    }
}
