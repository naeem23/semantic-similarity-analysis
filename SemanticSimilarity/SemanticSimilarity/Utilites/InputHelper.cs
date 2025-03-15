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
    public static class InputHelper
    {
        /// <summary>
        /// Display menu to user with following options 
        /// 1. word or phrase level comparison, 
        /// 2. document level comparison
        /// 3. exit the program
        /// Author: Naeem
        /// </summary>
        public static void DisplayMenu()
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
        public static int GetUserChoice()
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
        /// Get user input: it can be a word or phrase. user will be able to input two words or phrasess
        /// Author: Naeem
        /// </summary>
        /// <returns>two string</returns>
        public static (string, string) GetWordOrPhraseInput()
        {
            Console.Write("Enter the first word or phrase: ");
            string input1 = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Enter the second word or phrase: ");
            string input2 = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(input1) || string.IsNullOrEmpty(input2))
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
                return GetWordOrPhraseInput();
            }
            return (input1, input2);
        }

        /// <summary>
        /// Gets the contents of all .txt and .pdf files in the specified folder.
        /// Author: Naeem
        /// </summary>
        /// <param name="folderPath">The path to the folder.</param>
        /// <returns>A list of file contents.</returns>
        public static List<string> GetFileContents(string folderPath)
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


        //Collect source and reference content from user
        public static List<string> TextInputHandler()
        {
            var contents = new List<string>();
            Console.WriteLine("Start entering your content.");
            Console.WriteLine("Type \"END_CONTENT\" to finish a content.");
            Console.WriteLine("Type \"START_PROCESS\" to start similarity calculation.\n");

            while (true)
            {
                Console.WriteLine("Enter a new content:");
                var contentBuilder = new System.Text.StringBuilder();

                while (true)
                {
                    string line = Console.ReadLine();
                    if (line?.Trim().ToUpper() == "END_CONTENT")
                    {
                        if (contentBuilder.Length > 0)
                        {
                            contents.Add(contentBuilder.ToString().Trim());
                            Console.WriteLine("Content saved.\n");
                        }
                        else
                        {
                            Console.WriteLine("Please add some text.");
                        }
                        break;
                    }
                    else if (line?.Trim().ToUpper() == "START_PROCESS")
                    {
                        if (contents.Count < 2)
                        {
                            Console.WriteLine("Error: You must enter at least two content.\n");
                            break;
                        }
                        if (contentBuilder.Length > 0)
                        {
                            contents.Add(contentBuilder.ToString().Trim());
                            Console.WriteLine("Last content saved.\n");
                        }
                        Console.WriteLine("Finished collecting contents.");
                        return contents;
                    }
                    else
                    {
                        contentBuilder.AppendLine(line);
                    }
                }
            }
        }

        //Get source and reference files path  
        public static List<string> GetFilePaths()
        {
            Console.WriteLine("Please enter document path separated by new line:");

            var paths = new List<string>();
            string? path;

            while ((path = Console.ReadLine()) != string.Empty)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    paths.Add(path);
                }
            }
            return paths;
        }

        //Get .txt file content 
        /*public static List<string> GetTextFileContent(List<string> documentPaths) 
        { 
            var contents = new List<string>();

            foreach (var path in documentPaths)
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"The file '{path}' does not exist.");
                }

                if (Path.GetExtension(path).ToLower() != ".txt")
                {
                    throw new InvalidDataException($"The file '{path}' is not a text file.");
                }

                var content = File.ReadAllText(path);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    contents.Add(content);
                }
            }

            return contents;
        }*/

        /// <summary>
        /// Function to read reference keywords file and split by new line
        /// Author: Naeem
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>List of string</returns>
        /// <exception cref="FileNotFoundException"></exception>
        /*public static async Task<List<string>> ReadRefKeywordsAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var content = await File.ReadAllTextAsync(filePath);
            return content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }*/

        /// <summary>
        /// Take multiple source and reference inputs from user
        /// Author: Ahad
        /// </summary>
        /// <returns>List of string, list of string</returns>
        /// TO-DO: do unit test
        public static (List<string>, List<string>) GetUserInputs()
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