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
    public static class OutputHelper
    {
        public static async Task GenerateOutputAsync(List<string> sourceContents, List<string> refContents)
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

                Console.WriteLine($"Processing {totalPairs} content pairs...");

                foreach (var source in sourceContents)
                {
                    foreach (var refr in refContents)
                    {
                        // Provide feedback for each pair being processed
                        Console.WriteLine($"Processing pair {processedPairs + 1} of {totalPairs}:");
                        Console.WriteLine($"Source: {(source.Length > 20 ? source.Substring(0,20) + "..." : source )}");
                        Console.WriteLine($"Reference: {(refr.Length > 20 ? refr.Substring(0,20) + "..." : refr)}");

                        var result = new SimilarityResult
                        {
                            Source = source.Length > 20 ? source.Substring(0,20) + "..." : source,
                            Reference = refr.Length > 20 ? refr.Substring(0,20) + "..." : refr,
                            Score_Ada = await SimilarityHelper.CalculateSimilarityAsync("text-embedding-ada-002", source, refr),
                            Score_Small = await SimilarityHelper.CalculateSimilarityAsync("text-embedding-3-small", source, refr),
                            Score_Large = await SimilarityHelper.CalculateSimilarityAsync("text-embedding-3-large", source, refr)
                        };
                        results.Add(result);
                        
                        processedPairs++;
                        Console.WriteLine($"Completed pair {processedPairs} of {totalPairs}.");
                        Console.WriteLine();
                    }
                }

                // Write results to CSV
                Console.WriteLine("Writing results to CSV...");
                WriteResultsToCsv(outputFilePath, results);

                Console.WriteLine("Similarity scores calculated and saved to CSV successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            /*
            var generator = new EmbeddingGenerator(apiKey, "text-embedding-3-large");

            //Generate embeddings
            var sourceEmbeddings = await Task.WhenAll(sourceContents.Select(text => generator.GenerateEmbeddingsAsync(text)));
            var refEmbeddings = await Task.WhenAll(refContents.Select(text => generator.GenerateEmbeddingsAsync(text)));

            // Prepare CSV data
            var csvData = new List<string[]>();
            string[] headers = new string[1 + (refContents.Count * 3)];
            headers[0] = "Source Content";

            for (int i = 0; i < refContents.Count; i++)
            {
                //headers[1 + i] = $"{refContents[i]}";
                headers[1 + i] = refContents[i].Length > 20 ? refContents[i].Substring(0, 20) + "..." : refContents[i];
            }
            csvData.Add(headers);

            // Compute similarity for each string in A with all strings in B
            for (int i = 0; i < sourceContents.Count; i++)
            {
                var row = new string[1 + refContents.Count];
                row[0] = sourceContents[i].Length > 20 ? sourceContents[i].Substring(0,20) + "..." : sourceContents[i];

                for (int j = 0; j < refContents.Count; j++)
                {
                    row[1 + j] = SimilarityHelper.CalcCosineSimilarityMethod2(sourceEmbeddings[i], refEmbeddings[j]).ToString("F4");
                }

                csvData.Add(row);
            }

            // Write CSV
            WriteToCsv(csvData);*/
        }

        // Write results to CSV
        // Author: Naeem
        private static void WriteResultsToCsv(string filePath, List<SimilarityResult> results)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
            csv.WriteRecords(results);
        }

        private static void WriteToCsv(List<string[]> data)
        {
            string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            string outputFilePath = Path.Combine(projectRoot, "Output", "similarity_output.csv");

            using (var writer = new StreamWriter(outputFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                foreach (var row in data)
                {
                    if (row != null) // Check if the row is not null
                    {
                        foreach (var field in row)
                        {
                            csv.WriteField(field); // Write each field individually
                        }
                        csv.NextRecord();
                    }
                }
            }
        }
    }
}
