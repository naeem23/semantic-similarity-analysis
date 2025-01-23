using DotNetEnv;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Embeddings;

namespace SemanticSimilarity
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Env.Load();

            //ChatClient client = new(model: "gpt-4o", apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            //ChatCompletion completion = client.CompleteChat("Say 'this is a test.'");

            //Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");

            //OpenAIClient client = new(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            //AudioClient ttsClient = client.GetAudioClient("tts-1");
            //AudioClient whisperClient = client.GetAudioClient("whisper-1");

            //Console.WriteLine($"tts client {ttsClient}");
            //Console.WriteLine($"whisper client {whisperClient}");

            //generate text embeddings
            EmbeddingClient client = new EmbeddingClient("text-embedding-ada-002", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            //accept user input for comparison:
            //Console.WriteLine("Enter first text:");
            //string input1 = Console.ReadLine();
            //Console.WriteLine("Enter second text:");
            //string input2 = Console.ReadLine();

            ////generate embeddings
            //OpenAIEmbedding embedding1 = await client.GenerateEmbeddingAsync(input1);
            //OpenAIEmbedding embedding2 = await client.GenerateEmbeddingAsync(input2);

            //ReadOnlyMemory<float> vector1 = embedding1.ToFloats();
            //ReadOnlyMemory<float> vector2 = embedding2.ToFloats();

            //float similarity = CosineSimilarity(vector1 ,vector2);

            //Console.WriteLine($"Similarity in \"{input1}\" and \"{input2}\" is {similarity}");

            // Optional: Handle multiple comparisons
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
            //            float pairwiseSimilarity = CosineSimilarity(embeddings[i], embeddings[j]);
            //            Console.WriteLine($"Similarity between \"{texts[i]}\" and \"{texts[j]}\" is {pairwiseSimilarity:F4}");
            //        }
            //    }
            //}

            //reading document 
            Console.WriteLine("Enter the path to the first document:");
            string docPath1 = Console.ReadLine();

            Console.WriteLine("Enter the path of the second document:");
            string docPath2 = Console.ReadLine();

            //Read the document content 
            string document1 = File.ReadAllText(docPath1);
            string document2 = File.ReadAllText(docPath2);

            Console.WriteLine(document1);
            Console.WriteLine("\n\n\n");
            Console.WriteLine(document2);
        }

        //function to calculate cosine similarity
        static double CalculateCosineSimilarity(ReadOnlyMemory<float> vector1, ReadOnlyMemory<float> vector2)
        {
            float[] vec1 = vector1.ToArray();
            float[] vec2 = vector2.ToArray();

            //calculate dot product 
            double dotProduct = vec1.Zip(vec2, (v1, v2) => v1 * v2).Sum();

            //calculate magnitude (norms)
            double magnitude1 = Math.Sqrt(vec1.Sum(v => v * v));
            double magnitude2 = Math.Sqrt(vec2.Sum(v => v * v));

            //return cosine similarity
            return dotProduct / (magnitude1 * magnitude2);
        }

        //another function to calculate cosine similarity
        static float CosineSimilarity(ReadOnlyMemory<float> vectorA, ReadOnlyMemory<float> vectorB)
        {
            float dotProduct = 0;
            float magnitudeA = 0;
            float magnitudeB = 0;

            for (int i = 0; i < vectorA.Length; i++) 
            {
                dotProduct += vectorA.Span[i] * vectorB.Span[i];
                magnitudeA += vectorA.Span[i] * vectorA.Span[i];
                magnitudeB += vectorB.Span[i] * vectorB.Span[i];
            }

            return dotProduct / (MathF.Sqrt(magnitudeA) * MathF.Sqrt(magnitudeB));
        }
    }
}
