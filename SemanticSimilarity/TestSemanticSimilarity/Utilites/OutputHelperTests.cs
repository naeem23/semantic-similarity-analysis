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
    public class OutputHelperTests
    {
        public OutputGenerator outputHelper = new OutputGenerator();
        private static readonly string ProjectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
        private static readonly string OutputFilePath = Path.Combine(ProjectRoot, "Output", "similarity_results.csv");

        [Fact]
        public async Task GenerateOutputAsync_ShouldGenerateCsvFile_WhenValidInputIsProvided()
        {
            // Arrange
            var sourceContents = new List<string> { "Cat", "Tesla" };
            var refContents = new List<string> { "Pet Animal", "Car" };

            // Acttry
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup
            File.Delete(OutputFilePath);
        }

        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleEmptySourceContentList()
        {
            // Arrange
            var sourceContents = new List<string>();
            var refContents = new List<string> { "This is a reference content." };

            // Act
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup
            File.Delete(OutputFilePath);
        }

        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleEmptyRefContentList()
        {
            // Arrange
            var sourceContents = new List<string> { "This is a source content." };
            var refContents = new List<string>();

            // Act
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup
            File.Delete(OutputFilePath);
        }

        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleBothEmptyLists()
        {
            // Arrange
            var sourceContents = new List<string>();
            var refContents = new List<string>();

            // Act
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup
            File.Delete(OutputFilePath);
        }

        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleLongContentStrings()
        {
            // Arrange
            var longString = new string('a', 10000);
            var sourceContents = new List<string> { longString };
            var refContents = new List<string> { longString };

            // Act
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup
            File.Delete(OutputFilePath);
        }

        [Fact]
        public async Task GenerateOutputAsync_ShouldHandleSpecialCharactersInContent()
        {
            // Arrange
            var sourceContents = new List<string> { "This is a source content with special characters: !@#$%^&*()" };
            var refContents = new List<string> { "This is a reference content with special characters: !@#$%^&*()" };

            // Act
            await outputHelper.GenerateOutputAsync(sourceContents, refContents);

            // Assert
            Console.WriteLine($"Output File Path: {OutputFilePath}");
            Assert.True(File.Exists(OutputFilePath), $"File not found at: {OutputFilePath}");

            // Cleanup
            File.Delete(OutputFilePath);
        }

        [Fact]
        public async Task GenerateOutputAsync_ShouldNotThrowException()
        {
            // Arrange
            var sourceContents = new List<string> { "Cat", "Tesla" };
            var refContents = new List<string> { "Pet Animal", "Car" };

            // Act
            var exception = await Record.ExceptionAsync(() => outputHelper.GenerateOutputAsync(sourceContents, refContents));

            // Assert
            Assert.Null(exception);
        }
    }
}
