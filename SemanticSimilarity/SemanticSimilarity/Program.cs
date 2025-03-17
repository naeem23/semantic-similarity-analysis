using DotNetEnv;
using Newtonsoft.Json.Linq;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using System;
using OpenAI.Embeddings;
using SemanticSimilarity.Utilites;
using System.Formats.Asn1;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading.Tasks;
using iText.Layout.Splitting;
using static System.Net.WebRequestMethods;

namespace SemanticSimilarity
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                var inputHelper = new InputHelper();
                var outputHelper = new OutputHelper();

                inputHelper.DisplayMenu();
                int choice = inputHelper.GetUserChoice();

                if (choice == 1)
                {
                    Console.WriteLine("\nWord or Phrase Level Comparison");
                    (List<string> sourceContents, List<string> refContents) = inputHelper.GetUserInputs();
                    await outputHelper.GenerateOutputAsync(sourceContents, refContents);
                    Environment.Exit(0);
                }
                else if (choice == 2)
                {
                    Console.WriteLine("\nDocument Level Comparison");
                    Console.Write("Enter source documents folder path: \n");
                    string sourceFolder = Console.ReadLine()?.Trim() ?? "";
                    Console.Write("Enter reference documents folder path: \n");
                    string refFolder = Console.ReadLine()?.Trim() ?? "";

                    Console.WriteLine("\nReading file contents...");
                    var sourceContents = inputHelper.GetFileContents(sourceFolder);
                    var refContents = inputHelper.GetFileContents(refFolder);

                    await outputHelper.GenerateOutputAsync(sourceContents, refContents);

                    Environment.Exit(0);
                }
                else if (choice == 3)
                {
                    Console.WriteLine("Exiting the program. Goodbye!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }
    }
}


