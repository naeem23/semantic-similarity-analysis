using System;
using System.Collections.Generic;

namespace SemanticSimilarity
{
    public class SimilarityReportGenerator
    {
        // Method to generate a summary of similarity results
        public void GenerateSummary(Dictionary<(string, string), double> similarityResults)
        {
            Console.WriteLine("Similarity Analysis Report:");
            Console.WriteLine("============================");

            foreach (var result in similarityResults)
            {
                Console.WriteLine($"Texts: \"{result.Key.Item1}\" and \"{result.Key.Item2}\"");
                Console.WriteLine($"Similarity Score: {result.Value:F4}");
                Console.WriteLine(new string('-', 50));
            }
        }

        // Method to generate a heatmap (text-based) for pairwise similarities
        public void GenerateHeatmap(string[] texts, Dictionary<(string, string), double> similarityResults)
        {
            Console.WriteLine("Pairwise Similarity Heatmap:");
            Console.WriteLine("============================");

            // Print header
            Console.Write("        ");
            foreach (var text in texts)
            {
                Console.Write($"{text.Substring(0, Math.Min(10, text.Length))} ");
            }
            Console.WriteLine();

            // Print rows
            for (int i = 0; i < texts.Length; i++)
            {
                Console.Write($"{texts[i].Substring(0, Math.Min(10, texts[i].Length))} ");
                for (int j = 0; j < texts.Length; j++)
                {
                    if (i == j)
                    {
                        Console.Write("  1.000  ");
                    }
                    else
                    {
                        var key = (texts[i], texts[j]);
                        if (similarityResults.ContainsKey(key))
                        {
                            Console.Write($"{similarityResults[key]:F4} ");
                        }
                        else
                        {
                            key = (texts[j], texts[i]);
                            Console.Write($"{similarityResults[key]:F4} ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}