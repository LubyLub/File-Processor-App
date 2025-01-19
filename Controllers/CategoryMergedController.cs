using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Services;
using File_Processor.Models;

namespace File_Processor.Controllers
{
    class CategoryMergedController
    {
        private CategoryMergedService _service;

        public CategoryMergedController()
        {
            _service = new CategoryMergedService();
        }

        public List<CategoryMergedViewModel> getCombinedData() 
        {
            List<CategoryMergedViewModel> output = new List<CategoryMergedViewModel>();
            List<CategoryMergedModel> combinedData = _service.getCategoryAndClassifications();
            foreach (CategoryMergedModel item in combinedData) 
            {
                output.Add(new CategoryMergedViewModel(item.category, item.filePath, item.patterns));
            }
            return output;
        }
    }
}
