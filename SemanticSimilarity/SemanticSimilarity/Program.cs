using DotNetEnv;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Embeddings;
using SemanticSimilarity.Utilites;
using CsvHelper;
using System.Globalization;

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


            // await Ahad(apiKey);
            //await Ahad(apiKey);
            //await Naeem(apiKey);

            await Faraz(apiKey);
            await FarazOnyx(apiKey);
            await Abbasi(apiKey);
        }


        //ahad
        //static async Task Ahad(string apiKey)
        //{
          //  try // Semantic Similarity Score
          //  {
            //    Console.WriteLine("Enter the first text:");
              //  string text1 = Console.ReadLine();

                //Console.WriteLine("\nEnter the second text:");
                //string text2 = Console.ReadLine();

//                Console.WriteLine("Calculating embeddings...");

                //set OpenAI api model "text-embedding-3-small/text-embedding-3-large/text-embedding-ada-002"
  //              var model = "text-embedding-3-small";

                // Initialize the EmbeddingGenerator class with the provided API key and model.
    //            var generator = new EmbeddingGenerator(apiKey, model);

      //          string embeddingResponse1 = await generator.GetEmbedding(text1);
        //        string embeddingResponse2 = await generator.GetEmbedding(text2);

          //      List<double> embedding1 = EmbeddingGenerator.ParseEmbedding(embeddingResponse1);
            //    List<double> embedding2 = EmbeddingGenerator.ParseEmbedding(embeddingResponse2);

              //  double similarity = SimilarityHelper.CalculateCosineSimilarity3(embedding1, embedding2);
                //Console.WriteLine($"\nSemantic Similarity Score: {similarity}");
            //}
            //catch (Exception ex)
            //{
              //  Console.WriteLine("An error occurred:");
                //Console.WriteLine(ex.Message);
            //}


            //Method to Compare Documents me
            //{
              //  var documentProcessor = new DocumentProcessor(apiKey);
              //  Console.WriteLine("\n");
                // Load documents
                //Console.WriteLine("             ***********************************************");
                //Console.WriteLine("             *******Compare Documents Similarity Score******");
                //Console.WriteLine("             ***********************************************");

//                Console.WriteLine("\nEnter the path of the first document:");
  //              string doc1Path = Console.ReadLine();
    //            string doc1Content = documentProcessor.LoadDocument(doc1Path);

      //          Console.WriteLine("\nEnter the path of the second document:");
        //        string doc2Path = Console.ReadLine();
          //      string doc2Content = documentProcessor.LoadDocument(doc2Path);

                // Preprocess the documents
            //    string processedDoc1 = documentProcessor.PreprocessText(doc1Content);
              //  string processedDoc2 = documentProcessor.PreprocessText(doc2Content);
              
              //  Console.WriteLine("Processing documents...");

                // Generate embeddings
                //List<double> embedding1 = await documentProcessor.GetEmbeddingAsync(processedDoc1);
                //List<double> embedding2 = await documentProcessor.GetEmbeddingAsync(processedDoc2);

                // Calculate similarity
//                double similarityScore = documentProcessor.CalculateCosineSimilarity(embedding1, embedding2);

                // Display results
  //              Console.WriteLine($"\nSemantic Similarity Score: {similarityScore:F4}");

                // Interpret results
    //            if (similarityScore > 0.8)
      //              Console.WriteLine("Documents are highly related.");
        //        else if (similarityScore > 0.5)
          //          Console.WriteLine("Documents are somewhat related.");
            //    else
              //      Console.WriteLine("Documents are not related.");
           // }

        //}

        //static async Task Naeem(string[] args)
        //{

            //load environment file 
          //  Env.Load();

            //get OpenAI api key if null or empty throw error
            //var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            //if (string.IsNullOrEmpty(apiKey))
            //{
              //  throw new InvalidOperationException("API key cannot be null or empty. Please set the OPENAI_API_KEY environment variable.");
            //}

            //set OpenAI api model "text-embedding-3-small/text-embedding-3-large/text-embedding-ada-002"
            //var model = "text-embedding-3-small";

            // Initialize the EmbeddingGenerator class with the provided API key and model.
            //var generator = new EmbeddingGenerator(apiKey, model);

            //get document paths from user 
            //var documentPaths = InputHelper.GetFilePaths();

            //get content of the documents 
            //var textContents = InputHelper.GetTextFileContent(documentPaths);

            //embedding value for keyword "Climate"
            //var keywordEmbedding = await generator.GenerateEmbeddingsAsync("Climate");

            //generate embedding for document content and calculate similarity with keyword 
            //foreach (var content in textContents)
            //{
              //  var embedding = await generator.GenerateEmbeddingsAsync(content);
                //float similarity = SimilarityHelper.CalcCosineSimilarityMethod2(embedding, keywordEmbedding);
                //Console.WriteLine($"Similarity is {similarity}");
                //Console.WriteLine("=========================================");
            //}

  
