using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemanticSimilarity.Utilites;

namespace SemanticSimilarity.Utilites
{
    public class SimilarityCalculator
    {
        // Calculate cosine similarity between two embeddings
        // Author: Naeem
        public float CalculateCosineSimilarity(float[] embedding1, float[] embedding2)
        {
            if (embedding1.Length != embedding2.Length)
                throw new ArgumentException("Embeddings must have the same length.");

            float dotProduct = 0, magnitude1 = 0, magnitude2 = 0;
            for (int i = 0; i < embedding1.Length; i++)
            {
                dotProduct += embedding1[i] * embedding2[i];
                magnitude1 += embedding1[i] * embedding1[i];
                magnitude2 += embedding2[i] * embedding2[i];
            }

            magnitude1 = (float)Math.Sqrt(magnitude1);
            magnitude2 = (float)Math.Sqrt(magnitude2);

            if (magnitude1 == 0 || magnitude2 == 0)
                throw new InvalidOperationException("One of the embeddings has zero magnitude.");

            return dotProduct / (magnitude1 * magnitude2);
        }

        // Calculate similarity score for a given model and word pair
        // Author: Naeem
        public async Task<(float Score, float[] Embedding1, float[] Embedding2)> CalculateSimilarityAsync(string model, string source, string refr)
        {
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(refr))
            {
                throw new ArgumentException("Source and reference texts cannot be null or empty.");
            }

            try
            {
                // generate embeddings for source and reference  
                var generator = new EmbeddingGenerator();
                var embedding1 = await generator.GenerateEmbeddingsAsync(source, model);
                var embedding2 = await generator.GenerateEmbeddingsAsync(refr, model);
                var score = CalculateCosineSimilarity(embedding1, embedding2);

                return (score, embedding1, embedding2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating similarity for model {model}: {ex.Message}");
                return (-1, Array.Empty<float>(), Array.Empty<float>()); // Return -1 to indicate an error
            }
        }
    }

    // Class to represent similarity results
    //Author: Naeem
    public class SimilarityResult
    {
        public string Source { get; set; }
        public string Reference { get; set; }
        public float ScoreAda { get; set; }
        public float ScoreSmall { get; set; }
        public float ScoreLarge { get; set; }
    }
}
