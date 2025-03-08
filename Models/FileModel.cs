using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Processor.Models
{
    public class FileModel
    {
        public string filePath { get; set; }
        public string fileName { get; set; }
        public string fileHash { get; set; }
        public DateTime lastModified { get; set; }
        public DateTime created { get; set; }
        public bool ignore { get; set; }
        public string extension { get; set; }

        internal FileModel(string filePath, string fileHash, string fileName, string extension, DateTime lastModified, DateTime created)
        {
            this.filePath = filePath;
            this.fileName = fileName;
            this.lastModified = lastModified;
            this.created = created;
            this.fileHash = fileHash;
            this.extension = extension;
            this.ignore = false;
        }

        public override string ToString()
        {
            return "Name: " + fileName + " | Path: " + filePath + " | Last Modified: " + lastModified + " | Hash: " + fileHash;
        }
    }
}
