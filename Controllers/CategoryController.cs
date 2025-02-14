using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using File_Processor.Models;
using File_Processor.Services;

namespace File_Processor.Controllers
{
    class CategoryController
    {
        //private Page _page;
        //private MainWindow _view;
        private CategoryService _service;

        public CategoryController()
        {
            //_page = page;
            //_view = view;
            _service = new CategoryService();
        }

        public int addCategory(string path, string cat, int priority)
        {
            return _service.addCategoryToDb(new CategoryModel(path, cat, priority));
        }

        public bool RemoveCategory(string path, string cat)
        {
            return _service.DeleteCategoryFromDb(new CategoryModel(path, cat, 0));
        }
    }
}
