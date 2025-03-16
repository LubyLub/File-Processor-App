using System.IO;
using SC = ShapeCrawler;

namespace File_Processor.Services
{
    class PPTProcessor : FileProcessor
    {
        private protected override string readWholeFile(string path)
        {
            string slideContents = "";
            string slideNotes = "";

            FileStream stream = new FileStream(path, FileMode.Open);

            var pres = new SC.Presentation(stream);

            var slides = pres.Slides;

            foreach (var slide in slides)
            {
                foreach (var shape in slide.Shapes)
                {
                    slideContents += shape.Text + " ";
                }

                var slideNote = slide.Notes;
                if (slideNote != null) { slideNotes += slideNote.Text; }
            }

            if (slideContents.Length > 0) { slideContents = slideContents.Substring(0, slideContents.Length - 1); }

            stream.Close();

            return slideContents + "\n" + slideNotes;
        }
    }
}
