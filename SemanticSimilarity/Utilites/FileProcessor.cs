using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Xceed.Words.NET;
using System.Reflection.PortableExecutable;

namespace SemanticSimilarity.Utilites
{
    public class FileProcessor
    {
        // Read text from different file formats
        // Author: Ahad
        public string ReadFileText(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".txt")
            {
                return File.ReadAllText(filePath);
            }
            else if (extension == ".pdf")
            {
                return ExtractTextFromPdf(filePath);
            }
            else if (extension == ".docx")
            {
                return ExtractTextFromDocx(filePath);
            }
            else
            {
                throw new NotSupportedException($"Unsupported file format: {extension}");
            }
        }

        // Extract text from PDF using iText7
        // Author: Ahad
        private string ExtractTextFromPdf(string filePath)
        {
            using (PdfReader reader = new PdfReader(filePath))
            using (PdfDocument pdfDoc = new PdfDocument(reader))
            {
                string text = "";
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    text += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)) + "\n";
                }
                return text;
            }
        }

        // Extract text from DOCX using Xceed.Words.NET
        // Problem: Function is not working. asking for license
        // Author: Ahad
        private string ExtractTextFromDocx(string filePath)
        {
            using (DocX document = DocX.Load(filePath))
            {
                return document.Text;
            }
        }
    }
}//end line
