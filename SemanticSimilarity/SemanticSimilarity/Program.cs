using DotNetEnv;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;

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
            // Input texts for embeddings

            OpenAIClient client = new(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

            AudioClient ttsClient = client.GetAudioClient("tts-1");
            AudioClient whisperClient = client.GetAudioClient("whisper-1");

            Console.WriteLine($"tts client {ttsClient}");
            Console.WriteLine($"whisper client {whisperClient}");
        }

    }
}
