using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using File_Processor.Models;
using System.IO;

namespace File_Processor.Services
{
    internal class GeneralProcessor : FileProcessor
    {
        public override List<CategoryMergedModel> categorizeFile(FileModel file)
        {
            List<CategoryMergedModel> categories = categoryMergedService.getCategoryAndClassifications();
            List<CategoryMergedModel> flaggedCategories = new List<CategoryMergedModel>();
            String fileContents = readWholeFile(file.filePath);
            foreach (CategoryMergedModel c in categories)
            {
                for (int i = 0; i < c.patterns.Count; i++)
                {
                    string pattern = c.patterns[i];
                    if (c.types[i] == ((int)PatternType.Keyword))
                    {
                        if (fileContents.Contains(pattern)) { flaggedCategories.Add(c); break; }
                    }
                    else
                    {
                        Regex r = new Regex(pattern);
                        Match m = r.Match(fileContents);
                        
                        if (m.Success) { flaggedCategories.Add(c); break; }
                    }
                }
            }

            return flaggedCategories;
        }
    }
}
