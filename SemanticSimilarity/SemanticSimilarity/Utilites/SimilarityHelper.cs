using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemanticSimilarity.Utilites;

namespace SemanticSimilarity.Utilites
{
    public static class SimilarityHelper
    {

        // Calculate cosine similarity: method 1
        // Author: Naeem
        public static double CalcCosineSimilarityMethod1(ReadOnlyMemory<float> vector1, ReadOnlyMemory<float> vector2)
        {
            float[] vec1 = vector1.ToArray();
            float[] vec2 = vector2.ToArray();

            //calculate dot product 
            double dotProduct = vec1.Zip(vec2, (v1, v2) => v1 * v2).Sum();

            //calculate magnitude (norms)
            double magnitude1 = Math.Sqrt(vec1.Sum(v => v * v));
            double magnitude2 = Math.Sqrt(vec2.Sum(v => v * v));

            //return cosine similarity
            return dotProduct / (magnitude1 * magnitude2);
        }

        // Calculate cosine similarity: method 2
        // Author: Naeem
        public static float CalcCosineSimilarityMethod2(ReadOnlyMemory<float> vectorA, ReadOnlyMemory<float> vectorB)
        {
            float dotProduct = 0;
            float magnitudeA = 0;
            float magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA.Span[i] * vectorB.Span[i];
                magnitudeA += vectorA.Span[i] * vectorA.Span[i];
                magnitudeB += vectorB.Span[i] * vectorB.Span[i];
            }

            // Calculate the cosine similarity
            float similarity = dotProduct / (MathF.Sqrt(magnitudeA) * MathF.Sqrt(magnitudeB));

            // Round the result to one decimal place before returning
            //return MathF.Round(similarity, 1);
            return similarity;
        }

        // Calculate Similarity using cosine formula 
        // Author: Haimonti
        public static double CalculateCosineSimilarity3(List<double> vectorA, List<double> vectorB)
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

        // Calculate cosine similarity between two embeddings
        // Author: Naeem/Ahad
        public static float CalculateCosineSimilarity(float[] embedding1, float[] embedding2)
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
        public static async Task<float> CalculateSimilarityAsync(string model, string source, string refr)
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
                return CalculateCosineSimilarity(embedding1, embedding2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating similarity for model {model}: {ex.Message}");
                return -1; // Return -1 to indicate an error
            }
        }
    }

    // Class to represent similarity results
    //Author: Naeem
    // TO-DO: write test function
    public class SimilarityResult
    {
        public string Source { get; set; }
        public string Reference { get; set; }
        public float Score_Ada { get; set; }
        public float Score_Small { get; set; }
        public float Score_Large { get; set; }
    }
}
