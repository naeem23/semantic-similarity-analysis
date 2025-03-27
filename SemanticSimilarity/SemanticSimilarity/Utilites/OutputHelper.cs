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

namespace SemanticSimilarity.Utilites
{
    public class OutputHelper
    {
        /// <summary>
        /// Generate embedding, calculate similarity and generate csv
        /// </summary>
        /// <param name="sourceContents">list string</param>
        /// <param name="refContents">list string</param>
        /// <returns></returns>
        public async Task GenerateOutputAsync(List<string> sourceContents, List<string> refContents)
        {
            try
            {
                // Define the output file path
                string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
                string outputFilePath = Path.Combine(projectRoot, "Output", "similarity_results.csv");

                // Process content pairs and calculate similarity scores
                var results = new List<SimilarityResult>();
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

                        SimilarityHelper similarityHelper = new SimilarityHelper();
                        var result = new SimilarityResult
                        {
                            Source = source.Length > 20 ? source.Substring(0,20) + "..." : source,
                            Reference = refr.Length > 20 ? refr.Substring(0,20) + "..." : refr,
                            Score_Ada = await similarityHelper.CalculateSimilarityAsync("text-embedding-ada-002", source, refr),
                            Score_Small = await similarityHelper.CalculateSimilarityAsync("text-embedding-3-small", source, refr),
                            Score_Large = await similarityHelper.CalculateSimilarityAsync("text-embedding-3-large", source, refr)
                        };
                        results.Add(result);
                        
                        processedPairs++;
                        Console.WriteLine($"Completed pair {processedPairs} of {totalPairs}.");
                        Console.WriteLine();
                    }
                }

                // Write results to CSV
                Console.WriteLine("Writing results to CSV file (Output\\similarity_results.csv)");
                WriteResultsToCsv(outputFilePath, results);

                Console.WriteLine("Similarity scores calculated and saved to CSV (Output\\similarity_results.csv) successfully.");
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
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(results);
        }
    }
}
