using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Processor.Models
{
    class CategoryMergedViewModel
    {
        public String category { get; set; }
        public String filePath { get; set; }
        public int priority { get; set; }
        public String patternsToText { get; set; }
        private const int maxLength = 30;
        public CategoryMergedViewModel(String name, String path, List<String> patternList, int priority) 
        { 
            this.category = name;
            this.filePath = path;
            this.priority = priority;
            this.patternsToText = PatternsToText(patternList);
        }

        private static String PatternsToText(List<String> patterns) 
        {
            String output = "";
            if (patterns.Count != 0)
            {
                int len = patterns.First().Length;
                if (len < maxLength)
                {
                    output += patterns.First();
                }
                for (int i = 1; i < patterns.Count; i++) 
                {
                    String pattern = patterns[i];
                    if (len + pattern.Length < maxLength)
                    {
                        output += ", " + pattern;
                        len += pattern.Length;
                    }
                    else
                    {
                        output += "...";
                        break;
                    }
                }
            }
            return output;
        }
    }
}
