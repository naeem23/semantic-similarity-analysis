using Xunit;
using System;
using DotNetEnv;
using SemanticSimilarity.Utilites;
using SemanticSimilarity.Utilites;
using Assert = Xunit.Assert;

namespace TestSemanticSimilarity.Utilites
{
    /// <summary>
    /// Unit tests for the ApiKeyProvider class, verifying its behavior when 
    /// provided with an API key directly or loading it from environment variables.
    /// </summary>
    public class ApiKeyProviderTests
    {
        /// <summary>
        /// Tests that the constructor correctly sets the API key when a valid key is provided.
        /// </summary>
        [Fact]
        public void Constructor_WithValidApiKey_SetsApiKey()
        {
            // Arrange: Define the expected API key
            string expectedApiKey = "test-api-key";

            // Act: Instantiate the ApiKeyProvider with the API key
            var apiKeyProvider = new ApiKeyProvider(expectedApiKey);

            // Assert: Verify the stored key matches the input
            Assert.Equal(expectedApiKey, apiKeyProvider.GetApiKey());
        }

        /// <summary>
        /// Tests that the constructor correctly reads the API key from the environment 
        /// variable when no API key is explicitly provided.
        /// </summary>
        [Fact]
        public void Constructor_WithoutApiKey()
        {
            // Arrange: Locate the .env file by traversing to the project root
            string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            string envFilePath = Path.Combine(projectRoot, ".env");

            // Load environment variables from the .env file
            Env.Load(envFilePath);

            // Read the expected key from the environment variable
            string expectedApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            // Act: Instantiate ApiKeyProvider without passing any key
            var apiKeyProvider = new ApiKeyProvider();

            // Assert: Verify that the key from the environment is used
            Assert.Equal(expectedApiKey, apiKeyProvider.GetApiKey());

            // Cleanup: Clear the environment variable to avoid side effects
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", null);
        }

        /// <summary>
        /// Tests that the GetApiKey method returns the correct key 
        /// when initialized with a valid API key.
        /// </summary>
        [Fact]
        public void GetApiKey_WithValidApiKey_ReturnsApiKey()
        {
            // Arrange: Define and assign a test API key
            string expectedApiKey = "test-api-key";
            var apiKeyProvider = new ApiKeyProvider(expectedApiKey);

            // Act: Retrieve the API key using GetApiKey
            string actualApiKey = apiKeyProvider.GetApiKey();

            // Assert: Ensure the retrieved key matches the original
            Assert.Equal(expectedApiKey, actualApiKey);
        }
    }
}
