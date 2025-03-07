using OpenAI.Embeddings;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SentenceTransformers;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace SemanticSimilarity.Utilites
{
    public static class InputHelper
    {
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

        public static List<string> GetFilePaths()
        {
            Console.WriteLine("Please enter document path separated by new line:");

            var paths = new List<string>();
            string? path;

            while((path = Console.ReadLine()) != string.Empty)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    paths.Add(path);
                }
            }
            return paths;
        }

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


<<<<<<< Updated upstream
=======
        //Function to real all .txt files from a directory  
        //author: Naeem
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

        // Function to read reference keywords file and split by new line
        //Author: Naeem23
        public static async Task<List<string>> ReadRefKeywordsAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var content = await File.ReadAllTextAsync(filePath);
            return content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        // Method to read file paths and contents for data1 or data2
        public static async Task<List<string>> ReadFilesAsync(int numberOfFiles, string dataType)
        {
            var fileContents = new List<string>();

            for (int i = 0; i < numberOfFiles; i++)
            {
                Console.WriteLine($"Enter the path for {dataType} file {i + 1}:");
                string filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    string content = await File.ReadAllTextAsync(filePath);
                    fileContents.Add(content);
                }
                else
                {
                    Console.WriteLine("File does not exist. Please try again.");
                    i--; // Retry for the same file index
                }
            }

            return fileContents;
        }

        // Method to compare all files from data1 with all files from data2
        public static void CompareFiles(List<string> data1Files, List<string> data2Files)
        {
            // Load a pre-trained Sentence Transformer model
            var model = new SentenceTransformer("all-MiniLM-L6-v2"); // Lightweight model for semantic similarity

            for (int i = 0; i < data1Files.Count; i++)
            {
                for (int j = 0; j < data2Files.Count; j++)
                {
                    Console.WriteLine($"Comparing data1 file {i + 1} with data2 file {j + 1}...");

                    // Get embeddings for the texts
                    var embedding1 = model.Encode(data1Files[i]);
                    var embedding2 = model.Encode(data2Files[j]);

                    // Calculate cosine similarity
                    double similarity = CosineSimilarity(embedding1, embedding2);

                    Console.WriteLine($"Semantic similarity: {similarity:P2}");
                }
            }
        }
        //Faraz
        // Method to calculate cosine similarity between two vectors
        private static double CosineSimilarity(float[] vector1, float[] vector2)
        {
            double dotProduct = 0.0;
            double magnitude1 = 0.0;
            double magnitude2 = 0.0;

            for (int i = 0; i < vector1.Length; i++)
            {
                dotProduct += vector1[i] * vector2[i];
                magnitude1 += Math.Pow(vector1[i], 2);
                magnitude2 += Math.Pow(vector2[i], 2);
            }

            magnitude1 = Math.Sqrt(magnitude1);
            magnitude2 = Math.Sqrt(magnitude2);

            if (magnitude1 == 0 || magnitude2 == 0)
            {
                return 0.0; // Avoid division by zero
            }

            return dotProduct / (magnitude1 * magnitude2);
        }
