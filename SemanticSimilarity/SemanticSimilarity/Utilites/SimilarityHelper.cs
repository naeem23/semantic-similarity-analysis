using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticSimilarity.Utilites
{
    public static class SimilarityHelper
    {
        public static double CalcCosineSimilarityMethod1(ReadOnlyMemory<float> vector1, ReadOnlyMemory<float> vector2)
        {
            // Implementation for cosine similarity calculation

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

        //ahad
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
    }
}
