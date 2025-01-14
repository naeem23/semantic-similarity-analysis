using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class SemanticSimilarityAcrossDomains
{
    static void Main(string[] args)
    {
        // Path to your GloVe embeddings file
        string gloveFilePath = @"C:\Users\HP\Downloads\glove.840B.300d\glove.840B.300d.txt";

        // Load embeddings
        Console.WriteLine("Loading embeddings...");
        var embeddings = LoadEmbeddings(gloveFilePath);
        Console.WriteLine($"Embeddings loaded. Total words: {embeddings.Count}");

        // Word pairs from different domains
        var wordPairs = new List<(string, string)>
        {
            // Politics Domain
            ("Angela", "government"),
            // Sports Domain
            ("Cristiano", "goal"),
            // Nature Domain
            ("sunny", "rainy"),
            // Cross-Domain Comparisons
            ("Cristiano", "government"),
            ("Angela", "goal")
        };

        // Write results to a CSV file
        string csvFilePath = @"D:\Information Technology\Software Engineering - Project\SE Project\semantic-similarity-analysis\similarity_results.csv";
        ExportSimilarityResultsToCSV(wordPairs, embeddings, csvFilePath);

        Console.WriteLine("Similarity results have been written to the CSV file.");
    }

    /// <summary>
    /// Loads GloVe embeddings into a dictionary.
    /// </summary>
    /// <param name="filePath">Path to the GloVe file.</param>
    /// <returns>Dictionary with words as keys and their vector representations as values.</returns>
    static Dictionary<string, float[]> LoadEmbeddings(string filePath)
    {
        var embeddings = new Dictionary<string, float[]>();

        try
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split(' ');
                if (parts.Length > 1)
                {
                    string word = parts[0];
                    float[] vector = parts.Skip(1).Select(float.Parse).ToArray();
                    embeddings[word] = vector;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading GloVe embeddings: {ex.Message}");
        }

        return embeddings;
    }

    /// <summary>
    /// Calculates cosine similarity between two vectors.
    /// </summary>
    /// <param name="vector1">First vector.</param>
    /// <param name="vector2">Second vector.</param>
    /// <returns>Cosine similarity score.</returns>
    static double CosineSimilarity(float[] vector1, float[] vector2)
    {
        double dotProduct = vector1.Zip(vector2, (x, y) => x * y).Sum();
        double magnitude1 = Math.Sqrt(vector1.Sum(x => x * x));
        double magnitude2 = Math.Sqrt(vector2.Sum(y => y * y));

        return dotProduct / (magnitude1 * magnitude2);
    }

    /// <summary>
    /// Exports similarity results between word pairs to a CSV file.
    /// </summary>
    /// <param name="wordPairs">List of word pairs to compare.</param>
    /// <param name="embeddings">The dictionary containing word embeddings.</param>
    /// <param name="filePath">Path to the CSV file to save the results.</param>
    static void ExportSimilarityResultsToCSV(List<(string, string)> wordPairs, Dictionary<string, float[]> embeddings, string filePath)
    {
        try
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Word1,Word2,Similarity");

                foreach (var (word1, word2) in wordPairs)
                {
                    if (embeddings.ContainsKey(word1) && embeddings.ContainsKey(word2))
                    {
                        var vector1 = embeddings[word1];
                        var vector2 = embeddings[word2];
                        double similarity = CosineSimilarity(vector1, vector2);
                        writer.WriteLine($"{word1},{word2},{similarity:F2}");
                    }
                    else
                    {
                        Console.WriteLine($"One or both words not found: '{word1}', '{word2}'");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to CSV: {ex.Message}");
        }
    }
}
