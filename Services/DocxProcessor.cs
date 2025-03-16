using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DocumentFormat.OpenXml.Packaging;
using wordUtil = DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

namespace File_Processor.Services
{
    internal class DocxProcessor : FileProcessor
    {
        private protected override string readWholeFile(string path)
        {
            string content = "";
            using (WordprocessingDocument document = WordprocessingDocument.Open(path, false))
            {
                if (document != null)
                {
                    MainDocumentPart main = document.MainDocumentPart;
                    if (main != null)
                    {
                        //Note this does not reader text in order because all header texts will come first, then all body text and then all pages' footer text but this is irrelevant due to our requirements

                        //Read the headers
                        IEnumerable<HeaderPart> headerParts = main.HeaderParts;
                        string headerText = "";
                        headerText += headerParts.First().Header.InnerText;
                        foreach (HeaderPart headerPart in headerParts.Skip(1))
                        {
                            headerText += " " + headerPart.Header.InnerText;
                        }
                        content += headerText + "\n";

                        //Read the body
                        wordUtil.Body body = main.Document.Body;
                        if (body != null)
                        {
                            string bodyText = "";

                            foreach (var elementType in body.Descendants())
                            {
                                if (elementType is wordUtil.Table)
                                {
                                    bodyText += "\n";
                                    wordUtil.Table table = (wordUtil.Table)elementType;
                                    foreach (wordUtil.TableRow row in table.Elements<wordUtil.TableRow>())
                                    {
                                        foreach (wordUtil.TableCell cell in row.Elements<wordUtil.TableCell>())
                                        {
                                            bodyText += cell.InnerText + "\t";
                                        }
                                    }
                                }
                                else if (elementType is wordUtil.Paragraph)
                                {
                                    wordUtil.Paragraph paragraph = (wordUtil.Paragraph)elementType;
                                    if (!paragraphParentIsTable(paragraph))
                                    {
                                        bodyText += "\n" + paragraph.InnerText;
                                    }
                                }
                            }

                            content += bodyText + "\n";
                        }

                        //Read the footers
                        IEnumerable<FooterPart> footerParts = main.FooterParts;
                        string footerText = "";
                        footerText += footerParts.First().Footer.InnerText;
                        foreach (FooterPart footerPart in footerParts.Skip(1))
                        {
                            footerText += " " + footerPart.Footer.InnerText;
                        }
                        content += footerText;
                    }
                }
            }
            return content;
        }

        private bool paragraphParentIsTable(wordUtil.Paragraph p)
        {
            OpenXmlElement parent = p.Parent;

            while (parent != null)
            {
                if (parent is wordUtil.TableCell)
                {
                    return true;
                }
                parent = parent.Parent;
            }

            return false;
        }
    }
}
