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
        public String fileName { get; set; }
        public ulong fileHash { get; set; }
        public DateTime lastModified { get; set; }
        public DateTime created { get; set; }

        internal FileModel(String filePath, ulong fileHash, String fileName, DateTime lastModified, DateTime created)
        {
            this.filePath = filePath;
            this.fileName = fileName;
            this.lastModified = lastModified;
            this.created = created;
            this.fileHash = fileHash;
        }
    }
}
