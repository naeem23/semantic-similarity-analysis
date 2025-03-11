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
        /// Get two file path for comparison. please provide absolute path.
        /// e.g. G:\FUAS\SE\semantic-similarity-analysis\SemanticSimilarity\SemanticSimilarity\Input\Sources\document.txt
        /// Author: Naeem
        /// </summary>
        /// <returns>two string</returns>
        public static (string, string) GetFilePathInput()
        {
            while (true)
            {
                Console.Write("Enter the path to the first file: ");
                string file1 = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Enter the path to the second file: ");
                string file2 = Console.ReadLine()?.Trim() ?? "";

                if (!File.Exists(file1) || !File.Exists(file2))
                {
                    Console.WriteLine("One or both files do not exist. Please try again.");
                }
                else
                {
                    return (file1, file2);
                }
            }
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
        public static List<string> GetTextFileContent(List<string> documentPaths) 
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
        }

        /// <summary>
        /// Function to real all .txt files from a directory  
        /// Author: Naeem
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns>list of string</returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static async Task<List<string>> ReadAllFilesInFolderAsync(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");
            }

            var files = Directory.GetFiles(folderPath, "*.txt");
            var fileContents = new List<string>();

            foreach (var file in files)
            {
                string content = await File.ReadAllTextAsync(file);
                fileContents.Add(content);
            }

            return fileContents;
        }

        /// <summary>
        /// Function to read reference keywords file and split by new line
        /// Author: Naeem
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>List of string</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static async Task<List<string>> ReadRefKeywordsAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var content = await File.ReadAllTextAsync(filePath);
            return content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Take multiple source and reference inputs from user
        /// Author: Ahad
        /// </summary>
        /// <returns>List of string, list of string</returns>
        public static (List<string>, List<string>) GetUserInputs()
        {
            List<string> user1Inputs = new List<string>();
            List<string> user2Inputs = new List<string>();

            Console.WriteLine("Enter Source Input (Type 'done' to finish):");
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "done") break;
                user1Inputs.Add(input);
            }

            Console.WriteLine("Enter Reference Input (Type 'done' to finish):");
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "done") break;
                user2Inputs.Add(input);
            }

            return (user1Inputs, user2Inputs);
        }
    }
}

    /*
     * //start: ahad test code 
    public class InputHelper
    {
        //private readonly string _apiKey;
        string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"); //use this structure for all
        private const string BaseUrl = "https://api.openai.com/v1/embeddings";

        public InputHelper(string apiKey)
        {
            apiKey = apiKey;
        } //

        

        // Get embeddings from OpenAI API
        public async Task<List<double>> GetEmbeddingAsync(string text, string model = "text-embedding-ada-002")
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", $"Bearer {apiKey}");
            request.AddHeader("Content-Type", "application/json");

            var body = new { input = text, model = model };
            request.AddJsonBody(body);

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error: {response.StatusDescription}\n{response.Content}");
            }

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
            var embedding = jsonResponse["data"][0]["embedding"].ToObject<List<double>>();
            return embedding;
        }

        // Calculate cosine similarity
        public double CalculateCosineSimilarity(List<double> vectorA, List<double> vectorB)
        {
            if (vectorA.Count != vectorB.Count)
                throw new ArgumentException("Vectors must be of the same length.");

            double dotProduct = 0.0, magnitudeA = 0.0, magnitudeB = 0.0;

            for (int i = 0; i < vectorA.Count; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += Math.Pow(vectorA[i], 2);
                magnitudeB += Math.Pow(vectorB[i], 2);
            }

            return dotProduct / (Math.Sqrt(magnitudeA) * Math.Sqrt(magnitudeB));
        }

        // Compare all user inputs, calculate similarity scores, and save to CSV
        public async Task CompareUserInputsAndSaveToCSV(List<string> user1Inputs, List<string> user2Inputs, string csvFilePath)
        {
            List<string> csvLines = new List<string>
        {
            "Source Input,Reference Input,Similarity Score"
        };

            for (int i = 0; i < user1Inputs.Count; i++)
            {
                for (int j = 0; j < user2Inputs.Count; j++)
                {
                    string text1 = user1Inputs[i];
                    string text2 = user2Inputs[j];

                    List<double> vectorA = await GetEmbeddingAsync(text1);
                    List<double> vectorB = await GetEmbeddingAsync(text2);

                    double similarity = CalculateCosineSimilarity(vectorA, vectorB);

                    Console.WriteLine($"Similarity between \"{text1}\" and \"{text2}\" → {similarity:F4}");

                    // Add data to CSV
                    csvLines.Add($"\"{text1}\",\"{text2}\",{similarity:F4}");
                }
            }

            // Write results to CSV file
            File.WriteAllLines(csvFilePath, csvLines);
            Console.WriteLine($" Congratulation!!! Results successfully Saved to: {csvFilePath}");
            //I have to generated a csv file
        }
    }
    // end ahad test code
    */


