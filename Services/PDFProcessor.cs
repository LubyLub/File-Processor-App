using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using File_Processor.Models;
using UglyToad.PdfPig;
using pdfUtilities = UglyToad.PdfPig.Content;

namespace File_Processor.Services
{
    internal class PDFProcessor : FileProcessor
    {
        private protected override string readWholeFile(string path)
        {
            string content = "";
            using (PdfDocument document = PdfDocument.Open(path))
            {
                foreach (pdfUtilities.Page page in document.GetPages())
                {
                    IEnumerable<pdfUtilities.Word> words = page.GetWords();
                    content += words.First();
                    foreach (pdfUtilities.Word word in words.Skip(1))
                    {
                        content += " " + word;
                    }
                }
            }
            return content;
        }
    }
}
