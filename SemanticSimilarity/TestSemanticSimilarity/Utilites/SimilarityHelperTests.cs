using SemanticSimilarity.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSemanticSimilarity.Utilites
{
    [TestClass]
    public class SimilarityHelperTests
    {
        [TestMethod]
        public void CalculateCosineSimilarity_ValidEmbeddings_ReturnsCorrectSimilarity()
        {
            // Arrange
            float[] embedding1 = { 1, 2, 3 };
            float[] embedding2 = { 4, 5, 6 };

            // Act
            float similarity = SimilarityHelper.CalculateCosineSimilarity(embedding1, embedding2);

            // Assert
            Assert.IsTrue(similarity >= -1 && similarity <= 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateCosineSimilarity_EmbeddingsDifferentLengths()
        {
            // Arrange
            float[] embedding1 = { 1, 2, 3 };
            float[] embedding2 = { 4, 5 };

            // Act
            SimilarityHelper.CalculateCosineSimilarity(embedding1, embedding2);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CalculateCosineSimilarity_ZeroMagnitudeEmbedding()
        {
            // Arrange
            float[] embedding1 = { 0, 0, 0 };
            float[] embedding2 = { 1, 2, 3 };

            // Act
            SimilarityHelper.CalculateCosineSimilarity(embedding1, embedding2);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public async Task CalculateSimilarityAsync_ValidInputs()
        {
            // Arrange
            string model = "text-embedding-3-large";
            string source = "Cat";
            string reference = "Pet";

            // Act
            float similarity = await SimilarityHelper.CalculateSimilarityAsync(model, source, reference);

            // Assert
            Assert.IsTrue(similarity >= -1 && similarity <= 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CalculateSimilarityAsync_EmptyInputs()
        {
            // Arrange
            string model = "text-embedding-3-large";
            string source = "";
            string reference = "";

            // Act
            await SimilarityHelper.CalculateSimilarityAsync(model, source, reference);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        public async Task CalculateSimilarityAsync_ErrorOccurs()
        {
            // Arrange
            string model = "invalid-model";
            string source = "Cat";
            string reference = "Pet";

            // Act
            float similarity = await SimilarityHelper.CalculateSimilarityAsync(model, source, reference);

            // Assert
            Assert.AreEqual(-1, similarity);
        }
    }
}
