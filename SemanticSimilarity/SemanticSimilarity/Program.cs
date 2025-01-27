using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticSimilarity
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try // Semantic Similarity Score
            {
                string apiKey = "Null"; //Put OpenAI Key here
                var openAIClient = new OpenAIClient(apiKey);

                Console.WriteLine("Enter the first text:");
                string text1 = Console.ReadLine();

                Console.WriteLine("\nEnter the second text:");
                string text2 = Console.ReadLine();

                Console.WriteLine("Calculating embeddings...");
                string embeddingResponse1 = await openAIClient.GetEmbedding(text1);
                string embeddingResponse2 = await openAIClient.GetEmbedding(text2);

                List<double> embedding1 = ParseEmbedding(embeddingResponse1);
                List<double> embedding2 = ParseEmbedding(embeddingResponse2);

                double similarity = openAIClient.CalculateCosineSimilarity(embedding1, embedding2);
                Console.WriteLine($"\nSemantic Similarity Score: {similarity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.Message);
            }


            //Method to Compare Documents me
            {
                string apiKey = "ewrewreret";
                var documentProcessor = new DocumentProcessor(apiKey);
                Console.WriteLine("\n");
                // Load documents
                Console.WriteLine("             ***********************************************");
                Console.WriteLine("             *******Compare Documents Similarity Score******");
                Console.WriteLine("             ***********************************************");

                Console.WriteLine("\nEnter the path of the first document:");
                string doc1Path = Console.ReadLine();
                string doc1Content = documentProcessor.LoadDocument(doc1Path);

                Console.WriteLine("\nEnter the path of the second document:");
                string doc2Path = Console.ReadLine();
                string doc2Content = documentProcessor.LoadDocument(doc2Path);

                // Preprocess the documents
                string processedDoc1 = documentProcessor.PreprocessText(doc1Content);
                string processedDoc2 = documentProcessor.PreprocessText(doc2Content);

                Console.WriteLine("Processing documents...");

                // Generate embeddings
                List<double> embedding1 = await documentProcessor.GetEmbeddingAsync(processedDoc1);
                List<double> embedding2 = await documentProcessor.GetEmbeddingAsync(processedDoc2);

                // Calculate similarity
                double similarityScore = documentProcessor.CalculateCosineSimilarity(embedding1, embedding2);

                // Display results
                Console.WriteLine($"\nSemantic Similarity Score: {similarityScore:F4}");

                // Interpret results
                if (similarityScore > 0.8)
                    Console.WriteLine("Documents are highly related.");
                else if (similarityScore > 0.5)
                    Console.WriteLine("Documents are somewhat related.");
                else
                    Console.WriteLine("Documents are not related.");
            }

        }






        static List<double> ParseEmbedding(string jsonResponse)
        {
            var json = JObject.Parse(jsonResponse);
            var embeddings = json["data"][0]["embedding"].ToObject<List<double>>();
            return embeddings;
        };
        }
    }
}
