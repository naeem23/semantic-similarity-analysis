using OpenAI.Embeddings;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
