﻿using System;
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

        public override bool deduplicationFile(FileModel file, String destinationDirectory)
        {
            DirectoryModel directory = new DirectoryModel(destinationDirectory, "");
            var filesInDirectory = Directory.GetFiles(directory.directoryPath).Select(f => new FileInfo(f));

            bool deduplicated = false;
            bool useName = Properties.Settings.Default.UseFileNameDeduplication;
            bool useContent = Properties.Settings.Default.UseFileContentDeduplication;

            foreach (FileInfo fileInfo in filesInDirectory)
            {
                if (useName)
                {
                    if (file.fileName.Equals(fileInfo.Name))
                    {
                        File.Delete(fileInfo.FullName);
                        deduplicated = true;
                    }
                }
                if (useContent)
                {
                    if (file.fileHash.Equals(FileToHash(fileInfo.FullName)))
                    {
                        File.Delete(fileInfo.FullName);
                        deduplicated = true;
                    }
                }
            }

            return deduplicated;
        }
    }
}
