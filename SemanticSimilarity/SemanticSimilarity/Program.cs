using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try // Semantic Similarity Score
        {
            string apiKey = "Null"; //Put OpenAI Key here
            var openAIClient = new OpenAIClient(apiKey);

            Console.WriteLine("Enter the first text:");
            string text1 = Console.ReadLine();

            Console.WriteLine("\nEnter the second text:");
            string text2 = Console.ReadLine();
            Console.WriteLine("Calculating embeddings...");
            
            string embeddingResponse1 = await openAIClient.GetEmbedding(text1);
            string embeddingResponse2 = await openAIClient.GetEmbedding(text2);

            List<double> embedding1 = ParseEmbedding(embeddingResponse1);
            List<double> embedding2 = ParseEmbedding(embeddingResponse2);
        
            double similarity = openAIClient.CalculateCosineSimilarity(embedding1, embedding2);
            Console.WriteLine($"\nSemantic Similarity Score: {similarity}");
}

        catch (Exception ex)
{
    Console.WriteLine("An error occurred:");
    Console.WriteLine(ex.Message);
}
