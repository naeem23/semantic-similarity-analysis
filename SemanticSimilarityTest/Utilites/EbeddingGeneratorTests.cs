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
    public class EbeddingGeneratorTests
    {
        [Fact]
        public async Task GenerateEmbeddingsAsync_ValidContent()
        {
            // Arrange
            var embeddingGenerator = new EmbeddingGenerator();
            string content = "This is a test content.";

            // Act
            var embeddings = await embeddingGenerator.GenerateEmbeddingsAsync(content);

            // Assert
            Assert.NotNull(embeddings);
            Assert.NotEmpty(embeddings);
        }

        [Fact]
        public async Task GenerateEmbeddingsAsync_NullContent()
        {
            // Arrange
            var embeddingGenerator = new EmbeddingGenerator();
            string content = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => embeddingGenerator.GenerateEmbeddingsAsync(content));
        }

        [Fact]
        public async Task GenerateEmbeddingsAsync_EmptyContent()
        {
            // Arrange
            var embeddingGenerator = new EmbeddingGenerator();
            string content = string.Empty;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => embeddingGenerator.GenerateEmbeddingsAsync(content));
        }

        [Fact]
        public async Task GenerateEmbeddingsAsync_ApiError()
        {
            // Arrange
            var embeddingGenerator = new EmbeddingGenerator();
            string content = "This is a test content.";

            // Cconsider an API error. For simplicity, assuming the model is invalid
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => embeddingGenerator.GenerateEmbeddingsAsync(content, "fake-embedding-model"));
        }
    }
}
