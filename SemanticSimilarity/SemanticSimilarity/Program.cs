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
            EmbeddingClient client = new("text-embedding-3-small", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
            
            //first pair of phrases
            string phrase1 = "Angela Merkel";
            string phrase2 = "Government";
            string phrase3 = "Cristiano Ronaldo";

            //generate embeddings for each phrase of first pair 
            OpenAIEmbedding embedding1 = client.GenerateEmbedding(phrase1);
            ReadOnlyMemory<float> vector1 = embedding1.ToFloats();

            OpenAIEmbedding embedding2 = client.GenerateEmbedding(phrase2);
            ReadOnlyMemory<float> vector2 = embedding2.ToFloats();

            OpenAIEmbedding embedding3 = client.GenerateEmbedding(phrase3);
            ReadOnlyMemory<float> vector3 = embedding3.ToFloats();

            double similarityInPhrase1Phrase2 = CalculateCosineSimilarity(vector1 ,vector2);
            double similarityInPhrase3Phrase2 = CalculateCosineSimilarity(vector3 ,vector2);

            Console.WriteLine($"Similarity in {phrase1} and {phrase2} is {similarityInPhrase1Phrase2}");
            Console.WriteLine($"Similarity in {phrase3} and {phrase2} is {similarityInPhrase3Phrase2}");
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

    }
}
