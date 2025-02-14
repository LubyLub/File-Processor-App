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
        public int type { get; set; } // Type == 0 is Regex, Type == 1 is Keyword

        public CategoryClassificationModel(string category, string pattern)
        {
            this.category = category;
            this.pattern = pattern;
            this.type = 0;
        }

        public CategoryClassificationModel(string category, string pattern, int type) 
        { 
            this.category = category;
            this.pattern = pattern;
            this.type = type;
        }

        public CategoryClassificationModel(string category, string pattern, int type, CategoryModel categoryModel) 
        {
            this.category = category;
            this.pattern = pattern;
            this.type = type;
            this.categoryModel = categoryModel;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            CategoryClassificationModel other = (CategoryClassificationModel)obj;
            return this.category.Equals(other.category) && this.pattern.Equals(other.pattern);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return (this.category + "|" + this.pattern).GetHashCode();
        }
    }
}
