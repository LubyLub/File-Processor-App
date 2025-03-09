using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Processor.Models
{
    public class CategoryMergedModel
    {
        public String category { get; set; }
        public String filePath { get; set; }
        public int priority { get; set; }
        public List<String> patterns { get; set; }
        public List<int> types { get; set; }

        public CategoryMergedModel(String name, String path, List<String> patternList, List<int> types, int priority) 
        { 
            this.category = name;
            this.filePath = path;
            this.priority = priority;
            this.patterns = patternList;
            this.types = types;
        }

        public override String ToString() 
        {
            String output = "Name:" + category + "\nPath: " + filePath + "\nPriority: " + priority + "\nPatterns: ";
            foreach (String pattern in patterns)
            {
                output += pattern + " ";
            }
            return output; 
        }
    }
}
