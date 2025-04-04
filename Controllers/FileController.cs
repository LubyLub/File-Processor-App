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
            FileLogModel log = new FileLogModel(file.filePath.Substring(0, file.filePath.LastIndexOf('\\')));

            if (!Directory.Exists(log.sourcePath)) { log.error = true; return log; }

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

            return log;
        }

        public FileLogModel ProcessFileStage2(FileModel file, FileLogModel log)
        {
            //Find related categories
            log.flaggedCategories = _service.categorizeFile(file);

            return log;
        }

        public FileLogModel ProcessFileStage3(FileModel file, FileLogModel log)
        {
            int numOfFilesWithSameName = _service.filesWithSameName(log.destinationPath, file);
            String expectedFinalFilePath = log.destinationPath + "\\";
            string tempName = file.fileName;

            if (log.sourcePath.Equals(log.destinationPath)) 
            {
                tempName = (numOfFilesWithSameName + 1) + file.fileName;
                _service.moveFile(log.sourcePath + "\\" + file.fileName, log.sourcePath + "\\" + tempName);
            }

            //Deduplicate file based on its destination address
            if (Properties.Settings.Default.Deduplication)
            {
                try
                {
                    _service.deduplicationFile(file, log, tempName);
                }
                catch (Exception e)
                {
                    log.error = true;
                    return log;
                }
            }

            if ((log.deduplicated && Properties.Settings.Default.UseFileNameDeduplication) || numOfFilesWithSameName < 2) { expectedFinalFilePath += file.fileName; }
            else { expectedFinalFilePath += (numOfFilesWithSameName + 1) + file.fileName; }

            log.error = _service.moveFile(log.sourcePath + "\\" + tempName, expectedFinalFilePath);

            return log;
        }

        public FileModel FileToFileModel(FileInfo fileInfo)
        {
            return _service.PathToFileModel(fileInfo.FullName, fileInfo.Extension);
        }

        public bool deleteMaliciousFile(FileModel file)
        {
            return _service.deleteMaliciousFile(file);
        }

        public bool validateFile(FileModel file)
        {
            return _service.validateFile(file);
        }
    }
}
