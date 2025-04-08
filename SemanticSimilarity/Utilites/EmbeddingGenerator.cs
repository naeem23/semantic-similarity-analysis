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
    /// <summary>
    /// Generates text embeddings using OpenAI's embedding models.
    /// This class handles the creation of vector embeddings from text content,
    /// which can be used for semantic search, clustering, and other ML tasks.
    /// It automatically manages API authentication using an API key provider.
    /// </summary>
    public class EmbeddingGenerator
    {
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.openai.com/v1";


        /// <summary>
        /// Initializes a new instance of the EmbeddingGenerator class.
        /// Retrieves the OpenAI API key during initialization using ApiKeyProvider class.
        /// The constructor will fail if no valid API key is available.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the API key cannot be retrieved from the ApiKeyProvider.
        /// </exception>
        public EmbeddingGenerator()
        {
            // Initialize ApiKeyProvider class and retrieve the key
            ApiKeyProvider apiKeyProvider = new ApiKeyProvider();
            string apiKey = apiKeyProvider.GetApiKey();
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        /// <summary>
        /// Generates embeddings (vector representation) for the given text content.
        /// This method:
        /// 1. Validates the input content
        /// 2. Calls the OpenAI embedding API
        /// 3. Returns the embedding vector as a float array
        /// 
        /// The default model "text-embedding-3-large" provides high-quality embeddings
        /// with 3072 dimensions. You can specify other supported embedding models.
        /// Author: Naeem
        /// </summary>
        /// <param name="content">The text content to generate embeddings for</param>
        /// <param name="model">The embedding model to use (default: "text-embedding-3-large")</param>
        /// <returns>A float array representing the text embeddings</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the input content is null or empty.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when embedding generation fails (contains original exception as inner exception).
        /// </exception>
        public async Task<float[]> GenerateEmbeddingsAsync(string content, string model = "text-embedding-3-large")
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be null or empty");
            }

            try
            {
                // Create client and generate embeddings
                EmbeddingClient openAIClient = new EmbeddingClient(model, _apiKey);
                OpenAIEmbedding embedding = await openAIClient.GenerateEmbeddingAsync(content);

                // Convert embeddings to float array and return
                return embedding.ToFloats().ToArray();
            }
            catch (Exception ex) 
            {
                // Log the exception and rethrow a more specific exception
                Console.WriteLine($"Error generating embeddings: {ex.Message}");
                // Wrap and rethrow with more context
                throw new InvalidOperationException("Failed to generate embeddings.", ex);
            }
        }
    }
}
