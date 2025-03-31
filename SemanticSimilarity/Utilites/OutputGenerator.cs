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
    public class OutputGenerator
    {
        /// <summary>
        /// Generate embedding, calculate similarity and generate csv
        /// </summary>
        /// <param name="sourceContents"></param>
        /// <param name="refContents"></param>
        /// <returns></returns>
        public async Task GenerateOutputAsync(List<string> sourceContents, List<string> refContents)
        {
            try
            {
                // Define the output file path
                string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
                string outputFilePath = Path.Combine(projectRoot, "Output", "similarity_results.csv");
                string srcScalarOutputFilePath = Path.Combine(projectRoot, "Output", "source_scalar_values.csv");
                string refScalarOutputFilePath = Path.Combine(projectRoot, "Output", "reference_scalar_values.csv");

                // Process content pairs and calculate similarity scores
                var results = new List<SimilarityResult>();
                var srcEmbeddingScalarValue = new List<(string Text, float[] Embedding)>();
                var refEmbeddingScalarValue = new List<(string Text, float[] Embedding)>();
                int totalPairs = sourceContents.Count * refContents.Count;
                int processedPairs = 0;

                Console.WriteLine($"\n\nProcessing {totalPairs} content pairs...");

                foreach (var source in sourceContents)
                {
                    foreach (var refr in refContents)
                    {
                        // Provide feedback for each pair being processed
                        Console.WriteLine($"Processing pair {processedPairs + 1} of {totalPairs}:");
                        Console.WriteLine($"Source: {(source.Length > 20 ? source.Substring(0,20) + "..." : source )}");
                        Console.WriteLine($"Reference: {(refr.Length > 20 ? refr.Substring(0,20) + "..." : refr)}");

                        SimilarityCalculator similarityCalculator = new SimilarityCalculator();

                        string shortSource = source.Length > 20 ? source.Substring(0, 20) + "..." : source;
                        string shortReference = refr.Length > 20 ? refr.Substring(0, 20) + "..." : refr;
                        var (scoreAda, srcEmbeddingAda, refEmbeddingAda) = await similarityCalculator.CalculateSimilarityAsync("text-embedding-ada-002", source, refr);
                        var (scoreSmall, srcEmbeddingSmall, refEmbeddingSmall) = await similarityCalculator.CalculateSimilarityAsync("text-embedding-3-small", source, refr);
                        var (scoreLarge, srcEmbeddingLarge, refEmbeddingLarge) = await similarityCalculator.CalculateSimilarityAsync("text-embedding-3-large", source, refr);

                        Console.WriteLine($"Similarity Score for Ada: {scoreAda}");
                        Console.WriteLine($"Similarity Score for Small: {scoreSmall}");
                        Console.WriteLine($"Similarity Score for Large: {scoreLarge}");
                        var result = new SimilarityResult
                        {
                            Source = shortSource,
                            Reference = shortReference,
                            ScoreAda = scoreAda,
                            ScoreSmall = scoreSmall,
                            ScoreLarge = scoreLarge,
                        };
                        results.Add(result);
                        srcEmbeddingScalarValue.Add((shortSource, srcEmbeddingLarge));
                        refEmbeddingScalarValue.Add((shortReference, refEmbeddingLarge));
                        processedPairs++;
                        Console.WriteLine($"Completed pair {processedPairs} of {totalPairs}.");
                        Console.WriteLine();
                    }
                }

                // Write results to CSV
                Console.WriteLine("Writing results to CSV file (similarity_results.csv)");
                WriteResultsToCsv(outputFilePath, results);
                Console.WriteLine("Similarity scores calculated and saved to CSV (similarity_results.csv) successfully.");

                // write source and reference embedding value in two different csv file.
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

        // Write results to CSV
        // Author: Naeem
        private void WriteResultsToCsv(string filePath, List<SimilarityResult> results)
        {
            // save in the project Output folder
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(results);

            // create a csv file in the root directory
            using var writer1 = new StreamWriter("similarity_results.csv");
            using var csv1 = new CsvWriter(writer1, System.Globalization.CultureInfo.InvariantCulture);
            csv1.WriteRecords(results);
        }

        /// <summary>
        /// Function to write source/reference scalar value in a csv file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="embeddingPairs">A list of </param>
        /// <param name="type">It can be either Source/Reference</param>
        private void WriteEmbeddingScalarValueToCsv(string filePath, List<(string Text, float[] Embedding)> embeddingPairs, string type)
        {
            if (embeddingPairs.Count == 0)
            {
                Console.WriteLine("No data to write.");
                return;
            }

            var defaultFilePath = type == "Source" ? "source_scalar_values.csv" : "reference_scalar_values.csv";

            void WriteCsv(string path)
            {
                using var writer = new StreamWriter(path);
                using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);

                // Write header
                csv.WriteField(type);
                for (int i = 0; i < embeddingPairs.First().Embedding.Length; i++)
                {
                    csv.WriteField($"Dim_{i + 1}"); // Column names for embedding dimensions
                }
                csv.NextRecord();

                // Write records
                foreach (var (title, embedding) in embeddingPairs)
                {
                    csv.WriteField(title);
                    foreach (var value in embedding)
                    {
                        csv.WriteField(value);
                    }
                    csv.NextRecord();
                }
            }

            // Write to both filePath and defaultFilePath
            WriteCsv(filePath);
            WriteCsv(defaultFilePath);
        }
    }
}
