using Xunit;
using System;
using DotNetEnv;
using SemanticSimilarity.Utilites;
using SemanticSimilarity.Utilites;
using Assert = Xunit.Assert;

namespace TestSemanticSimilarity.Utilites
{
    public class ApiKeyProviderTests
    {
        [Fact]
        public void Constructor_WithValidApiKey_SetsApiKey()
        {
            // Arrange
            string expectedApiKey = "test-api-key";

            // Act
            var apiKeyProvider = new ApiKeyProvider(expectedApiKey);

            // Assert
            Assert.Equal(expectedApiKey, apiKeyProvider.GetApiKey());
        }

        [Fact]
        public void Constructor_WithoutApiKey_LoadsFromEnvironmentVariable()
        {
            // Arrange
            string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
            string envFilePath = Path.Combine(projectRoot, ".env");
            Env.Load(envFilePath);
            string expectedApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            // Act
            var apiKeyProvider = new ApiKeyProvider();

            // Assert
            Assert.Equal(expectedApiKey, apiKeyProvider.GetApiKey());

            // Cleanup
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", null);
        }

        [Fact]
        public void GetApiKey_WithValidApiKey_ReturnsApiKey()
        {
            // Arrange
            string expectedApiKey = "test-api-key";
            var apiKeyProvider = new ApiKeyProvider(expectedApiKey);

            // Act
            string actualApiKey = apiKeyProvider.GetApiKey();

            // Assert
            Assert.Equal(expectedApiKey, actualApiKey);
        }
    }
}
