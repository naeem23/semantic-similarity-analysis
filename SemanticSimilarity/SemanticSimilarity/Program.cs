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
