using Newtonsoft.Json;
using OpenAI.Embeddings;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using static System.Net.WebRequestMethods;

namespace SemanticSimilarity.Utilites
{
    /// <summary>
    /// Handles user interaction and input collection for semantic similarity analysis.
    /// Provides methods for displaying menus, collecting user choices, and gathering input content
    /// from both direct user input and files. Supports text and PDF file processing.
    /// Author: Naeem
    /// </summary>
    public class InputCollector
    {
        /// <summary>
        /// Displays the main menu options for semantic similarity analysis.
        /// Presents three options:
        /// 1. Word/phrase level comparison
        /// 2. Document level comparison
        /// 3. Program exit
        /// </summary>
        public void DisplayMenu()
        {
            Console.WriteLine("\nWelcome to Semantic Similarity Analysis!");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Word or Phrase Level Comparison");
            Console.WriteLine("2. Document Level Comparison");
            Console.WriteLine("3. Exit");
        }

        /// <summary>
        /// Collects and validates the user's menu choice.
        /// Continuously prompts the user until a valid input (1, 2, or 3) is provided.
        /// Handles non-numeric and out-of-range inputs.
        /// </summary>
        /// <returns>Integer value between 1-3 representing the user's selection</returns>
        public int GetUserChoice()
        {
            while (true)
            {
                Console.Write("Enter your choice (1-3): ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 3)
                {
                    return choice;
                }
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
            }
        }

        /// <summary>
        /// Retrieves text content from all supported files in a specified folder.
        /// Processes both .txt and .pdf files in the specified folder.
        /// Skips unsupported file types automatically.
        /// </summary>
        /// <param name="folderPath">Path to the folder containing files to process</param>
        /// <returns>List of strings containing each file's text content</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the folder doesn't exist or contains no supported files
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown when there are problems reading any of the files
        /// </exception>
        public List<string> GetFileContents(string folderPath)
        {
            // Validate folder existence
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("The specified folder path is invalid or does not exist.");
            }

            // Get all .txt and .pdf files in the folder
            var files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                                 .Where(file => file.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                                 .ToList();

            // Check if any supported files were found
            if (files.Count == 0)
            {
                throw new ArgumentException("The folder does not contain any .txt or .pdf files.");
            }

            // Process each file and collect contents
            var fileContents = new List<string>();
            foreach (var file in files)
            {
                try
                {
                    FileProcessor fileProcessor = new FileProcessor();
                    string content = fileProcessor.ReadFileText(file);
                    fileContents.Add(content);
                }
                catch (Exception ex)
                {
                    throw new IOException($"An error occurred while reading the file: {file}", ex);
                }
            }

            return fileContents;
        }

        /// <summary>
        /// Collects multiple text inputs from the user interactively.
        /// Guides the user to enter:
        /// 1. Source inputs (type 'done' to finish)
        /// 2. Reference inputs (type 'done' to finish)
        /// Returns both collections separately for comparison purposes.
        /// Empty inputs are allowed and preserved in the returned lists.
        /// Author: Ahad
        /// </summary>
        /// <returns>
        /// Tuple containing:
        /// - List of source inputs
        /// - List of reference inputs
        /// </returns>
        public (List<string>, List<string>) GetUserInputs()
        {
            List<string> sourceContents = new List<string>();
            List<string> refContents = new List<string>();

            Console.WriteLine("Enter Source Input (Type 'done' to finish):");
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "done") break;
                sourceContents.Add(input);
            }

            Console.WriteLine("Enter Reference Input (Type 'done' to finish):");
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "done") break;
                refContents.Add(input);
            }

            return (sourceContents, refContents);
        }
    }
}