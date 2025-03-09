using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Processor.Models
{
    public class FileLogModel
    {
        public bool isMalicious {  get; set; }
        public string sourcePath { get; set; }
        public string destinationPath { get; set; }
        public List<CategoryMergedModel> flaggedCategories { get; set; }
        public bool error { get; set; }
        public string message { get; set; }
        public bool delete { get; set; }

        public FileLogModel() : this("")
        {
            
        }

        public FileLogModel(string source)
        {
            isMalicious = false;
            sourcePath = source;
            destinationPath = "";
            flaggedCategories = new List<CategoryMergedModel>();
            error = false;
            delete = false;
            message = "";
        }
    }
}
