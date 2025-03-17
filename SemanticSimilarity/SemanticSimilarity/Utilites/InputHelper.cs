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
    public class InputHelper
    {
        /// <summary>
        /// Display menu to user with following options 
        /// 1. word or phrase level comparison, 
        /// 2. document level comparison
        /// 3. exit the program
        /// Author: Naeem
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
        /// Get user choice for options from menu
        /// Author: Naeem
        /// </summary>
        /// <returns>a integer value range from 1 - 3</returns>
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
        /// Gets the contents of all .txt and .pdf files in the specified folder.
        /// Author: Naeem
        /// </summary>
        /// <param name="folderPath">The path to the folder.</param>
        /// <returns>A list of file contents.</returns>
        public List<string> GetFileContents(string folderPath)
        {
            // Validate the folder path
            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("The specified folder path is invalid or does not exist.");
            }

            // Get all .txt and .pdf files in the folder
            var files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                                 .Where(file => file.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                                 .ToList();

            // Check if there are any valid files
            if (files.Count == 0)
            {
                throw new ArgumentException("The folder does not contain any .txt or .pdf files.");
            }

            // Read the contents of the files
            var fileContents = new List<string>();
            foreach (var file in files)
            {
                try
                {
                    var multiFileProcessor = new MultipleFileSimilarityProcessor();
                    string content = multiFileProcessor.ReadFileText(file);
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
        /// Take multiple source and reference inputs from user
        /// Author: Ahad
        /// </summary>
        /// <returns>List of string, list of string</returns>
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