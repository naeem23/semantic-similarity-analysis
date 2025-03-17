using OpenAI.Embeddings;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.CodeDom.Compiler;

namespace SemanticSimilarity.Utilites
{
    public class EmbeddingGenerator
    {
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.openai.com/v1";

        public EmbeddingGenerator()
        {
            // Get api key
            ApiKeyProvider apiKeyProvider = new ApiKeyProvider();
            string apiKey = apiKeyProvider.GetApiKey();
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        // Generate embeddings using OpenAI package 
        // Author: Naeem
        public async Task<float[]> GenerateEmbeddingsAsync(string content, string model = "text-embedding-3-large")
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be null or empty");
            }

            try
            {
                EmbeddingClient openAIClient = new EmbeddingClient(model, _apiKey);
                OpenAIEmbedding embedding = await openAIClient.GenerateEmbeddingAsync(content);

                return embedding.ToFloats().ToArray();
            }
            catch (Exception ex) 
            {
                // Log the exception and rethrow a more specific exception
                Console.WriteLine($"Error generating embeddings: {ex.Message}");
                throw new InvalidOperationException("Failed to generate embeddings.", ex);
            }
        }
    }
}
