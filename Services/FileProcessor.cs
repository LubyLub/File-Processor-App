using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Models;
using IO = System.IO;

namespace File_Processor.Services
{
    internal abstract class FileProcessor
    {
        private protected CategoryMergedService categoryMergedService;
        internal FileProcessor()
        {
            categoryMergedService = new CategoryMergedService();
        }

        public abstract List<CategoryMergedModel> categorizeFile(FileModel file);
        public bool deduplicationFile(FileModel file, string destinationDirectory, string tempFileName)
        {
            if (file.fileName.Equals(tempFileName)) { tempFileName = ""; }
            DirectoryModel directory = new DirectoryModel(destinationDirectory, "");
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
                    deduplicated = true;
                }
            }

            return deduplicated;
        }

        private protected String readWholeFile(string path)
        {
            return IO.File.ReadAllText(path);
        }

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
