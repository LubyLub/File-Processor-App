using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using File_Processor.Models;
using File_Processor.Services;

namespace File_Processor.Controllers
{
    class CategoryController
    {
        private MainWindow _view;
        private CategoryService _service;

        public CategoryController(MainWindow view)
        {
            _view = view;
            _service = new CategoryService();
        }

        public int addCategory(string path, string cat)
        {
            return _service.addCategoryToDb(new CategoryModel(path, cat));
        }
    }
}
