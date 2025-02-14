using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Models;

namespace File_Processor.Services
{
    class CategoryMergedService
    {
        public CategoryMergedService() { }

        public List<CategoryMergedModel> getCategoryAndClassifications()
        {
            using ( var context = new DbDefinition() )
            {
                var combinedData = context.Categories
                    .GroupJoin(
                        context.CategoriesClassification,
                        c1 => c1.category,
                        c2 => c2.category,
                        (c1, c2) => new CategoryMergedModel(
                            c1.category, 
                            c1.filePath, 
                            c2.Select( c => c.pattern ).ToList(),
                            c1.priority
                        )
                    ).ToList();

                //foreach ( CategoryMergedModel c in combinedData )
                //{
                //    Console.WriteLine(c.ToString() + "\n==========================");
                //}
                return combinedData;
            }
        }
    }
}
