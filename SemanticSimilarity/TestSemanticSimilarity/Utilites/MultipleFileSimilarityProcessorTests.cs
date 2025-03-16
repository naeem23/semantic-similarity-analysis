using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
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
    public class MultipleFileSimilarityProcessorTests
    {
        private readonly MultipleFileSimilarityProcessor _processor;

        public MultipleFileSimilarityProcessorTests()
        {
            _processor = new MultipleFileSimilarityProcessor();
        }

        [Fact]
        public void ReadTextFileContent()
        {
            // Arrange
            string filePath = "testfile.txt";
            string expectedContent = "This is a test text file.";
            File.WriteAllText(filePath, expectedContent);

            // Act
            string actualContent = _processor.ReadFileText(filePath);

            // Assert
            Assert.Equal(expectedContent, actualContent);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void ReadPDFFileContent()
        {
            // Arrange
            string filePath = "testfile.pdf";
            string expectedContent = "This is a test PDF file.";

            // Create a simple PDF file using iText 7
            using (PdfWriter writer = new PdfWriter(filePath))
            {
                using (PdfDocument pdfDoc = new PdfDocument(writer))
                {
                    Document document = new Document(pdfDoc);
                    document.Add(new Paragraph(expectedContent));
                    document.Close();
                }
            }

            // Act
            string actualContent = _processor.ReadFileText(filePath);

            // Assert
            Assert.Equal(expectedContent, actualContent.Trim());

            // Cleanup
            File.Delete(filePath);
        }

        //[Fact]
        //public void ReadDocFileContent()
        //{
        //    // Arrange
        //    string filePath = "testfile.docx";
        //    string expectedContent = "This is a test DOCX file.";
        //    CreateDocxFile(filePath, expectedContent);

        //    // Act
        //    string actualContent = _processor.ReadFileText(filePath);

        //    // Assert
        //    Assert.Equal(expectedContent, actualContent);

        //    // Cleanup
        //    File.Delete(filePath);
        //}

        [Fact]
        public void ReadFileText_UnsupportedFormat()
        {
            // Arrange
            string filePath = "testfile.unsupported";
            File.WriteAllText(filePath, "This is an unsupported file.");

            // Act & Assert
            _ = Assert.Throws<NotSupportedException>(() => _processor.ReadFileText(filePath));

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public void ReadFileText_ShouldThrowExceptionForNonExistentFile()
        {
            // Arrange
            string filePath = "nonexistentfile.txt";

            // Act & Assert
            _ = Assert.Throws<FileNotFoundException>(() => _processor.ReadFileText(filePath));
        }

        private void CreatePdfFile(string filePath, string content)
        {
            using (PdfWriter writer = new PdfWriter(filePath))
            using (PdfDocument pdfDoc = new PdfDocument(writer))
            {
                PdfPage page = pdfDoc.AddNewPage();
                var canvas = new PdfCanvas(page);
                canvas.BeginText();
                canvas.SetFontAndSize(PdfFontFactory.CreateFont(), 12);
                canvas.ShowText(content);
                canvas.EndText();
            }
        }

        private void CreateDocxFile(string filePath, string content)
        {
            // This is a placeholder for creating a DOCX file.
            // In a real scenario, you would use a library like DocX or OpenXML to create the file.
            File.WriteAllText(filePath, content);
        }
    }
}
