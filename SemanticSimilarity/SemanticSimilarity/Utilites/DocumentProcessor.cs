using System;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace SemanticSimilarity.Utilites
{
    public class DocumentProcessor
    {
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.openai.com/v1/embeddings";

        public DocumentProcessor(string apiKey)
        {
            _apiKey = apiKey;
        }

        // Load text from a document file (TXT)
        public string LoadDocument(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        // Preprocess the document text
        public string PreprocessText(string text)
        {
            text = text.ToLower();
            text = new string(text.Where(c => !char.IsPunctuation(c)).ToArray());
            text = text.Replace("\n", " ").Replace("\r", " ");
            return text;
        }

        // Get embeddings from OpenAI API
        public async Task<List<double>> GetEmbeddingAsync(string text, string model = "text-embedding-ada-002")
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", "application/json");

            var body = new
            {
                input = text,
                model = model
            };

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
        public double CalculateCosineSimilarity(List<double> vectorA, List<double> vectorB)
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

        //Function to real all .txt files from a directory  
        //author: Naeem
        public static async Task<List<string>> ReadAllFilesInFolderAsync(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.txt");
            var fileContents = new List<string>();

            foreach (var file in files)
            {
                string content = await File.ReadAllTextAsync(file);
                fileContents.Add(content);
            }

            return fileContents;
        }

        // Function to read reference keywords file and split by new line
        //Author: Naeem23
        public static async Task<List<string>> ReadRefKeywordsAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var content = await File.ReadAllTextAsync(filePath);
            return content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
