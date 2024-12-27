using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Models;

namespace File_Processor.Services
{
    internal class FileService
    {
        private FileModel fileModel {  get; set; }

        internal FileService(FileModel file) { fileModel = file; }



    }
}
