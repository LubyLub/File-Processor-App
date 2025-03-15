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
using File_Processor.Exceptions;

namespace File_Processor.Controllers
{
    internal class FileController
    {
        private FileService _service;

        public FileController()
        {
            _service = new FileService();
        }

        public async Task<FileLogModel> ProcessFileStage1(FileModel file)
        {
            //Remember to implement a logging system using either strings or a Log class
            FileLogModel log = new FileLogModel(file.filePath.Substring(0, file.filePath.LastIndexOf('\\')));

            //Run Malware Analysis
            if (Properties.Settings.Default.Security && Properties.Settings.Default.MalwareAnalysis) 
            {
                try
                {
                    log.isMalicious = await _service.malwareAnalysisOfFile(file);
                }
                catch (NoInternetConnectionException e)
                {
                    log.error = true;
                }
            }

            //Find related categories
            if (!log.error) { log.flaggedCategories = _service.categorizeFile(file); }
            else { log.flaggedCategories = new List<CategoryMergedModel>(); }

                return log;
        }

        public FileLogModel ProcessFileStage2(FileModel file, FileLogModel log)
        {
            int numOfFilesWithSameName = _service.filesWithSameName(log.destinationPath, file);
            String expectedFinalFilePath = log.destinationPath + "\\";

            //Deduplicate file based on its destination address
            if (Properties.Settings.Default.Deduplication)
            {
                try
                {
                    log.deduplicated = _service.deduplicationFile(file, log.destinationPath);
                }
                catch (Exception e)
                {
                    log.error = true;
                    return log;
                }
            }

            if ((log.deduplicated && Properties.Settings.Default.UseFileNameDeduplication) || numOfFilesWithSameName == 0) { expectedFinalFilePath += file.fileName; }
            else { expectedFinalFilePath += (numOfFilesWithSameName + 1) + file.fileName; }

            log.error = _service.moveFile(log.sourcePath + "\\" + file.fileName, expectedFinalFilePath);

            return log;
        }

        public FileModel FileToFileModel(FileInfo fileInfo)
        {
            return _service.PathToFileModel(fileInfo.FullName, fileInfo.Extension);
        }
    }
}
