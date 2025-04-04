using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using File_Processor.Controllers;
using File_Processor.Models;
using IO = System.IO;

namespace File_Processor.Services
{
    internal abstract class FileProcessor
    {
        private protected CategoryMergedController categoryMergedController;
        internal FileProcessor()
        {
            categoryMergedController = new CategoryMergedController();
        }

        public List<CategoryMergedModel> categorizeFile(FileModel file)
        {
            List<CategoryMergedModel> categories = categoryMergedController.getCategories();
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

        public bool deduplicationFile(FileModel file, FileLogModel log, string tempFileName)
        {
            if (file.fileName.Equals(tempFileName)) { tempFileName = ""; }
            DirectoryModel directory = new DirectoryModel(log.destinationPath, "");
            var filesInDirectory = Directory.GetFiles(directory.directoryPath).Select(f => new FileInfo(f));

            bool deduplicated = false;
            bool useName = Properties.Settings.Default.UseFileNameDeduplication;
            bool useContent = Properties.Settings.Default.UseFileContentDeduplication;

            foreach (FileInfo fileInfo in filesInDirectory)
            {
                bool delete = false;
                if (useName)
                {
                    if (file.fileName.Equals(fileInfo.Name))
                    {
                        delete = true;
                    }
                }
                if (useContent)
                {
                    if (file.fileHash.Equals(FileToHash(fileInfo.FullName)))
                    {
                        if (!fileInfo.Name.Equals(tempFileName)) { delete = true; }
                    }
                }

                if (delete)
                {
                    File.Delete(fileInfo.FullName);
                    log.deleteFiles.Add(new FileModel(fileInfo.FullName));
                    log.deduplicated = true;
                }
            }

            return deduplicated;
        }

        private protected abstract String readWholeFile(string path);

        private protected IEnumerable<String> readFile(string path)
        {
            return IO.File.ReadLines(path);
        }

        internal string FileToHash(string path)
        {
            using (var md5 = MD5.Create())
            using (var stream = IO.File.OpenRead(path))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "");
            }
        }
    }
}
