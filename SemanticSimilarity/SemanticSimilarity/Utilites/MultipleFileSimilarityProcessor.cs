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
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.openai.com/v1/embeddings";

        public MultipleFileSimilarityProcessor(string apiKey)
        {
            _apiKey = apiKey;
        }

        // Read text from different file formats
        private string ReadFileText(string filePath)
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
        private string ExtractTextFromDocx(string filePath)
        {
            using (DocX document = DocX.Load(filePath))
            {
                return document.Text;
            }
        }

        // Get embeddings from OpenAI API
        private async Task<List<double>> GetEmbeddingAsync(string text, string model = "text-embedding-ada-002")
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");

            var body = new { input = text, model = model };
            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.StatusDescription}\n{response.Content}");
            }

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
            var embedding = jsonResponse["data"][0]["embedding"].ToObject<List<double>>();
            return embedding;
        }

        // Calculate cosine similarity
        private double CalculateCosineSimilarity(List<double> vectorA, List<double> vectorB)
        {
            if (vectorA.Count != vectorB.Count)
                throw new ArgumentException("Vectors must be of the same length.");

            double dotProduct = 0.0, magnitudeA = 0.0, magnitudeB = 0.0;

            for (int i = 0; i < vectorA.Count; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += Math.Pow(vectorA[i], 2);
                magnitudeB += Math.Pow(vectorB[i], 2);
            }

            return dotProduct / (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
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

                        List<double> vectorA = await GetEmbeddingAsync(text1);
                        List<double> vectorB = await GetEmbeddingAsync(text2);

                        double similarity = CalculateCosineSimilarity(vectorA, vectorB);
                        Console.WriteLine($"Similarity between {Path.GetFileName(file1)} and {Path.GetFileName(file2)} → {similarity:F4}");

                        csvLines.Add($"\"{Path.GetFileName(file1)}\",\"{Path.GetFileName(file2)}\",{similarity:F4}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing {file1} and {file2}: {ex.Message}");
                    }
                }
            }

            File.WriteAllLines(csvFilePath, csvLines);
            Console.WriteLine($" Congratulation!!! Results successfully saved to: {csvFilePath}");
        }
    }
}//end line
