using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemanticSimilarity.Utilites;

namespace SemanticSimilarity.Utilites
{
    /// <summary>
    /// Calculates similarity scores between text pairs using various embedding models.
    /// Provides methods to generate embeddings and compute cosine similarity between them.
    /// </summary>
    public class SimilarityCalculator
    {
        /// <summary>
        /// Calculates cosine similarity between two embedding vectors.
        /// Cosine similarity measures the angle between two vectors in multi-dimensional space.
        /// Higher values indicate more similar content.
        /// Author: Naeem
        /// </summary>
        /// <param name="embedding1">First embedding vector (array of floats)</param>
        /// <param name="embedding2">Second embedding vector (array of floats)</param>
        /// <returns>Cosine similarity score between -1 and 1 (1 = identical, -1 = opposite)</returns>
        /// <exception cref="ArgumentException">Thrown when embeddings have different lengths</exception>
        /// <exception cref="InvalidOperationException">Thrown when an embedding has zero magnitude</exception>

        public float CalculateCosineSimilarity(float[] embedding1, float[] embedding2)
        {
            // Validate input vectors have same dimensions
            if (embedding1.Length != embedding2.Length)
                throw new ArgumentException("Embeddings must have the same length.");

            // Initialize variables for dot product and magnitudes
            float dotProduct = 0, magnitude1 = 0, magnitude2 = 0;

            // Calculate dot product and magnitudes in a single loop for efficiency
            for (int i = 0; i < embedding1.Length; i++)
            {
                dotProduct += embedding1[i] * embedding2[i];
                magnitude1 += embedding1[i] * embedding1[i];
                magnitude2 += embedding2[i] * embedding2[i];
            }

            // Convert squared magnitudes to actual magnitudes
            magnitude1 = (float)Math.Sqrt(magnitude1);
            magnitude2 = (float)Math.Sqrt(magnitude2);

            // Prevent division by zero
            if (magnitude1 == 0 || magnitude2 == 0)
                throw new InvalidOperationException("One of the embeddings has zero magnitude.");

            // Return cosine similarity (dot product divided by product of magnitudes)
            return dotProduct / (magnitude1 * magnitude2);
        }

        /// <summary>
        /// Calculates similarity between two texts using a specified embedding model.
        /// This method handles the complete similarity calculation pipeline:
        /// 1. Generates embeddings for both texts
        /// 2. Calculates cosine similarity between embeddings
        /// 3. Returns results including raw embeddings
        /// 
        /// Author: Naeem
        /// </summary>
        /// <param name="model">Name of embedding model to use (e.g., "text-embedding-ada-002")</param>
        /// <param name="source">Source text to compare</param>
        /// <param name="refr">Reference text to compare against</param>
        /// <returns>
        /// Tuple containing:
        /// - Similarity score (float)
        /// - Source text embedding (float[])
        /// - Reference text embedding (float[])
        /// Returns (-1, empty arrays) if error occurs
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when input texts are null or empty</exception>
        public async Task<(float Score, float[] Embedding1, float[] Embedding2)> CalculateSimilarityAsync(string model, string source, string refr)
        {
            // Validate input texts
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(refr))
            {
                throw new ArgumentException("Source and reference texts cannot be null or empty.");
            }

            try
            {
                // Generate embeddings for both texts using specified model
                var generator = new EmbeddingGenerator();
                var embedding1 = await generator.GenerateEmbeddingsAsync(source, model);
                var embedding2 = await generator.GenerateEmbeddingsAsync(refr, model);

                // Calculate similarity score between embeddings
                var score = CalculateCosineSimilarity(embedding1, embedding2);

                return (score, embedding1, embedding2);
            }
            catch (Exception ex)
            {
                // Log error and return error indicator
                Console.WriteLine($"Error calculating similarity for model {model}: {ex.Message}");
                return (-1, Array.Empty<float>(), Array.Empty<float>()); // Return -1 to indicate an error
            }
        }
    }

    /// <summary>
    /// Represents the similarity comparison results between a source and reference text.
    /// Stores similarity scores from multiple embedding models.
    /// Author: Naeem
    /// </summary>
    public class SimilarityResult
    {
        /// <summary>
        /// Shortened source text (first 20 chars + ... if long)
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Shortened reference text (first 20 chars + ... if long)
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Similarity score from text-embedding-ada-002 model
        /// </summary>
        public float ScoreAda { get; set; }

        /// <summary>
        /// Similarity score from text-embedding-3-small model
        /// </summary>
        public float ScoreSmall { get; set; }

        /// <summary>
        /// Similarity score from text-embedding-3-large model
        /// </summary>
        public float ScoreLarge { get; set; }
    }
}
