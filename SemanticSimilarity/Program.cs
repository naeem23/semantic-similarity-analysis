using DotNetEnv;
using Newtonsoft.Json.Linq;
using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
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
    /// <summary>
    /// cloud Project
    /// Main program class that serves as the entry point for the similarity comparison application.
    /// Provides a menu-driven interface for comparing text content at either word/phrase or document level.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// Presents a menu and handles user choices for comparison operations.
        /// The program offers three options:
        /// 1. Word/phrase level comparison (manual text input)
        /// 2. Document level comparison (file path input)
        /// 3. Exit the program
        /// </summary>
        /// <param name="args">Command-line arguments (not currently used)</param>
        /// <returns>Task representing the asynchronous operation</returns>
        static async Task Main(string[] args)
        {
            // Main program loop - continues until user chooses to exit
            while (true)
            {
                // Initialize required services
                InputCollector inputCollector = new InputCollector();
                OutputGenerator outputGenerator = new OutputGenerator();

                // Display main menu options
                inputCollector.DisplayMenu();

                // Get user's menu selection
                int choice = inputCollector.GetUserChoice();

                // Handle user's choice
                if (choice == 1) // Word/phrase level comparison
                {
                    Console.WriteLine("\nWord or Phrase Level Comparison");

                    // Get user inputs for source and reference texts
                    (List<string> sourceContents, List<string> refContents) = inputCollector.GetUserInputs();

                    // Generate and output similarity results
                    await outputGenerator.GenerateOutputAsync(sourceContents, refContents);

                    // Exit after completion
                    Environment.Exit(0);
                }
                else if (choice == 2) // Document level comparison
                {
                    Console.WriteLine("\nDocument Level Comparison");

                    // Get folder paths from user
                    Console.Write("Enter source documents folder path: \n");
                    string sourceFolder = Console.ReadLine()?.Trim() ?? "";

                    Console.Write("Enter reference documents folder path: \n");
                    string refFolder = Console.ReadLine()?.Trim() ?? "";

                    Console.WriteLine("\nReading file contents...");

                    // Read all files from specified folders
                    var sourceContents = inputCollector.GetFileContents(sourceFolder);
                    var refContents = inputCollector.GetFileContents(refFolder);

                    // Generate and output similarity results
                    await outputGenerator.GenerateOutputAsync(sourceContents, refContents);

                    // Exit after completion
                    Environment.Exit(0);
                }
                else if (choice == 3) // Exit program
                {
                    Console.WriteLine("Exiting the program. Goodbye!");
                    Environment.Exit(0);
                }
                else // Invalid choice
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            }
        }

        // Retrieve the connection string for use with the application. 
        string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

        // Create a BlobServiceClient object 
        var blobServiceClient = new BlobServiceClient(connectionString);
    }
}


