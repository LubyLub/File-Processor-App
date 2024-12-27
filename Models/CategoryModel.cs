using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Processor.Models
{
    internal class CategoryModel
    {
        public String filePath { get; set; }
        [Key] public String category { get; set; }
        public String? parentCategory { get; set; }
        public String? subCategory { get; set; }
        public virtual ICollection<CategoryClassificationModel> classifications { get; set; }

        //public CategoryModel() { }
        public CategoryModel(String filePath, String category, String parentCategory, String subCategory)
        {
            this.filePath = filePath;
            this.category = category;
            this.parentCategory = parentCategory;
            this.subCategory = subCategory;
            this.classifications = new List<CategoryClassificationModel>();
        }
        public CategoryModel(String filePath, String category)
        {
            this.filePath = filePath;
            this.category = category;
            this.parentCategory = null;
            this.subCategory = null;
            this.classifications = new List<CategoryClassificationModel>();
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            CategoryModel other = (CategoryModel) obj;
            return this.category.Equals(other.category);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return this.category.GetHashCode();
        }
    }
}
