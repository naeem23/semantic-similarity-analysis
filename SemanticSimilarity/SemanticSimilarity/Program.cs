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
            Env.Load();

            var documentPaths = InputHelper.GetFilePaths();
            var textContents = InputHelper.GetTextFileContent(documentPaths);

            Console.WriteLine("Contents of text files:");
            foreach (var content in textContents)
            {
                Console.WriteLine(content);
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

            //ChatClient client = new(model: "gpt-4o", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            //ChatCompletion completion = client.CompleteChat("Say 'this is a test.'");

            //Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");

            //OpenAIClient client = new(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            //AudioClient ttsClient = client.GetAudioClient("tts-1");
            //AudioClient whisperClient = client.GetAudioClient("whisper-1");

            //Console.WriteLine($"tts client {ttsClient}");
            //Console.WriteLine($"whisper client {whisperClient}");

            ////generate text embeddings
            //EmbeddingClient client = new EmbeddingClient("text-embedding-ada-002", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            ////accept user input for comparison:
            //Console.WriteLine("Enter first text:");
            //string input1 = Console.ReadLine();
            //Console.WriteLine("Enter second text:");
            //string input2 = Console.ReadLine();

            ////generate embeddings
            //OpenAIEmbedding embedding1 = await client.GenerateEmbeddingAsync(input1);
            //OpenAIEmbedding embedding2 = await client.GenerateEmbeddingAsync(input2);

            //ReadOnlyMemory<float> vector1 = embedding1.ToFloats();
            //ReadOnlyMemory<float> vector2 = embedding2.ToFloats();

            //float similarity = SimilarityHelper.CalcCosineSimilarityMethod2(vector1, vector2);

            //Console.WriteLine($"Similarity in \"{input1}\" and \"{input2}\" is {similarity}");

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

            ////reading document 
            //Console.WriteLine("Enter the path to the first document:");
            //string docPath1 = Console.ReadLine();

            //Console.WriteLine("Enter the path of the second document:");
            //string docPath2 = Console.ReadLine();

            ////Read the document content 
            //string document1 = File.ReadAllText(docPath1);
            //string document2 = File.ReadAllText(docPath2);

            ////generate embeddings
            //OpenAIEmbedding embedding3 = await client.GenerateEmbeddingAsync(document1);
            //OpenAIEmbedding embedding4 = await client.GenerateEmbeddingAsync(document2);

            //ReadOnlyMemory<float> vector3 = embedding3.ToFloats();
            //ReadOnlyMemory<float> vector4 = embedding4.ToFloats();

            //float similarity34 = SimilarityHelper.CalcCosineSimilarityMethod2(vector3, vector4);

            //Console.WriteLine($"Similarity in two files is {similarity34}");
        }
    }
}
