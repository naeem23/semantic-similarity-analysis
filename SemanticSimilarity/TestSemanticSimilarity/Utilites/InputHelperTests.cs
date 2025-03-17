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
    [TestClass]
    public class InputHelperTests
    {
        public InputHelper inputHelper = new InputHelper();
        [TestMethod]
        public void DisplayMenu_ShouldDisplayCorrectMenu()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                inputHelper.DisplayMenu();
                var result = sw.ToString().Trim();
                Assert.IsTrue(result.Contains("Welcome to Semantic Similarity Analysis!"));
                Assert.IsTrue(result.Contains("1. Word or Phrase Level Comparison"));
                Assert.IsTrue(result.Contains("2. Document Level Comparison"));
                Assert.IsTrue(result.Contains("3. Exit"));
            }
        }

        [TestMethod]
        public void GetUserChoice_ValidInput()
        {
            // Test all valid inputs (1, 2, 3)
            for (int i = 1; i <= 3; i++)
            {
                using (var sr = new StringReader(i.ToString()))
                {
                    Console.SetIn(sr);
                    int choice = inputHelper.GetUserChoice();
                    Assert.AreEqual(i, choice);
                }
            }
        }

        [TestMethod]
        public void GetUserChoice_InvalidInputThenValidInput()
        {
            using (var sr = new StringReader("5\n3"))
            {
                Console.SetIn(sr);
                int choice = inputHelper.GetUserChoice();
                Assert.AreEqual(3, choice);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileContents_InvalidFolderPath()
        {
            inputHelper.GetFileContents("invalid/path");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileContents_NoValidFiles()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);
            inputHelper.GetFileContents(tempFolder);
            Directory.Delete(tempFolder);
        }

        [TestMethod]
        public void GetFileContents_ValidFiles()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);
            string filePath = Path.Combine(tempFolder, "test.txt");
            File.WriteAllText(filePath, "Hello, World!");

            var result = inputHelper.GetFileContents(tempFolder);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Hello, World!", result[0]);

            Directory.Delete(tempFolder, true);
        }

        [Fact]
        public void GetUserInputs_EmptySourceAndReferenceInputs_ReturnsEmptyLists()
        {
            // Arrange
            var input = new string[] { "done", "done" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputHelper.GetUserInputs();

            // Assert
            XAssert.Empty(sourceContents);
            XAssert.Empty(refContents);
        }

        [Fact]
        public void GetUserInputs_SourceInputOnly_ReturnsSourceList()
        {
            // Arrange
            var input = new string[] { "source1", "source2", "done", "done" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputHelper.GetUserInputs();

            // Assert
            XAssert.Equal(new List<string> { "source1", "source2" }, sourceContents);
            XAssert.Empty(refContents);
        }

        [Fact]
        public void GetUserInputs_ReferenceInputOnly_ReturnsReferenceList()
        {
            // Arrange
            var input = new string[] { "done", "ref1", "ref2", "done" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputHelper.GetUserInputs();

            // Assert
            XAssert.Empty(sourceContents);
            XAssert.Equal(new List<string> { "ref1", "ref2" }, refContents);
        }

        [Fact]
        public void GetUserInputs_BothSourceAndReferenceInputs_ReturnsBothLists()
        {
            // Arrange
            var input = new string[] { "source1", "source2", "done", "ref1", "ref2", "done" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputHelper.GetUserInputs();

            // Assert
            XAssert.Equal(new List<string> { "source1", "source2" }, sourceContents);
            XAssert.Equal(new List<string> { "ref1", "ref2" }, refContents);
        }

        [Fact]
        public void GetUserInputs_MixedCaseDone_StopsReadingInput()
        {
            // Arrange
            var input = new string[] { "source1", "DoNe", "ref1", "dOnE" };
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act
            var (sourceContents, refContents) = inputHelper.GetUserInputs();

            // Assert
            XAssert.Equal(new List<string> { "source1" }, sourceContents);
            XAssert.Equal(new List<string> { "ref1" }, refContents);
        }

        [Fact]
        public void GetUserInputs_NoDoneInput_ThrowsException()
        {
            // Arrange
            var input = new string[] { "source1", "source2" }; // No "done" input
            var consoleInput = new TestConsoleInput(input);
            Console.SetIn(consoleInput);

            // Act & Assert
            XAssert.Throws<InvalidOperationException>(() => inputHelper.GetUserInputs());
        }
    }

    // Helper class to simulate console input
    public class TestConsoleInput : System.IO.TextReader
    {
        private readonly Queue<string> _inputQueue;

        public TestConsoleInput(IEnumerable<string> inputs)
        {
            _inputQueue = new Queue<string>(inputs);
        }

        public override string ReadLine()
        {
            if (_inputQueue.Count > 0)
            {
                return _inputQueue.Dequeue();
            }
            throw new InvalidOperationException("No more input available.");
        }

        public override void Close()
        {
            _inputQueue.Clear();
        }
    }
}
