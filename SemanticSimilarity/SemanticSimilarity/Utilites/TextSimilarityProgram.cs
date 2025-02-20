using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SemanticSimilarity.Utilites
{
    internal class TextSimilarityProgram
    {
        public static async Task Run()
        {
            string apiKey = "your-api-key-here";  // Replace with your OpenAI API key 
            DocumentProcessor processor = new DocumentProcessor(apiKey);

            // Load text from files
            List<string> names = FileProcessor.LoadFile("names.txt");
            List<string> phrases = FileProcessor.LoadFile("phrases.txt");
            List<string> documents = FileProcessor.LoadFile("documents.txt");

            // Store results for CSV
            List<(string, string, double)> results = new List<(string, string, double)>();

            // Compare text in each category
            await CompareTextPairs(processor, names, "Names", results);
            await CompareTextPairs(processor, phrases, "Phrases", results);
            await CompareTextPairs(processor, documents, "Documents", results);

            // Save results to CSV
            CsvWriter.SaveResultsToCsv("similarity_results.csv", results);
        }

        static async Task CompareTextPairs(DocumentProcessor processor, List<string> texts, string category, List<(string, string, double)> results)
        {
            Console.WriteLine($"\n==== {category} Semantic Similarity ====");

            for (int i = 0; i < texts.Count; i++)
            {
                for (int j = i + 1; j < texts.Count; j++)
                {
                    string text1 = texts[i];
                    string text2 = texts[j];

                    List<double> vectorA = await processor.GetEmbeddingAsync(text1);
                    List<double> vectorB = await processor.GetEmbeddingAsync(text2);

                    double similarity = processor.CalculateCosineSimilarity(vectorA, vectorB);
                    Console.WriteLine($"Comparing: \"{text1}\" ↔ \"{text2}\" → Similarity Score: {similarity:F4}");

                    results.Add((text1, text2, similarity));
                }
            }
        }
    }

}

