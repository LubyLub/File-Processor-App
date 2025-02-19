using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace File_Processor.Models
{
    internal class DirectoryModel
    {
        [Key] public string directoryPath { get; set; }
        public String directoryName { get; set; }

        public DirectoryModel(string directoryPath, String directoryName)
        {
            this.directoryPath = directoryPath;
            this.directoryName = directoryName;
        }

        public override string ToString()
        {
            return "Name: " + directoryName + " | Path: " + directoryPath;
        }
    }
}
