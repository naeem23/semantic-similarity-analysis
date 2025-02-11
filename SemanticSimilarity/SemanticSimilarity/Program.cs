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
        }

        static async Task Naeem(string apiKey)
        {
            //set OpenAI api model "text-embedding-3-small/text-embedding-3-large/text-embedding-ada-002"
            var model = "text-embedding-3-small";

            // Initialize the EmbeddingGenerator class with the provided API key and model.
            var generator = new EmbeddingGenerator(apiKey, model);

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
