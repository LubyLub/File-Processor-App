using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Services;

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
    }
}
