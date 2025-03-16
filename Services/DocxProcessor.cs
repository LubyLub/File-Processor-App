using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File_Processor.Models;

namespace File_Processor.Services
{
    internal class DocxProcessor : FileProcessor
    {
        public override List<CategoryMergedModel> categorizeFile(FileModel file)
        {
            throw new NotImplementedException();
        }
    }
}
