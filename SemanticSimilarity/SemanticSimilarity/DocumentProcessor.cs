using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using Xceed.Words.NET;
using UglyToad.PdfPig;
using CsvHelper;

public class DocumentProcessor
{
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.openai.com/v1/embeddings";

    public DocumentProcessor(string apiKey)
    {
        _apiKey = apiKey;
    }

    // Load text from a document file (TXT, DOC, DOCX, PDF)
    public string LoadDocument(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.");
        }

        // Remove quotes from the file path
        filePath = filePath.Trim('"');

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        // Check file extension
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".txt":
                return File.ReadAllText(filePath);

            case ".doc":
            case ".docx":
                using (var doc = DocX.Load(filePath))
                {
                    return doc.Text;
                }

            case ".pdf":
                var text = new StringBuilder();
                using (var pdf = PdfDocument.Open(filePath))
                {
                    foreach (var page in pdf.GetPages())
                    {
                        text.Append(page.Text);
                    }
                }
                return text.ToString();

            default:
                throw new NotSupportedException($"File format '{extension}' is not supported.");
        }
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
}