using DotNetEnv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;

namespace SemanticSimilarity.Utilites
{
    public class Utilities
    {
        // Get api key from .env file
        // Author: Naeem
        public static string GetApiKey()
        {
            // Load environment variables from .env file (if applicable)
            Env.Load();

            // Get OpenAI API key from environment variables
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            // Checks for null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("API key cannot be null or empty. Please set the OPENAI_API_KEY environment variable.");
            }

            return apiKey;
        }
    }
}