>>>>>>> Stashed changes
        //Faraz

        // Method to read file paths and contents for data1 or data2
        public static async Task<List<string>> ReadFilesAsync(int numberOfFiles, string dataType)
        {
            var fileContents = new List<string>();

            for (int i = 0; i < numberOfFiles; i++)
            {
                Console.WriteLine($"Enter the path for {dataType} file {i + 1}:");
                string filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    string content = await File.ReadAllTextAsync(filePath);
                    fileContents.Add(content);
                }
                else
                {
                    Console.WriteLine("File does not exist. Please try again.");
                    i--; // Retry for the same file index
                }
            }

            return fileContents;
        }

        // Method to compare all files from data1 with all files from data2
        public static void CompareFiles(List<string> data1Files, List<string> data2Files)
        {
            // Load a pre-trained Sentence Transformer model
            var model = new SentenceTransformer("all-MiniLM-L6-v2"); // Lightweight model for semantic similarity

            for (int i = 0; i < data1Files.Count; i++)
            {
                for (int j = 0; j < data2Files.Count; j++)
                {
                    Console.WriteLine($"Comparing data1 file {i + 1} with data2 file {j + 1}...");

                    // Get embeddings for the texts
                    var embedding1 = model.Encode(data1Files[i]);
                    var embedding2 = model.Encode(data2Files[j]);

                    // Calculate cosine similarity
                    double similarity = CosineSimilarity(embedding1, embedding2);

                    Console.WriteLine($"Semantic similarity: {similarity:P2}");
                }
            }
        }

        // Method to calculate cosine similarity between two vectors
        private static double CosineSimilarity(float[] vector1, float[] vector2)
        {
            double dotProduct = 0.0;
            double magnitude1 = 0.0;
            double magnitude2 = 0.0;

            for (int i = 0; i < vector1.Length; i++)
            {
                dotProduct += vector1[i] * vector2[i];
                magnitude1 += Math.Pow(vector1[i], 2);
                magnitude2 += Math.Pow(vector2[i], 2);
            }

            magnitude1 = Math.Sqrt(magnitude1);
            magnitude2 = Math.Sqrt(magnitude2);

            if (magnitude1 == 0 || magnitude2 == 0)
            {
                return 0.0; // Avoid division by zero
            }

            return dotProduct / (magnitude1 * magnitude2);
        }

        private static InferenceSession _session;

        //Author: Faraz
        // Load the ONNX model
        public static void LoadModel(string modelPath)
        {
            _session = new InferenceSession(modelPath);
        }

        // Method to read file paths and contents for data1 or data2
        public static async Task<List<string>> ReadFilesAsync(int numberOfFiles, string dataType)
        {
            var fileContents = new List<string>();

            for (int i = 0; i < numberOfFiles; i++)
            {
                Console.WriteLine($"Enter the path for {dataType} file {i + 1}:");
                string filePath = Console.ReadLine();

                if (File.Exists(filePath))
                {
                    string content = await File.ReadAllTextAsync(filePath);
                    fileContents.Add(content);
                }
                else
                {
                    Console.WriteLine("File does not exist. Please try again.");
                    i--; // Retry for the same file index
                }
            }

            return fileContents;
        }

        // Method to compare all files from data1 with all files from data2
        public static void CompareFiles(List<string> data1Files, List<string> data2Files)
        {
            for (int i = 0; i < data1Files.Count; i++)
            {
                for (int j = 0; j < data2Files.Count; j++)
                {
                    Console.WriteLine($"Comparing data1 file {i + 1} with data2 file {j + 1}...");

                    // Get embeddings for the texts
                    var embedding1 = GetEmbedding(data1Files[i]);
                    var embedding2 = GetEmbedding(data2Files[j]);

                    // Calculate cosine similarity
                    double similarity = CosineSimilarity(embedding1, embedding2);

                    Console.WriteLine($"Semantic similarity: {similarity:P2}");
                }
            }
        }

        // Method to get embeddings using the ONNX model
        private static float[] GetEmbedding(string text)
        {
            // Tokenize the input text (you may need a tokenizer for your specific model)
            var inputs = new Dictionary<string, Tensor<string>>
        {
            { "input_text", new DenseTensor<string>(new[] { text }, new[] { 1 }) }
        };

            // Run inference
            using var results = _session.Run(inputs);
            var embedding = results.First().Value as DenseTensor<float>;

            return embedding.ToArray();
        }

        // Method to calculate cosine similarity between two vectors
        private static double CosineSimilarity(float[] vector1, float[] vector2)
        {
            double dotProduct = 0.0;
            double magnitude1 = 0.0;
            double magnitude2 = 0.0;

            for (int i = 0; i < vector1.Length; i++)
            {
                dotProduct += vector1[i] * vector2[i];
                magnitude1 += Math.Pow(vector1[i], 2);
                magnitude2 += Math.Pow(vector2[i], 2);
            }

            magnitude1 = Math.Sqrt(magnitude1);
            magnitude2 = Math.Sqrt(magnitude2);

            if (magnitude1 == 0 || magnitude2 == 0)
            {
                return 0.0; // Avoid division by zero
            }

            return dotProduct / (magnitude1 * magnitude2);
        }

    }
}
