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
    public class MultipleFileSimilarityProcessor
    {
        // Read text from different file formats
        // TO-DO: write test function
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
        // TO-DO: write test function
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
        // TO-DO: write test function
        private string ExtractTextFromDocx(string filePath)
        {
            using (DocX document = DocX.Load(filePath))
            {
                return document.Text;
            }
        }

        // Compare all files from two folders and save results in CSV
        public async Task ProcessFilesAndSaveToCSV(string folderPath1, string folderPath2, string csvFilePath)
        {
            string[] files1 = Directory.GetFiles(folderPath1);
            string[] files2 = Directory.GetFiles(folderPath2);

            if (files1.Length == 0 || files2.Length == 0)
            {
                Console.WriteLine("One or both folders contain no valid files.");
                return;
            }

            List<string> csvLines = new List<string> { "File 1,File 2,Similarity Score" };

            foreach (var file1 in files1)
            {
                foreach (var file2 in files2)
                {
                    try
                    {
                        string text1 = ReadFileText(file1);
                        string text2 = ReadFileText(file2);

                        //List<double> vectorA = await GetEmbeddingAsync(text1);
                        //List<double> vectorB = await GetEmbeddingAsync(text2);

                        //double similarity = CalculateCosineSimilarity(vectorA, vectorB);
                        //Console.WriteLine($"Similarity between {Path.GetFileName(file1)} and {Path.GetFileName(file2)} → {similarity:F4}");

                        //csvLines.Add($"\"{Path.GetFileName(file1)}\",\"{Path.GetFileName(file2)}\",{similarity:F4}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing {file1} and {file2}: {ex.Message}");
                    }
                }
            }

            File.WriteAllLines(csvFilePath, csvLines);
            Console.WriteLine("");
            Console.WriteLine($" Congratulation!!! Results successfully saved to: {csvFilePath}");
        }
    }
}//end line