static async Task Haimanti(string[] args)
        {
            // Load environment file
            Env.Load();

            // Get OpenAI API key
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key cannot be null or empty. Please set the OPENAI_API_KEY environment variable.");
            }

            // Initialize DocumentProcessor
            var documentProcessor = new DocumentProcessor(apiKey);

            // Get document paths from user
            Console.WriteLine("Enter the path of the first document:");
            string doc1Path = Console.ReadLine()?.Trim('"');

            Console.WriteLine("Enter the path of the second document:");
            string doc2Path = Console.ReadLine()?.Trim('"');

            try
            {
                // Load and preprocess documents
                string doc1Content = documentProcessor.LoadDocument(doc1Path);
                string doc2Content = documentProcessor.LoadDocument(doc2Path);

                string processedDoc1 = documentProcessor.PreprocessText(doc1Content);
                string processedDoc2 = documentProcessor.PreprocessText(doc2Content);

                // Generate embeddings
                List<double> embedding1 = await documentProcessor.GetEmbeddingAsync(processedDoc1);
                List<double> embedding2 = await documentProcessor.GetEmbeddingAsync(processedDoc2);

                // Calculate similarity
                double similarityScore = documentProcessor.CalculateCosineSimilarity(embedding1, embedding2);

                // Create a list of similarity results
                var results = new List<SimilarityResult>
        {
            new SimilarityResult
            {
                Document1 = doc1Path,
                Document2 = doc2Path,
                SimilarityScore = similarityScore
            }
        };

                // Write results to CSV
                string csvFilePath = "similarity_results.csv";
                using (var writer = new StreamWriter(csvFilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(results);
                }

                Console.WriteLine($"Similarity results saved to {csvFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        static async Task Faraz(string apiKey)
        {
            // Step 1: Ask for the number of data1 files
            Console.WriteLine("How many data1 files do you want to compare?");
            int data1Count = int.Parse(Console.ReadLine());

            // Step 2: Read data1 files
           // var data1Files = await InputHelper.ReadFilesAsync(data1Count, "data1");

            // Step 3: Ask for the number of data2 files
            Console.WriteLine("How many data2 files do you want to compare?");
            int data2Count = int.Parse(Console.ReadLine());

            // Step 4: Read data2 files
            var data2Files = await InputHelper.ReadFilesAsync(data2Count, "data2");

            // Step 5: Compare all data1 files with all data2 files
            InputHelper.CompareFiles(data1Files, data2Files);
        }

        static async Task FarazOnyx(string apiKey)
        {
            // Load the ONNX model
            string modelPath = "path/to/your/model.onnx";
            InputHelper.LoadModel(modelPath);

            // Step 1: Ask for the number of data1 files
            Console.WriteLine("How many data1 files do you want to compare?");
            int data1Count = int.Parse(Console.ReadLine());

            // Step 2: Read data1 files
            //var data1Files = await InputHelper.ReadFilesAsync(data1Count, "data1");

            // Step 3: Ask for the number of data2 files
            Console.WriteLine("How many data2 files do you want to compare?");
            int data2Count = int.Parse(Console.ReadLine());

            // Step 4: Read data2 files
           // var data2Files = await InputHelper.ReadFilesAsync(data2Count, "data2");

            // Step 5: Compare all data1 files with all data2 files
            InputHelper.CompareFiles(data1Files, data2Files);
        }

        static void Abbasi(string[] args)
        {
            // Step 1: Define two sentences for semantic similarity analysis
            string sentence1 = "I love programming in C#.";
            string sentence2 = "Coding in C# is fun and exciting.";

            // Step 2: Perform semantic similarity analysis
            double similarityScore = CalculateSemanticSimilarity(sentence1, sentence2);

            // Step 3: Create a result object
            var result = new SemanticSimilarityResult
            {
                Sentence1 = sentence1,
                Sentence2 = sentence2,
                SimilarityScore = similarityScore
            };

            // Step 4: Add the result to a list
            var results = new List<SemanticSimilarityResult> { result };

            // Step 5: Save the results to a CSV file
            string csvFilePath = "semantic_similarity_results.csv";
            OutputHelper.WriteToCsv(csvFilePath, results);

            Console.WriteLine($"Results saved to {csvFilePath}");
        }

        // Method to calculate semantic similarity (dummy implementation)
        private static double CalculateSemanticSimilarity(string sentence1, string sentence2)
        {
            // Replace this with your actual semantic similarity logic or API call
            // For now, it returns a random similarity score between 0 and 1
            Random random = new Random();
            return random.NextDouble();
        }
    }

}

}


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
   