using DotNetEnv;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using System;
using OpenAI.Embeddings;
using SemanticSimilarity.Utilites;
using System.Formats.Asn1;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SemanticSimilarity
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //load environment file 
            Env.Load();

            //get OpenAI API key if null or empty throw error
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"); //use this structure for all
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key cannot be null or empty. Please set the OPENAI_API_KEY environment variable.");
            }

            await Ahad(apiKey);
        }


        //ahad
        static async Task Ahad(string apiKey)
        {
            try // Semantic Similarity Score
            {
                Console.WriteLine("             ***********************************************");
                Console.WriteLine("             ******* Single Inputs Similarity Score******");
                Console.WriteLine("             ***********************************************");
                Console.WriteLine("Enter the first text:");
                string text1 = Console.ReadLine();

                Console.WriteLine("\nEnter the second text:");
                string text2 = Console.ReadLine();

                Console.WriteLine("Calculating embeddings...");

                //set OpenAI api model "text-embedding-3-small/text-embedding-3-large/text-embedding-ada-002"
                var model = "text-embedding-3-small";

                // Initialize the EmbeddingGenerator class with the provided API key and model.
                var generator = new EmbeddingGenerator(apiKey, model);

                string embeddingResponse1 = await generator.GetEmbedding(text1);
                string embeddingResponse2 = await generator.GetEmbedding(text2);

                List<double> embedding1 = EmbeddingGenerator.ParseEmbedding(embeddingResponse1);
                List<double> embedding2 = EmbeddingGenerator.ParseEmbedding(embeddingResponse2);

                double similarity = SimilarityHelper.CalculateCosineSimilarity3(embedding1, embedding2);
                Console.WriteLine($"\nSemantic Similarity Score: {similarity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.Message);
            }

            //Input Helper start
            //static async Task Main(string[] args)
            {
                //string apiKey = "your-api-key-here"; // Replace with your OpenAI API Key
                Console.WriteLine("\n");
                Console.WriteLine("             ***********************************************");
                Console.WriteLine("             ******* Multiple Inputs Similarity Score******");
                Console.WriteLine("             ***********************************************");
                InputHelper inputHelper = new InputHelper(apiKey);

                // Get inputs from both users
                (List<string> user1Inputs, List<string> user2Inputs) = InputHelper.GetUserInputs();

                // Specify CSV file path
                string csvFilePath = "Multiple Inputs Similarity Score.csv";
                //SemanticSimilarity\bin\Debug\net9.0

                // Compare inputs and save results to CSV
                await inputHelper.CompareUserInputsAndSaveToCSV(user1Inputs, user2Inputs, csvFilePath);
            }
            //Input Helper End

            //Method to Compare Documents me
            {
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

            // other gula akhane add hobe er por thake }selo 3ta reeor komanor jonno barano hoise
            {
                //string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");  // Replace with your OpenAI API key
               // var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
                DocumentProcessor processor = new DocumentProcessor(apiKey);

                // Load text from files
                List<string> names = FileProcessor.LoadFile("H:\\Frankfurt University\\1st Semester\\Software engineering\\SoftwareEngineeringProject\\semantic-similarity-analysis\\SemanticSimilarity\\SemanticSimilarity\\names.txt");
                List<string> phrases = FileProcessor.LoadFile("H:\\Frankfurt University\\1st Semester\\Software engineering\\SoftwareEngineeringProject\\semantic-similarity-analysis\\SemanticSimilarity\\SemanticSimilarity\\phrases.txt");
                List<string> documents = FileProcessor.LoadFile("H:\\Frankfurt University\\1st Semester\\Software engineering\\SoftwareEngineeringProject\\semantic-similarity-analysis\\SemanticSimilarity\\SemanticSimilarity\\documents.txt");

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




                }//no cut this line
            }
        }
    }
}


