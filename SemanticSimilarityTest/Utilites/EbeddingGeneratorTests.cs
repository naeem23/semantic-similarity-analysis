using SemanticSimilarity.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace TestSemanticSimilarity.Utilites
{
    /// <summary>
    /// Unit tests for the EmbeddingGenerator class.
    /// These tests check if embeddings are generated correctly 
    /// and also handle various edge cases like null, empty content, or API errors.
    /// </summary>
    public class EbeddingGeneratorTests
    {
        /// <summary>
        /// Tests if embeddings are successfully generated for valid input content.
        /// </summary>
        [Fact]
        public async Task GenerateEmbeddingsAsync_ValidContent()
        {
            // Arrange: Create an instance of EmbeddingGenerator and define input content
            var embeddingGenerator = new EmbeddingGenerator();
            string content = "This is a test content.";

            // Act: Call the method to generate embeddings
            var embeddings = await embeddingGenerator.GenerateEmbeddingsAsync(content);

            // Assert: Check that the returned embeddings are not null or empty
            Assert.NotNull(embeddings);
            Assert.NotEmpty(embeddings);
        }

        /// <summary>
        /// Tests if the method throws an exception when the content is null.
        /// </summary>
        [Fact]
        public async Task GenerateEmbeddingsAsync_NullContent()
        {
            // Arrange: Create an instance of EmbeddingGenerator and set content to null
            var embeddingGenerator = new EmbeddingGenerator();
            string content = null;

            // Act & Assert: Expect an ArgumentException when calling with null content
            await Assert.ThrowsAsync<ArgumentException>(() => embeddingGenerator.GenerateEmbeddingsAsync(content));
        }

        /// <summary>
        /// Tests if the method throws an exception when the content is an empty string.
        /// </summary>
        [Fact]
        public async Task GenerateEmbeddingsAsync_EmptyContent()
        {
            // Arrange: Create an instance of EmbeddingGenerator and set content to empty
            var embeddingGenerator = new EmbeddingGenerator();
            string content = string.Empty;

            // Act & Assert: Expect an ArgumentException when calling with empty content
            await Assert.ThrowsAsync<ArgumentException>(() => embeddingGenerator.GenerateEmbeddingsAsync(content));
        }

        /// <summary>
        /// Tests if the method throws an exception when an invalid model name causes an API error.
        /// </summary>
        [Fact]
        public async Task GenerateEmbeddingsAsync_ApiError()
        {
            // Arrange: Create an instance and use a fake model name to simulate an API error
            var embeddingGenerator = new EmbeddingGenerator();
            string content = "This is a test content.";

            // Consider an API error. For simplicity, assuming the model is invalid
            // Act & Assert: Expect an InvalidOperationException when the model name is invalid
            await Assert.ThrowsAsync<InvalidOperationException>(() => embeddingGenerator.GenerateEmbeddingsAsync(content, "fake-embedding-model"));
        }
    }
}
