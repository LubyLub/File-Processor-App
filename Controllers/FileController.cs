using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;
using File_Processor.Models;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.CompilerServices;
using File_Processor.Services;

namespace File_Processor.Controllers
{
    internal class FileController
    {
        private FileService _service;

        public FileController()
        {
            _service = new FileService();
        }

        public string ProcessFile(FileModel file)
        {
            //Remember to implement a logging system using either strings or a Log class
            if (Properties.Settings.Default.Security && Properties.Settings.Default.MalwareAnalysis) { _service.malwareAnalysisOfFile(file); }
            _service.categorizeFile(file);
            if (Properties.Settings.Default.Deduplication) { _service.deduplicationFile(file); }
            if (Properties.Settings.Default.Security && Properties.Settings.Default.FileEncryption) { _service.encryptFile(file); }
            return "";
        }

        public FileModel FileToFileModel(FileInfo fileInfo)
        {

            return _service.PathToFileModel(fileInfo.FullName);
        }
    }
}
