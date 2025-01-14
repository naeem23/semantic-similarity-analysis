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
            
            //first pair of phrases
            string phrase1 = "Angela Merkel";
            string phrase2 = "Government";
            string phrase3 = "Cristiano Ronaldo";

            //generate embeddings for each phrase of first pair 
            OpenAIEmbedding embedding1 = await client.GenerateEmbeddingAsync(phrase1);
            OpenAIEmbedding embedding2 = await client.GenerateEmbeddingAsync(phrase2);
            OpenAIEmbedding embedding3 = await client.GenerateEmbeddingAsync(phrase3);

            ReadOnlyMemory<float> vector1 = embedding1.ToFloats();
            ReadOnlyMemory<float> vector2 = embedding2.ToFloats();
            ReadOnlyMemory<float> vector3 = embedding3.ToFloats();

            float similarityInPhrase1Phrase2 = CosineSimilarity(vector1 ,vector2);
            float similarityInPhrase3Phrase2 = CosineSimilarity(vector3 ,vector2);

            Console.WriteLine($"Similarity in {phrase1} and {phrase2} is {similarityInPhrase1Phrase2}");
            //0.22698620638210207 with text-embedding-3-small model
            //0.781814538890557 with text-embedding-ada-002 model
            Console.WriteLine($"Similarity in {phrase3} and {phrase2} is {similarityInPhrase3Phrase2}");
            //0.15764999859482054 with text-embedding-3-small model
            //0.7536560297012329 with text-embedding-ada-002 model
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
