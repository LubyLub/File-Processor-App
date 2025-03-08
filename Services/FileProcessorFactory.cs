using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Processor.Services
{
    internal class FileProcessorFactory
    {
        public static FileProcessor getProcessor(string extension)
        {
            FileProcessor processor = null;
            if (extension.Equals(".pdf")) { processor = new PDFProcessor(); }
            else if (extension.Equals(".docx")) { processor = new DocxProcessor(); }
            else { processor = new GeneralProcessor(); }
            return processor;
        }
    }
}
