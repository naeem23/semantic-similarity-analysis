using DotNetEnv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticSimilarity.Utilites
{
    public class ApiKeyProvider
    {
        // Get api key from .env file
        // Author: Naeem
        private readonly string _apiKey;

        public ApiKeyProvider(string apiKey = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                Env.Load();
                _apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            }
            else
            {
                _apiKey = apiKey;
            }
        }

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
