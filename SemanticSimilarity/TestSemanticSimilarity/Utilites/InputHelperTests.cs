using SemanticSimilarity.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSemanticSimilarity.Utilites
{
    [TestClass]
    public class InputHelperTests
    {
        [TestMethod]
        public void DisplayMenu_ShouldDisplayCorrectMenu()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                InputHelper.DisplayMenu();
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
                    int choice = InputHelper.GetUserChoice();
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
                int choice = InputHelper.GetUserChoice();
                Assert.AreEqual(3, choice);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileContents_InvalidFolderPath()
        {
            InputHelper.GetFileContents("invalid/path");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetFileContents_NoValidFiles()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);
            InputHelper.GetFileContents(tempFolder);
            Directory.Delete(tempFolder);
        }

        [TestMethod]
        public void GetFileContents_ValidFiles()
        {
            string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFolder);
            string filePath = Path.Combine(tempFolder, "test.txt");
            File.WriteAllText(filePath, "Hello, World!");

            var result = InputHelper.GetFileContents(tempFolder);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Hello, World!", result[0]);

            Directory.Delete(tempFolder, true);
        }

        //pest here
    }
}
