using DotNetEnv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticSimilarity.Utilites
{
    /// <summary>
    /// Provides a way to retrieve the OpenAI API key either from constructor parameters or environment variables.
    /// This class first checks for an API key passed to its constructor. If none is provided,
    /// it looks for the key in a .env file or system environment variables.
    /// Author: Naeem
    /// </summary>
    public class ApiKeyProvider
    {
        // Stores the API key once it's loaded
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the ApiKeyProvider class.
        /// If no API key is provided, this constructor will:
        /// 1. Load environment variables from .env file (if exists)
        /// 2. Look for OPENAI_API_KEY in the environment variables
        /// </summary>
        /// <param name="apiKey">Optional API key. If not provided, loads from environment variables.</param>
        public ApiKeyProvider(string apiKey = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                // Load environment variables from .env file
                Env.Load();
                // Get API key from environment variables
                _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            }
            else
            {
                // Use the API key provided in constructor
                _apiKey = apiKey;
            }
        }

        /// <summary>
        /// Retrieves the stored API key.
        /// Always call this method to get the API key rather than accessing it directly.
        /// This ensures validation is performed and proper error handling.
        /// </summary>
        /// <returns>The OpenAI API key as a string</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the API key is null or empty, indicating the key wasn't found.
        /// </exception>
        public string GetApiKey()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new InvalidOperationException("API key cannot be null or empty. Please set the OPENAI_API_KEY environment variable.");
            }

            return _apiKey;
        }
    }
}
