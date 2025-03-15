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
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                InputHelper.DisplayMenu();
                int choice = InputHelper.GetUserChoice();

                if (choice == 1)
                {
                    Console.WriteLine("\nWord or Phrase Level Comparison");
                    (List<string> sourceContents, List<string> refContents) = InputHelper.GetUserInputs();
                    await OutputHelper.GenerateOutputAsync(sourceContents, refContents);
                    Environment.Exit(0);
                }
                else if (choice == 2)
                {
                    Console.WriteLine("\nDocument Level Comparison");
                    Console.Write("Enter source documents folder path: \n");
                    string sourceFolder = Console.ReadLine()?.Trim() ?? "";
                    Console.Write("Enter reference documents folder path: \n");
                    string refFolder = Console.ReadLine()?.Trim() ?? "";

                    var sourceContents = InputHelper.GetFileContents(sourceFolder);
                    var refContents = InputHelper.GetFileContents(refFolder);

                    await OutputHelper.GenerateOutputAsync(sourceContents, refContents);

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


