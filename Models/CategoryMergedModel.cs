using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Processor.Models
{
    class CategoryMergedModel
    {
        public String category { get; set; }
        public String filePath { get; set; }
        public List<String> patterns { get; set; }

        public CategoryMergedModel(String name, String path, List<String> patternList) 
        { 
            this.category = name;
            this.filePath = path;
            this.patterns = patternList;
        }

        public override String ToString() 
        {
            String output = "Name:" + category + "\nPath: " + filePath + "\nPatterns: ";
            foreach (String pattern in patterns)
            {
                output += pattern + " ";
            }
            return output; 
        }
    }
}
