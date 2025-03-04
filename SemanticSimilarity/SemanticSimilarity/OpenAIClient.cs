using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class OpenAIClient
{
    private readonly string _apiKey;
    private const string BaseUrl = "https://api.openai.com/v1";

    public OpenAIClient(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<string> GetEmbedding(string text, string model = "text-embedding-ada-002")
    {
        var client = new RestClient($"{BaseUrl}/embeddings");
        //var request = new RestRequest(Method.Post); // Updated Syntax
        var request = new RestRequest();
        request.Method = Method.Post;

        // request.AddHeader("Authorization", $"Bearer {_apiKey}"); // REMOVE
        //var request = new RestRequest();  //calling post method different way Hit API

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
        if (response.IsSuccessful)
        {
            return response.Content;
        }

        throw new Exception($"Error: {response.StatusDescription}\n{response.Content}");
    }

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
