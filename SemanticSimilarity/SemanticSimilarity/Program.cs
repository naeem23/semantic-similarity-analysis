using DotNetEnv;
using Newtonsoft.Json.Linq;
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
using System.Threading.Tasks;

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

            //await Ahad(apiKey);
            await Naeem(apiKey);
        }

        //ahad
        //make Options start

        //make options end

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
                //InputHelper inputHelper = new InputHelper(apiKey);

                // Get inputs from both users
                //(List<string> user1Inputs, List<string> user2Inputs) = InputHelper.GetUserInputs();

                // Specify CSV file path
                //string csvFilePath = "Multiple Inputs Similarity Score.csv";
                //SemanticSimilarity\bin\Debug\net9.0 me

                // Compare inputs and save results to CSV
                //await inputHelper.CompareUserInputsAndSaveToCSV(user1Inputs, user2Inputs, csvFilePath);
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

            //MultipleFileSimilarityProcessor Start
            
            //static async Task Main(string[] args)
            {
                //Console.WriteLine("Enter your OpenAI API Key:");
                // string apiKey = Console.ReadLine();
                Console.WriteLine("\n");
                // Load documents
                Console.WriteLine("             ***********************************************");
                Console.WriteLine("             *******Multiple File Similarity Score******");
                Console.WriteLine("             ***********************************************");

                MultipleFileSimilarityProcessor processor = new MultipleFileSimilarityProcessor(apiKey);
                Console.WriteLine("\n");
                Console.Write("Enter the first folder path: ");
                string folderPath1 = Console.ReadLine();

                Console.WriteLine("\n");

                Console.Write("Enter the second folder path: ");
                string folderPath2 = Console.ReadLine();
                Console.WriteLine("\n");
                Console.WriteLine("Processing documents...");
                string csvFilePath = "MultipleFileSimilarityProcessor.csv";
                await processor.ProcessFilesAndSaveToCSV(folderPath1, folderPath2, csvFilePath);
            }
            //MultipleFileSimilarityProcessor End

            // other gula akhane add hobe er por thake }selo 3ta reeor komanor jonno barano hoise
            {
                //string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");  // Replace with your OpenAI API key
                // var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
                DocumentProcessor processor = new DocumentProcessor(apiKey);
                Console.WriteLine("\n");
                // Load documents
                Console.WriteLine("             ***********************************************");
                Console.WriteLine("             *******Compare Documents Similarity Score******");
                Console.WriteLine("             ***********************************************");

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
                CsvWriter.SaveResultsToCsv("Compare Documents Similarity Score.csv", results);
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

                  
        static async Task Naeem(string apiKey)
        {
            //set OpenAI api model "text-embedding-3-small/text-embedding-3-large/text-embedding-ada-002"
            //var model = "text-embedding-3-small";

            // Initialize the EmbeddingGenerator class with the provided API key and model.
            //var generator = new EmbeddingGenerator(apiKey, model);

            // Read all files
            string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            string sourceFilePaths = Path.Combine(projectRoot, "Input", "Sources");
            string refFilePaths = Path.Combine(projectRoot, "Input", "References");
            //string refKeywordsFilePath = Path.Combine(projectRoot, "Input", "reference_keywords.txt");

            var sourceContents = await InputHelper.ReadAllFilesInFolderAsync(sourceFilePaths);
            var refContents = await InputHelper.ReadAllFilesInFolderAsync(refFilePaths);
            //var refKeywords = await InputHelper.ReadRefKeywordsAsync(refKeywordsFilePath); // Read and split reference keywords file

            await OutputHelper.GenerateOutputAsync(sourceContents, refContents, apiKey);

            ////get document paths from user 
            //var documentPaths = InputHelper.GetFilePaths();

            ////get content of the documents 
            //var textContents = InputHelper.GetTextFileContent(documentPaths);

            ////embedding value for keyword "Climate"
            //var keywordEmbedding = await generator.GenerateEmbeddingsAsync("Climate");

            ////generate embedding for document content and calculate similarity with keyword 
            //foreach (var content in textContents)
            //{
            //    var embedding = await generator.GenerateEmbeddingsAsync(content);
            //    float similarity = SimilarityHelper.CalcCosineSimilarityMethod2(embedding, keywordEmbedding);
            //    Console.WriteLine($"Similarity is {similarity}");
            //    Console.WriteLine("=========================================");
            //}

                }//no cut this line
            }
        }

        
        static async Task Haimanti (string apiKey)
        {
            try // Semantic Similarity Score
            {
                var openAIClient = new OpenAIClient(apiKey);

                Console.WriteLine("Enter the first text:");
                string text1 = Console.ReadLine();

                Console.WriteLine("\nEnter the second text:");
                string text2 = Console.ReadLine();
                Console.WriteLine("Calculating embeddings...");

                string embeddingResponse1 = await openAIClient.GetEmbedding(text1);
                string embeddingResponse2 = await openAIClient.GetEmbedding(text2);

                var model = "text-embedding-3-small";

                // Initialize the EmbeddingGenerator class with the provided API key and model.
                var generator = new EmbeddingGenerator(apiKey, model);

                List<double> embedding1 = EmbeddingGenerator.ParseEmbedding(embeddingResponse1);
                List<double> embedding2 = EmbeddingGenerator.ParseEmbedding(embeddingResponse2);

                double similarity = openAIClient.CalculateCosineSimilarity(embedding1, embedding2);
                Console.WriteLine($"\nSemantic Similarity Score: {similarity}");
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}


