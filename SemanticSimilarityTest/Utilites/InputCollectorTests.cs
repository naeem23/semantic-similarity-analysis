using Microsoft.VisualStudio.TestPlatform.TestHost;
using SemanticSimilarity.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using XAssert = Xunit.Assert;

namespace TestSemanticSimilarity.Utilites
{
    /// <summary>
    /// Unit tests for the InputCollector class.
    /// This class verifies different user input scenarios like menu display,
    /// valid/invalid inputs, folder reading, and collecting string lists from console.
    /// </summary>
    [TestClass]
    public class InputCollectorTests
    {
        public InputCollector inputCollector = new InputCollector();

        /// <summary>
        /// Verifies if the menu displayed to the user contains all the correct options.
        /// </summary>
        [TestMethod]
        public void DisplayMenu_ShouldDisplayCorrectMenu()
        {
            // Redirect console output to capture it for testing
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                inputCollector.DisplayMenu();

                // Assert: Check for presence of expected menu items
                var result = sw.ToString().Trim();
                Assert.IsTrue(result.Contains("Welcome to Semantic Similarity Analysis!"));
                Assert.IsTrue(result.Contains("1. Word or Phrase Level Comparison"));
                Assert.IsTrue(result.Contains("2. Document Level Comparison"));
                Assert.IsTrue(result.Contains("3. Exit"));
            }
        }

        /// <summary>
        /// Tests GetUserChoice with valid inputs 1, 2, and 3.
        /// </summary>
        [TestMethod]
        public void GetUserChoice_ValidInput()
        {
            // Test all valid inputs (1, 2, 3)
            for (int i = 1; i <= 3; i++)
            {
                using (var sr = new StringReader(i.ToString()))
                {
                    Console.SetIn(sr);
                    int choice = inputCollector.GetUserChoice();

                    // Ensure the returned value matches the input
                    Assert.AreEqual(i, choice);
                }
            }
        }

        /// <summary>
        /// Tests GetUserChoice when the first input is invalid, followed by a valid input.
        /// </summary>
        [TestMethod]
        public void GetUserChoice_InvalidInputThenValidInput()
        {
            // Input: "5" (invalid), followed by "3" (valid)
            using (var sr = new StringReader("5\n3"))
            {
                Console.SetIn(sr);
                int choice = inputCollector.GetUserChoice();
                Assert.AreEqual(3, choice);
            }
        }

        /// <summary>
        /// Tests GetFileContents with an invalid folder path.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when path does not exist.</exception>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileContents_InvalidFolderPath()
        {
            inputCollector.GetFileContents("invalid/path");
        }

        /// <summary>
        /// Tests GetFileContents with a folder that contains no valid files.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when no .txt or supported files are found.</exception>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileContents_NoValidFiles()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);
            inputCollector.GetFileContents(tempFolder);
            Directory.Delete(tempFolder);
        }

        /// <summary>
        /// Tests GetFileContents with a folder that contains valid text files.
        /// </summary>
        [TestMethod]
        public void GetFileContents_ValidFiles()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);
            string filePath = Path.Combine(tempFolder, "test.txt");
            File.WriteAllText(filePath, "Hello, World!");

            var result = inputCollector.GetFileContents(tempFolder);

            // Verify that one file was read and content matches
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Hello, World!", result[0]);

            Directory.Delete(tempFolder, true);
        }

        /// <summary>
        /// Tests when both source and reference inputs are empty ("done" is typed immediately).
        /// </summary>
        [Fact]
        public void GetUserInputs_EmptySourceAndReferenceInputs_ReturnsEmptyLists()
        {
            // Arrange
            var input = new string[] { "done", "done" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputCollector.GetUserInputs();

            // Assert
            XAssert.Empty(sourceContents);
            XAssert.Empty(refContents);
        }

        /// <summary>
        /// Tests when only source inputs are provided.
        /// </summary>
        [Fact]
        public void GetUserInputs_SourceInputOnly_ReturnsSourceList()
        {
            // Arrange
            var input = new string[] { "source1", "source2", "done", "done" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputCollector.GetUserInputs();

            // Assert
            XAssert.Equal(new List<string> { "source1", "source2" }, sourceContents);
            XAssert.Empty(refContents);
        }

        /// <summary>
        /// Tests when only reference inputs are provided.
        /// </summary>
        [Fact]
        public void GetUserInputs_ReferenceInputOnly_ReturnsReferenceList()
        {
            // Arrange
            var input = new string[] { "done", "ref1", "ref2", "done" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputCollector.GetUserInputs();

            // Assert
            XAssert.Empty(sourceContents);
            XAssert.Equal(new List<string> { "ref1", "ref2" }, refContents);
        }

        /// <summary>
        /// Tests when both source and reference inputs are provided.
        /// </summary>
        [Fact]
        public void GetUserInputs_BothSourceAndReferenceInputs_ReturnsBothLists()
        {
            // Arrange
            var input = new string[] { "source1", "source2", "done", "ref1", "ref2", "done" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputCollector.GetUserInputs();

            // Assert
            XAssert.Equal(new List<string> { "source1", "source2" }, sourceContents);
            XAssert.Equal(new List<string> { "ref1", "ref2" }, refContents);
        }

        /// <summary>
        /// Tests case-insensitive "done" keyword to stop input collection.
        /// </summary>
        [Fact]
        public void GetUserInputs_MixedCaseDone_StopsReadingInput()
        {
            // Arrange
            var input = new string[] { "source1", "DoNe", "ref1", "dOnE" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputCollector.GetUserInputs();

            // Assert
            XAssert.Equal(new List<string> { "source1" }, sourceContents);
            XAssert.Equal(new List<string> { "ref1" }, refContents);
        }

        /// <summary>
        /// Tests behavior when no "done" input is provided (infinite input stream error).
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when input runs out unexpectedly.</exception>
        [Fact]
        public void GetUserInputs_NoDoneInput_ThrowsException()
        {
            // Arrange
            var input = new string[] { "source1", "source2" }; // No "done" input
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act & Assert
            XAssert.Throws<InvalidOperationException>(() => inputCollector.GetUserInputs());
        }
    }

    /// <summary>
    /// Helper class to simulate user input from console using a queue of strings.
    /// </summary>
    public class TestConsoleInput : System.IO.TextReader
    {
        private readonly Queue<string> _inputQueue;

        /// <param name="inputs">A sequence of strings to simulate console input.</param>
        public TestConsoleInput(IEnumerable<string> inputs)
        {
            _inputQueue = new Queue<string>(inputs);
        }

        /// <summary>
        /// Returns the next input line from the queue.
        /// </summary>
        /// <returns>Next input string from the simulated console.</returns>
        /// <exception cref="InvalidOperationException">Thrown when input is exhausted before expected.</exception>
        public override string ReadLine()
        {
            if (_inputQueue.Count > 0)
            {
                return _inputQueue.Dequeue();
            }
            throw new InvalidOperationException("No more input available.");
        }

        /// <summary>
        /// Clears all remaining simulated input.
        /// </summary>
        public override void Close()
        {
            _inputQueue.Clear();
        }
    }
}
