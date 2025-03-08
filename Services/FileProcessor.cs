using System;
using System.Collections.Generic;
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
        public abstract bool deduplicationFile(FileModel file);
        public abstract void encryptFile(FileModel file);
        //public abstract Task<bool> malwareAnalysisOfFile(FileModel file);

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
