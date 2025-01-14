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

            //generate embeddings for each phrase of first pair 
            OpenAIEmbedding embedding1 = client.GenerateEmbedding(phrase1);
            ReadOnlyMemory<float> vector1 = embedding1.ToFloats();

            OpenAIEmbedding embedding2 = client.GenerateEmbedding(phrase2);
            ReadOnlyMemory<float> vector2 = embedding2.ToFloats();


            Console.WriteLine($"High-dimensional numerical representations of {phrase1} is {vector1}");
            Console.WriteLine($"High-dimensional numerical representations of {phrase2} is {vector2}");
        }

    }
}
