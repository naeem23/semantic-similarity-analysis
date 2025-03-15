using Xunit;
using System;
using SemanticSimilarity.Utilites;

namespace TestSemanticSimilarity.Utilites
{
    public class UtilitiesTests
    {
        [Fact]
        public void GetApiKey_WhenApiKeyIsSet()
        {
            // Arrange
            string expectedApiKey = "your-test-key-here";
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", expectedApiKey);

            try
            {
                // Act
                string actualApiKey = Utilities.GetApiKey();

                // Assert
                Xunit.Assert.Equal(expectedApiKey, actualApiKey);
            }

            finally
            {
                // Cleanup
                Environment.SetEnvironmentVariable("OPENAI_API_KEY", null);
            }
        }

        [Fact]
        public void GetApiKey_WhenApiKeyIsNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", null);

            try
            {
                // Act & Assert
                var exception = Xunit.Assert.Throws<InvalidOperationException>(() => Utilities.GetApiKey());

                // Assert exception message
                Xunit.Assert.Equal("API key cannot be null or empty. Please set the OPENAI_API_KEY environment variable.", exception.Message);
            }
            finally
            {
                // Cleanup (optional)
                Environment.SetEnvironmentVariable("OPENAI_API_KEY", null);
            }
        }

        [Fact]
        public void GetApiKey_WhenApiKeyIsEmpty()
        {
            // Arrange
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", "");

            try
            {
                // Act & Assert
                var exception = Xunit.Assert.Throws<InvalidOperationException>(() => Utilities.GetApiKey());

                // Assert exception message
                Xunit.Assert.Equal("API key cannot be null or empty. Please set the OPENAI_API_KEY environment variable.", exception.Message);
            }
            finally
            {
                // Cleanup (optional)
                Environment.SetEnvironmentVariable("OPENAI_API_KEY", null);
            }
        }

        [Fact]
        public void GetApiKey_WhenApiKeyIsWhitespace()
        {
            // Arrange
            Environment.SetEnvironmentVariable("OPENAI_API_KEY", "   ");

            try
            {
                // Act & Assert
                var exception = Xunit.Assert.Throws<InvalidOperationException>(() => Utilities.GetApiKey());

                // Assert exception message
                Xunit.Assert.Equal("API key cannot be null or empty. Please set the OPENAI_API_KEY environment variable.", exception.Message);
            }
            finally
            {
                // Cleanup (optional)
                Environment.SetEnvironmentVariable("OPENAI_API_KEY", null);
            }
        }
    }
}
