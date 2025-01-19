using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace File_Processor.Models
{
    [PrimaryKey(nameof(category), nameof(pattern))]
    class CategoryClassificationModel
    {
        public CategoryModel categoryModel { get; set; }
        public string category { get; set; }
        public String pattern { get; set; }

        public CategoryClassificationModel(string category, string pattern) 
        { 
            this.category = category;
            this.pattern = pattern;
        }

        public CategoryClassificationModel(string category, string pattern, CategoryModel categoryModel) 
        {
            this.category = category;
            this.pattern = pattern;
            this.categoryModel = categoryModel;
        }
    }
}
