using OpenAI.Embeddings;
using System.Reflection;

namespace SemanticSimilarity.Utilites
{
    public class EmbeddingGenerator
    {
        private EmbeddingClient client;
        public EmbeddingGenerator(string apiKey, string model = "text-embedding-3-large") { 
            client = new EmbeddingClient(model, apiKey);
        }
        public async Task<ReadOnlyMemory<float>> GenerateEmbeddingsAsync(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be null or empty");
            }

            //generate text embeddings
            
            //generate embeddings
            OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(content);

            return embedding.ToFloats();
        }

        public async Task<ReadOnlyMemory<float>> GenerateEmbeddingsWithHttpClient (string content)
        {
            //write a function to generate embedding using api call --- Ahad
        }
    }
}
