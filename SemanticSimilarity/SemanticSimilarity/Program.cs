using DotNetEnv;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Embeddings;
using SemanticSimilarity.Utilites;

namespace SemanticSimilarity
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //load environment file 
            Env.Load();

            //get OpenAI api key if null or empty throw error
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key cannot be null or empty. Please set the OPENAI_API_KEY environment variable.");
            }

            //await Ahad(apiKey);
            await Naeem(apiKey);
        }

        //ahad
        static async Task Ahad(string apiKey)
        {
            try // Semantic Similarity Score
            {
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
            string refKeywordsFilePath = Path.Combine(projectRoot, "Input", "reference_keywords.txt");

            var sourceContents = await InputHelper.ReadAllFilesInFolderAsync(sourceFilePaths);
            var refKeywords = await InputHelper.ReadRefKeywordsAsync(refKeywordsFilePath); // Read and split reference keywords file

            await OutputHelper.GenerateOutputAsync(sourceContents, refKeywords, apiKey);

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
        }
    }
}