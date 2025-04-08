using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
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
    /// Unit tests for the OutputGenerator class.
    /// These tests ensure the output CSV file is generated correctly
    /// under various input conditions.
    /// </summary>
    public class OutputGeneratorTests
    {
        // Instance of the class under test
        public OutputGenerator outputHelper = new OutputGenerator();

        // Root directory of the project
        private static readonly string ProjectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;

        // Expected output CSV file path
        private static readonly string OutputFilePath = Path.Combine(ProjectRoot, "Output", "similarity_results.csv");


        /// <summary>
        /// Verifies that the CSV file is generated when valid inputs are provided.
        /// </summary>
        [Fact]
        public async Task GenerateOutputAsync_ShouldGenerateCsvFile_WhenValidInputIsProvided()
        {
            // Arrange: define sample source and reference texts
            var sourceContents = new List<string> { "Cat", "Tesla" };
            var refContents = new List<string> { "Pet Animal", "Car" };

            // Act: call the method to generate the CSV file
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert: confirm file was created or not
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup: delete the generated file
            File.Delete(OutputFilePath);
        }

        /// <summary>
        /// Ensures the method handles an empty source list without error and still generates a file.
        /// </summary>
        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleEmptySourceContentList()
        {
            // Arrange: define sample source and reference texts
            var sourceContents = new List<string>(); // Empty source
            var refContents = new List<string> { "This is a reference content." };

            // Act: call the method to generate the CSV file
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert: confirm file was created or not
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup: delete the generated file
            File.Delete(OutputFilePath);
        }

        /// <summary>
        /// Ensures the method handles an empty reference list without error and still generates a file.
        /// </summary>
        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleEmptyRefContentList()
        {
            // Arrange: define sample source and reference texts
            var sourceContents = new List<string> { "This is a source content." };
            var refContents = new List<string>(); // Empty reference

            // Act: call the method to generate the CSV file
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert: confirm file was created or not
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup: delete the generated file
            File.Delete(OutputFilePath);
        }

        /// <summary>
        /// Ensures that even when both source and reference inputs are empty, the method still creates a file.
        /// </summary>
        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleBothEmptyLists()
        {
            // Arrange: define empty source and reference
            var sourceContents = new List<string>();
            var refContents = new List<string>();

            // Act: call the method to generate the CSV file
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert: confirm file was created or not
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup: delete the generated file
            File.Delete(OutputFilePath);
        }

        /// <summary>
        /// Verifies that the method can handle long content strings without failing.
        /// </summary>
        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleLongContentStrings()
        {
            // Arrange: define long source and reference texts
            var longString = new string('a', 10000);
            var sourceContents = new List<string> { longString };
            var refContents = new List<string> { longString };

            // Act: call the method to generate the CSV file
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert: confirm file was created or not
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup: delete the generated file
            File.Delete(OutputFilePath);
        }

        /// <summary>
        /// Ensures special characters in input strings are handled properly and don't break file generation.
        /// </summary>
        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleSpecialCharactersInContent()
        {
            // Arrange:: define sample source and reference texts with special characters
            var sourceContents = new List<string> { "This is a source content with special characters: !@#$%^&*()" };
            var refContents = new List<string> { "This is a reference content with special characters: !@#$%^&*()" };

            // Act: call the method to generate the CSV file
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert: confirm file was created or not
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup: delete the generated file
            File.Delete(OutputFilePath);
        }

        /// <summary>
        /// Verifies that the method does not throw exceptions for valid inputs.
        /// </summary>
        [Fact]
        public async Task GenerateOutputAsync_ShouldNotThrowException()
        {
            // Arrange: define sample valid source and reference texts
            var sourceContents = new List<string> { "Cat", "Tesla" };
            var refContents = new List<string> { "Pet Animal", "Car" };

            // Act: call the method to generate the CSV file
            var exception = await Record.ExceptionAsync(() => outputHelper.GenerateOutputAsync(sourceContents, refContents));

            // Assert: confirm no exception is thrown
            Assert.Null(exception);
        }
    }
}
