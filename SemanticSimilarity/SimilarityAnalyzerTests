using System;
using Xunit;
using FluentAssertions;
using SemanticSimilarity;

namespace SemanticSimilarity.Tests
{
    public class SimilarityAnalyzerTests
    {
        [Fact]
        public void ComputeSimilarity_SimilarTexts_ReturnsHighScore()
        {
            string text1 = "Angela Merkel is the former Chancellor of Germany.";
            string text2 = "The German government was led by Angela Merkel.";

            float similarity = SimilarityAnalyzer.ComputeSimilarity(text1, text2);

            similarity.Should().BeGreaterThan(0.7f);
        }

        
