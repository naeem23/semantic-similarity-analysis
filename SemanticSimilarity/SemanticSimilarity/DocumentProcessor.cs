using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;
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
