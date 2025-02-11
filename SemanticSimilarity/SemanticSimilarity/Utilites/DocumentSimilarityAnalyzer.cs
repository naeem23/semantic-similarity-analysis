using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SemanticSimilarity
{
    public class DocumentSimilarityAnalyzer
    {
        private readonly DocumentProcessor _documentProcessor;

        public DocumentSimilarityAnalyzer(string apiKey)
        {
            _documentProcessor = new DocumentProcessor(apiKey);
        }

        // Method to calculate similarity between two documents
        public async Task<double> CalculateDocumentSimilarityAsync(string doc1Path, string doc2Path)
        {
            string doc1Content = _documentProcessor.LoadDocument(doc1Path);
            string doc2Content = _documentProcessor.LoadDocument(doc2Path);

            string processedDoc1 = _documentProcessor.PreprocessText(doc1Content);
            string processedDoc2 = _documentProcessor.PreprocessText(doc2Content);

            var embedding1 = await _documentProcessor.GetEmbeddingAsync(processedDoc1);
            var embedding2 = await _documentProcessor.GetEmbeddingAsync(processedDoc2);

            return _documentProcessor.CalculateCosineSimilarity(embedding1, embedding2);
        }

        // Method to compare a document with a list of documents
        public async Task<Dictionary<string, double>> CompareDocumentWithReferenceAsync(string referenceDocPath, params string[] docPaths)
        {
            var referenceContent = _documentProcessor.LoadDocument(referenceDocPath);
            var processedReference = _documentProcessor.PreprocessText(referenceContent);
            var referenceEmbedding = await _documentProcessor.GetEmbeddingAsync(processedReference);

            var results = new Dictionary<string, double>();

            foreach (var docPath in docPaths)
            {
                var docContent = _documentProcessor.LoadDocument(docPath);
                var processedDoc = _documentProcessor.PreprocessText(docContent);
                var embedding = await _documentProcessor.GetEmbeddingAsync(processedDoc);

                double similarity = _documentProcessor.CalculateCosineSimilarity(referenceEmbedding, embedding);
                results[docPath] = similarity;
            }

            return results;
        }
    }
}