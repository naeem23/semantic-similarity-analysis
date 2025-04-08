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
    /// <summary>
    /// Processes files of different formats and extracts their text content.
    /// Supports TXT, PDF, and DOCX file formats (DOCX currently has licensing issues).
    /// Uses iText7 for PDF processing and Xceed.Words.NET for DOCX files.
    /// Author: Ahad
    /// </summary>
    public class FileProcessor
    {
        /// <summary>
        /// Reads and extracts text content from a file.
        /// The method automatically detects the file format by its extension and
        /// uses the appropriate extraction method for each format.
        /// Note: DOCX support currently has licensing issues.
        /// </summary>
        /// <param name="filePath">Full path to the file to be processed</param>
        /// <returns>Extracted text content as a string</returns>
        /// <exception cref="FileNotFoundException">Thrown when the specified file doesn't exist</exception>
        /// <exception cref="NotSupportedException">Thrown for unsupported file formats</exception>
        /// <exception cref="Exception">May throw other I/O related exceptions</exception>
        public string ReadFileText(string filePath)
        {
            // Check if file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            // Get file extension and process accordingly
            string extension = Path.GetExtension(filePath).ToLower();

            if (extension == ".txt")
            {
                // Simple text file - read directly
                return File.ReadAllText(filePath);
            }
            else if (extension == ".pdf")
            {
                // Process PDF file
                return ExtractTextFromPdf(filePath);
            }
            else if (extension == ".docx")
            {
                // Process Word document (note: has licensing issues)
                return ExtractTextFromDocx(filePath);
            }
            else
            {
                throw new NotSupportedException($"Unsupported file format: {extension}");
            }
        }

        /// <summary>
        /// Extracts text content from a PDF file using iText7 library.
        /// Processes the PDF page by page and concatenates all text content.
        /// Properly disposes all PDF resources when done.
        /// </summary>
        /// <param name="filePath">Path to the PDF file</param>
        /// <returns>Extracted text as a single string</returns>
        /// <exception cref="PdfException">Thrown for invalid or corrupted PDF files</exception>
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

        /// <summary>
        /// Extracts text content from a DOCX file.
        /// IMPORTANT: This method currently doesn't work due to licensing requirements
        /// of the Xceed.Words.NET library. 
        /// </summary>
        /// <param name="filePath">Path to the DOCX file</param>
        /// <returns>Extracted text as a single string</returns>
        /// <exception cref="LicenseException">Currently throws due to licensing requirements</exception>
        private string ExtractTextFromDocx(string filePath)
        {
            using (DocX document = DocX.Load(filePath))
            {
                return document.Text;
            }
        }
    }
}//end line
