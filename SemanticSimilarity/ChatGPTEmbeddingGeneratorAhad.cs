using Microsoft.Extensions.Configuration;
using OpenAI.Embeddings;
using SemanticSimilarity.Utilites;

namespace MySemanticAnalysisSample.Embedding
{
    /// <summary>
    /// Class for generating embeddings using OpenAI's embedding model.
    /// </summary>
    internal class ChatGPTEmbeddingGenerator : IEmbeddingGenerator
    {
        private readonly EmbeddingClient _client;

        /// <summary>
        /// Constructor that initializes the EmbeddingClient with the OpenAI API key and embedding model name.
        /// </summary>
        /// <param name="config">Configuration object containing API key and model name</param>
        /// <exception cref="ArgumentException">Throws exception if valid API key or embedding name is not provided via config</exception>
        public ChatGPTEmbeddingGenerator(IConfiguration config)
        {
            string apiKey = config["OpenAI:apiKey"] ?? throw new ArgumentException("Missing OpenAI API Key in configuration.");
            string embeddingModel = config["OpenAI:EmbeddingModel"] ?? throw new ArgumentException("Missing OpenAI embedding model in configuration.");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentException("OpenAI API Key cannot be an empty string. Please provide valid API Key in configuration file");
            }
            if (string.IsNullOrEmpty(embeddingModel))
            {
                throw new ArgumentException("Please provide embedding model in configuration");
            }
            if (!ValidateOpenAIKey(apiKey, embeddingModel))
            {
                throw new ArgumentException("Invalid OpenAI API Key");
            }

            _client = new EmbeddingClient(embeddingModel, apiKey);

        }