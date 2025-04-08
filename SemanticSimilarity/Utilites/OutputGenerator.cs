using System;
using System.IO;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using SemanticSimilarity.Utilites;
using Sprache;
using DotNetEnv;
using iText.Forms.Xfdf;

namespace SemanticSimilarity.Utilites
{
    /// <summary>
    /// Handles the generation of output files containing similarity scores and embedding values
    /// between source and reference text content. Creates CSV files with the results.
    /// </summary>
    public class OutputGenerator
    {
        /// <summary>
        /// Generates embeddings, calculates similarity scores between all source-reference pairs,
        /// and saves results to CSV files.
        /// Outputs three CSV files:
        /// 1. similarity_results.csv - Contains similarity scores for each model (Ada, Small, Large)
        /// 2. source_scalar_values.csv - Contains embedding vectors for source texts
        /// 3. reference_scalar_values.csv - Contains embedding vectors for reference texts
        /// </summary>
        /// <param name="sourceContents">List of source text strings to compare</param>
        /// <param name="refContents">List of reference text strings to compare against</param>
        /// <returns>Task representing the asynchronous operation</returns>
        /// <exception cref="Exception">Catches and logs any exceptions during processing</exception>
        public async Task GenerateOutputAsync(List<string> sourceContents, List<string> refContents)
        {
            try
            {
                // Set up file paths in the project's Output folder
                string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
                string outputFilePath = Path.Combine(projectRoot, "Output", "similarity_results.csv");
                string srcScalarOutputFilePath = Path.Combine(projectRoot, "Output", "source_scalar_values.csv");
                string refScalarOutputFilePath = Path.Combine(projectRoot, "Output", "reference_scalar_values.csv");

                // Initialize collections to store results
                var results = new List<SimilarityResult>();
                var srcEmbeddingScalarValue = new List<(string Text, float[] Embedding)>();
                var refEmbeddingScalarValue = new List<(string Text, float[] Embedding)>();

                // Calculate total pairs for progress tracking
                int totalPairs = sourceContents.Count * refContents.Count;
                int processedPairs = 0;

                Console.WriteLine($"\n\nProcessing {totalPairs} content pairs...");

                // Process each source-reference pair
                foreach (var source in sourceContents)
                {
                    foreach (var refr in refContents)
                    {
                        // Show progress and current pair being processed
                        Console.WriteLine($"Processing pair {processedPairs + 1} of {totalPairs}:");
                        Console.WriteLine($"Source: {(source.Length > 20 ? source.Substring(0,20) + "..." : source )}");
                        Console.WriteLine($"Reference: {(refr.Length > 20 ? refr.Substring(0,20) + "..." : refr)}");

                        // Calculate similarity scores using different models
                        SimilarityCalculator similarityCalculator = new SimilarityCalculator();

                        // Shorten text for display purposes
                        string shortSource = source.Length > 20 ? source.Substring(0, 20) + "..." : source;
                        string shortReference = refr.Length > 20 ? refr.Substring(0, 20) + "..." : refr;

                        // Get similarity scores and embeddings from all models
                        var (scoreAda, srcEmbeddingAda, refEmbeddingAda) = await similarityCalculator.CalculateSimilarityAsync("text-embedding-ada-002", source, refr);
                        var (scoreSmall, srcEmbeddingSmall, refEmbeddingSmall) = await similarityCalculator.CalculateSimilarityAsync("text-embedding-3-small", source, refr);
                        var (scoreLarge, srcEmbeddingLarge, refEmbeddingLarge) = await similarityCalculator.CalculateSimilarityAsync("text-embedding-3-large", source, refr);

                        // Display calculated scores
                        Console.WriteLine($"Similarity Score for Ada: {scoreAda}");
                        Console.WriteLine($"Similarity Score for Small: {scoreSmall}");
                        Console.WriteLine($"Similarity Score for Large: {scoreLarge}");

                        // Store results
                        var result = new SimilarityResult
                        {
                            Source = shortSource,
                            Reference = shortReference,
                            ScoreAda = scoreAda,
                            ScoreSmall = scoreSmall,
                            ScoreLarge = scoreLarge,
                        };
                        results.Add(result);

                        // Store embeddings from the large model
                        srcEmbeddingScalarValue.Add((shortSource, srcEmbeddingLarge));
                        refEmbeddingScalarValue.Add((shortReference, refEmbeddingLarge));

                        processedPairs++;
                        Console.WriteLine($"Completed pair {processedPairs} of {totalPairs}.");
                        Console.WriteLine();
                    }
                }

                // Save all results to CSV files
                Console.WriteLine("Writing results to CSV file (similarity_results.csv)");
                WriteResultsToCsv(outputFilePath, results);
                Console.WriteLine("Similarity scores calculated and saved to CSV (similarity_results.csv) successfully.");

                // Save embedding values to separate CSV files
                Console.WriteLine("Writing source scalar value in CSV file (source_scalar_values.csv)");
                WriteEmbeddingScalarValueToCsv(srcScalarOutputFilePath, srcEmbeddingScalarValue, "Source");
                Console.WriteLine("Writing reference scalar value in CSV file (reference_scalar_values.csv)");
                WriteEmbeddingScalarValueToCsv(refScalarOutputFilePath, refEmbeddingScalarValue, "Reference");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Writes similarity results to a CSV file in two locations: 
        /// the specified file path and the application's root directory.
        /// </summary>
        /// <param name="filePath">Primary output file path (in Output folder)</param>
        /// <param name="results">List of SimilarityResult objects containing scores</param>
        private void WriteResultsToCsv(string filePath, List<SimilarityResult> results)
        {
            // save in the project Output folder
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(results);

            // Create a backup copy in the root directory
            using var writer1 = new StreamWriter("similarity_results.csv");
            using var csv1 = new CsvWriter(writer1, System.Globalization.CultureInfo.InvariantCulture);
            csv1.WriteRecords(results);
        }

        /// <summary>
        /// Writes embedding vector values to CSV files for analysis.
        /// Creates files in both the specified path and root directory.
        /// </summary>
        /// <param name="filePath">Primary output file path</param>
        /// <param name="embeddingPairs">List of text-embedding pairs to save</param>
        /// <param name="type">Type of embeddings ("Source" or "Reference")</param>
        private void WriteEmbeddingScalarValueToCsv(string filePath, List<(string Text, float[] Embedding)> embeddingPairs, string type)
        {
            if (embeddingPairs.Count == 0)
            {
                Console.WriteLine("No data to write.");
                return;
            }

            // Determine default filename based on type
            var defaultFilePath = type == "Source" ? "source_scalar_values.csv" : "reference_scalar_values.csv";

            // Local function to handle the actual CSV writing
            void WriteCsv(string path)
            {
                using var writer = new StreamWriter(path);
                using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);

                // Write header row with column names
                csv.WriteField(type);
                for (int i = 0; i < embeddingPairs.First().Embedding.Length; i++)
                {
                    csv.WriteField($"Dim_{i + 1}"); // Column names for embedding dimensions
                }
                csv.NextRecord();

                // Write each text with its embedding vector
                foreach (var (title, embedding) in embeddingPairs)
                {
                    csv.WriteField(title);  // Text content
                    foreach (var value in embedding)
                    {
                        csv.WriteField(value);  // Each value in the embedding vector
                    }
                    csv.NextRecord();
                }
            }

            // Write to both the specified path and default location
            WriteCsv(filePath);
            WriteCsv(defaultFilePath);
        }
    }
}
