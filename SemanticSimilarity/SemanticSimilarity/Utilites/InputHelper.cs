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
using Newtonsoft.Json;
using RestSharp;

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
            List<string> Enter_Source_Input = new List<string>();
            List<string> Enter_Reference_Input = new List<string>();
            Console.WriteLine("Enter Source Input(Type 'done' to finish):");
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "done") break;
                Enter_Source_Input.Add(input);
            }
            Console.WriteLine("Enter Reference Input(Type 'done' to finish):");
            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLower() == "done") break;
                Enter_Reference_Input.Add(input);
            }

            return (Enter_Source_Input, Enter_Reference_Input);
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

        // Compare all user inputs and calculate similarity scores
        public async Task CompareUserInputs(List<string> Enter_Source_Input, List<string> Enter_Reference_Input)
        {
            for (int i = 0; i < Enter_Source_Input.Count; i++)
            {
                for (int j = 0; j < Enter_Reference_Input.Count; j++)
                {
                    string text1 = Enter_Source_Input[i];
                    string text2 = Enter_Reference_Input[j];

                    List<double> vectorA = await GetEmbeddingAsync(text1);
                    List<double> vectorB = await GetEmbeddingAsync(text2);

                    double similarity = CalculateCosineSimilarity(vectorA, vectorB);
                    Console.WriteLine($"Similarity Between \"{text1}\" and \"{text2}\" → {similarity:F4}");
                    //I have to generated a csv file
                }
            }
        }
    } 
    //end ahad

}
