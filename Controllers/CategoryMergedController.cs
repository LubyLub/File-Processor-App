using File_Processor.Services;
using File_Processor.Models;
using System.IO;

namespace File_Processor.Controllers
{
    class CategoryMergedController
    {
        private CategoryMergedService _service;
        private CategoryController categoryController;

        public CategoryMergedController()
        {
            _service = new CategoryMergedService();
            categoryController = new CategoryController();
        }

        public List<CategoryMergedModel> getCategories()
        {
            List<CategoryMergedModel> categories = _service.getCategoryAndClassifications();

            for (int i = categories.Count - 1; i >= 0; i--)
            {
                CategoryMergedModel cat = categories[i];
                if (!Directory.Exists(cat.filePath))
                {
                    categoryController.RemoveCategory(cat.filePath, cat.category);
                    categories.RemoveAt(i);
                }
            }

            return categories;
        }

        public List<CategoryMergedViewModel> getCombinedData() 
        {
            List<CategoryMergedViewModel> output = new List<CategoryMergedViewModel>();
            List<CategoryMergedModel> combinedData = _service.getCategoryAndClassifications();
            foreach (CategoryMergedModel item in combinedData) 
            {
                output.Add(new CategoryMergedViewModel(item.category, item.filePath, item.patterns, item.priority));
            }
            return output;
        }
    }
}
