using OpenAI.Embeddings;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace SemanticSimilarity.Utilites
{
    public class EmbeddingGenerator
    {
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.openai.com/v1";
        private EmbeddingClient client;
        public EmbeddingGenerator(string apiKey, string model = "text-embedding-3-large") { 
            _apiKey = apiKey;
            client = new EmbeddingClient(model, apiKey);
        }
        public async Task<ReadOnlyMemory<float>> GenerateEmbeddingsAsync(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be null or empty");
            }

            //generate embeddings
            OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(content);

            return embedding.ToFloats();
        }

        //ahad
        public async Task<string> GetEmbedding(string text, string model = "text-embedding-ada-002")
        {
            var client = new RestClient($"{BaseUrl}/embeddings");
            //var request = new RestRequest(Method.Post); // Updated Syntax
            var request = new RestRequest();
            request.Method = Method.Post;
            // request.AddHeader("Authorization", $"Bearer {_apiKey}");
            //var request = new RestRequest();  //calling post method different way Hit API
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

        public static List<double> ParseEmbedding(string jsonResponse)
        {
            var json = JObject.Parse(jsonResponse);
            var embeddings = json["data"][0]["embedding"].ToObject<List<double>>();
            return embeddings;
        }
    }
}
