using System;
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

namespace SemanticSimilarity.Utilites
{
    public static class OutputHelper
    {
        public static async Task GenerateOutputAsync(List<string> sourceContents, List<string> refContents, string apiKey)
        {
            var generator = new EmbeddingGenerator(apiKey, "text-embedding-3-large");

            // Generate embeddings
            var sourceEmbeddings = await Task.WhenAll(sourceContents.Select(text => generator.GenerateEmbeddingsAsync(text)));
            var refEmbeddings = await Task.WhenAll(refContents.Select(text => generator.GenerateEmbeddingsAsync(text)));

            // Prepare CSV data
            var csvData = new List<string[]>();
            string[] headers = new string[1 + (refContents.Count * 3)];
            headers[0] = "Source Content";

            for (int i = 0; i < refContents.Count; i++)
            {
                headers[1 + i] = $"{refContents[i]}";
            }
            csvData.Add(headers);

            // Compute similarity for each string in A with all strings in B
            for (int i = 0; i < sourceContents.Count; i++)
            {
                var row = new string[1 + refContents.Count];
                row[0] = sourceContents[i].Length > 50 ? sourceContents[i].Substring(0,50) + "..." : sourceContents[i];

                for (int j = 0; j < refContents.Count; j++)
                {
                    row[1 + j] = SimilarityHelper.CalcCosineSimilarityMethod2(sourceEmbeddings[i], refEmbeddings[j]).ToString("F4");
                }

                csvData.Add(row);
            }

            // Write CSV
            WriteToCsv(csvData);
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
