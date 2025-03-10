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
    //public static class InputHelper
    //{ //from naeem
    //    public static List<string> TextInputHandler()
    //    {
    //        var contents = new List<string>();
    //        Console.WriteLine("Start entering your content.");
    //        Console.WriteLine("Type \"END_CONTENT\" to finish a content.");
    //        Console.WriteLine("Type \"START_PROCESS\" to start similarity calculation.\n");

    //        while (true)
    //        {
    //            Console.WriteLine("Enter a new content:");
    //            var contentBuilder = new System.Text.StringBuilder();

    //            while (true)
    //            {
    //                string line = Console.ReadLine();
    //                if (line?.Trim().ToUpper() == "END_CONTENT")
    //                {
    //                    if (contentBuilder.Length > 0)
    //                    {
    //                        contents.Add(contentBuilder.ToString().Trim());
    //                        Console.WriteLine("Content saved.\n");
    //                    }
    //                    else
    //                    {
    //                        Console.WriteLine("Please add some text.");
    //                    }
    //                    break;
    //                }
    //                else if (line?.Trim().ToUpper() == "START_PROCESS")
    //                {
    //                    if (contents.Count < 2)
    //                    {
    //                        Console.WriteLine("Error: You must enter at least two content.\n");
    //                        break;
    //                    }
    //                    if (contentBuilder.Length > 0)
    //                    { 
    //                        contents.Add(contentBuilder.ToString().Trim());
    //                        Console.WriteLine("Last content saved.\n");
    //                    } 
    //                    Console.WriteLine("Finished collecting contents.");
    //                    return contents;
    //                }
    //                else
    //                {
    //                    contentBuilder.AppendLine(line);
    //                }
    //            }
    //        }
    //    }

    //    public static List<string> GetFilePaths()
    //    {
    //        Console.WriteLine("Please enter document path separated by new line:");

    //        var paths = new List<string>();
    //        string? path;

    //        while((path = Console.ReadLine()) != string.Empty)
    //        {
    //            if (!string.IsNullOrEmpty(path))
    //            {
    //                paths.Add(path);
    //            }
    //        }
    //        return paths;
    //    }

    //    public static List<string> GetTextFileContent(List<string> documentPaths) 
    //    { 
    //        var contents = new List<string>();

    //        foreach (var path in documentPaths)
    //        {
    //            if (!File.Exists(path))
    //            {
    //                throw new FileNotFoundException($"The file '{path}' does not exist.");
    //            }

    //            if (Path.GetExtension(path).ToLower() != ".txt") 
    //            {
    //                throw new InvalidDataException($"The file '{path}' is not a text file.");
    //            }

    //            var content = File.ReadAllText(path);
    //            if (!string.IsNullOrWhiteSpace(content))
    //            {
    //                contents.Add(content);
    //            }
    //        }

    //        return contents;
    //    }
    //} //to naeem

        // from Ahad

public class InputHelper
    {
        //private readonly string _apiKey;
        string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY"); //use this structure for all
        private const string BaseUrl = "https://api.openai.com/v1/embeddings";

        public InputHelper(string apiKey)
        {
            apiKey = apiKey;
        } //

        // Function to take multiple inputs from two users
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
        }
    
    //end ahad


