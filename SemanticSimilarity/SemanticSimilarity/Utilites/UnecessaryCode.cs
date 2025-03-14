﻿using OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticSimilarity.Utilites
{
    internal class UnecessaryCode
    {
        /*static async Task Ahad(string apiKey)
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
                var generator = new EmbeddingGenerator();

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

                MultipleFileSimilarityProcessor processor = new MultipleFileSimilarityProcessor();
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
                CustomCsvWriter.SaveResultsToCsv("Compare Documents Similarity Score.csv", results);
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
            while (true)
            {
                InputHelper.DisplayMenu();
                int choice = InputHelper.GetUserChoice();

                if (choice == 1)
                {
                    Console.WriteLine("\nWord or Phrase Level Comparison");
                    (List<string> sourceContents, List<string> refContents) = InputHelper.GetUserInputs();
                    await OutputHelper.GenerateOutputAsync(sourceContents, refContents);
                    Environment.Exit(0);
                }
                else if (choice == 2)
                {
                    Console.WriteLine("\nDocument Level Comparison");
                    Console.Write("Enter source documents folder path: \n");
                    string sourceFolder = Console.ReadLine()?.Trim() ?? "";
                    Console.Write("Enter reference documents folder path: \n");
                    string refFolder = Console.ReadLine()?.Trim() ?? "";

                    var sourceContents = InputHelper.GetFileContents(sourceFolder);
                    var refContents = InputHelper.GetFileContents(refFolder);

                    await OutputHelper.GenerateOutputAsync(sourceContents, refContents);

                    Environment.Exit(0);
                }
                else if (choice == 3)
                {
                    Console.WriteLine("Exiting the program. Goodbye!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }

            //// word or phrase level comparison
            //var word1 = new List<string> { "Cat", "Dog", "Car", "Bicycle" };
            //var word2 = new List<string> { "Animal", "Transport" };

            //// Document level comparison
            //string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            //string sourceFilePaths = Path.Combine(projectRoot, "Input", "Sources");
            //string refFilePaths = Path.Combine(projectRoot, "Input", "References");
            ////string refKeywordsFilePath = Path.Combine(projectRoot, "Input", "reference_keywords.txt");

            //var sourceContents = await InputHelper.ReadAllFilesInFolderAsync(sourceFilePaths);
            //var refContents = await InputHelper.ReadAllFilesInFolderAsync(refFilePaths);
            ////var refKeywords = await InputHelper.ReadRefKeywordsAsync(refKeywordsFilePath); // Read and split reference keywords file

            //await OutputHelper.GenerateOutputAsync(sourceContents, refContents);
            ////set OpenAI api model "text-embedding-3-small/text-embedding-3-large/text-embedding-ada-002"
            //var model = "text-embedding-3-small";

            //// Initialize the EmbeddingGenerator class with the provided API key and model.
            //var generator = new EmbeddingGenerator(apiKey, model);

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

            //try
            //{
            //    var contents = InputHelper.TextInputHandler();
            //    Console.WriteLine("\nYou have entered the following articles:\n");

            //    for (int i = 0; i < contents.Count; i++)
            //    {
            //        Console.WriteLine($"Article {i + 1}:\n{contents[i]}");
            //        Console.WriteLine(new string('-', 50));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}

            //// Optional: Handle multiple comparisons
            //Console.WriteLine("\nDo you want to compare multiple texts? (y/n)");
            //if (Console.ReadLine().Trim().ToLower() == "y")
            //{
            //    Console.WriteLine("Enter texts separated by commas:");
            //    string[] texts = Console.ReadLine().Split(",").Select(x => x.Trim()).ToArray();

            //    //Generate embedding for all inputs
            //    List<ReadOnlyMemory<float>> embeddings = new List<ReadOnlyMemory<float>>();

            //    foreach (string text in texts)
            //    {
            //        OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(text);
            //        ReadOnlyMemory<float> vector = embedding.ToFloats();
            //        embeddings.Add(vector);
            //    }

            //    // Calculate pairwise similarity and display results
            //    Console.WriteLine("\nPairwise Similarity:");
            //    for (int i = 0; i < texts.Length; i++)
            //    {
            //        for (int j = i + 1; j < texts.Length; j++)
            //        {
            //            float pairwiseSimilarity = SimilarityHelper.CalcCosineSimilarityMethod2(embeddings[i], embeddings[j]);
            //            Console.WriteLine($"Similarity between \"{texts[i]}\" and \"{texts[j]}\" is {pairwiseSimilarity:F4}");
            //        }
            //    }
            //}

            //    // Calculate pairwise similarity and display results
            //    Console.WriteLine("\nPairwise Similarity:");
            //    for (int i = 0; i < texts.Length; i++)
            //    {
            //        for (int j = i + 1; j < texts.Length; j++)
            //        {
            //            float pairwiseSimilarity = SimilarityHelper.CalcCosineSimilarityMethod2(embeddings[i], embeddings[j]);
            //            Console.WriteLine($"Similarity between \"{texts[i]}\" and \"{texts[j]}\" is {pairwiseSimilarity:F4}");
            //        }
            //    }
            //}
        }

        static async Task Haimanti(string apiKey)
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
                var generator = new EmbeddingGenerator();

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
        }*/
    }
}
