using SemanticSimilarity.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSemanticSimilarity.Utilites
{
    /// <summary>
    /// Unit tests for the SimilarityCalculator class.
    /// These tests verify the correctness and robustness of cosine similarity calculations,
    /// including both direct vector comparison and similarity via model-based embeddings.
    /// </summary>
    [TestClass]
    public class SimilarityCalculatorTests
    {
        // Instance of the class that performs similarity calculations
        public SimilarityCalculator similarityHelper = new SimilarityCalculator();


        /// <summary>
        /// Tests that cosine similarity is calculated correctly for valid embeddings.
        /// Cosine similarity should always return a value between -1 and 1.
        /// </summary>
        [TestMethod]
        public void CalculateCosineSimilarity_ValidEmbeddings_ReturnsCorrectSimilarity()
        {
            // Arrange: define two sample embeddings
            float[] embedding1 = { 1, 2, 3 };
            float[] embedding2 = { 4, 5, 6 };

            // Act: calculate similarity
            float similarity = similarityHelper.CalculateCosineSimilarity(embedding1, embedding2);

            // Assert: ensure the result is in the expected range
            Assert.IsTrue(similarity >= -1 && similarity <= 1);
        }

        /// <summary>
        /// Tests that an ArgumentException is thrown when embeddings have different lengths.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when input arrays are of different lengths.</exception>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateCosineSimilarity_EmbeddingsDifferentLengths()
        {
            // Arrange: define mismatched embeddings
            float[] embedding1 = { 1, 2, 3 };
            float[] embedding2 = { 4, 5 };

            // Act: this should throw an ArgumentException
            similarityHelper.CalculateCosineSimilarity(embedding1, embedding2);

            // No need for assert due to ExpectedException
        }

        /// <summary>
        /// Tests that an InvalidOperationException is thrown if one embedding has zero magnitude.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when one or both embeddings have zero magnitude.</exception>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CalculateCosineSimilarity_ZeroMagnitudeEmbedding()
        {
            // Arrange: define a zero vector
            float[] embedding1 = { 0, 0, 0 };
            float[] embedding2 = { 1, 2, 3 };

            // Act: this should throw InvalidOperationException
            similarityHelper.CalculateCosineSimilarity(embedding1, embedding2);

            // Assert is handled by ExpectedException
        }

        /// <summary>
        /// Tests the async similarity calculation for valid inputs using a real embedding model.
        /// </summary>
        [TestMethod]
        public async Task CalculateSimilarityAsync_ValidInputs()
        {
            // Arrange: define valid model and content for source and reference
            string model = "text-embedding-3-large";
            string source = "Cat";
            string reference = "Pet";

            // Act: get similarity score and embeddings
            var (similarity, srcEmbedding, refEmbedding) = await similarityHelper.CalculateSimilarityAsync(model, source, reference);

            // Assert: similarity should be between -1 and 1
            Assert.IsTrue(similarity >= -1 && similarity <= 1);
        }

        /// <summary>
        /// Tests that an ArgumentException is thrown when both source and reference texts are empty.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when input texts are empty.</exception>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task CalculateSimilarityAsync_EmptyInputs()
        {
            // Arrange: empty inputs
            string model = "text-embedding-3-large";
            string source = "";
            string reference = "";

            // Act: this should throw an ArgumentException
            await similarityHelper.CalculateSimilarityAsync(model, source, reference);

            // Assert is handled by ExpectedException
        }

        /// <summary>
        /// Tests that the method returns -1 similarity when an invalid model is used.
        /// This simulates an error condition (e.g., API failure).
        /// </summary>
        [TestMethod]
        public async Task CalculateSimilarityAsync_ErrorOccurs()
        {
            // Arrange: intentionally use an invalid model
            string model = "invalid-model";
            string source = "Cat";
            string reference = "Pet";

            // Act: run the method
            var (similarity, srcEmbedding, refEmbedding) = await similarityHelper.CalculateSimilarityAsync(model, source, reference);

            // Assert: method should return -1 similarity as fallback
            Assert.AreEqual(-1, similarity);
        }
    }
}
