using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using System;
using System.Linq;

namespace SemanticSimilarityAPI.Controllers
{
    [Route("api/similarity")]
    [ApiController]
    public class SimilarityController : ControllerBase
    {
        private static readonly MLContext mlContext = new MLContext();

        [HttpPost]
        public IActionResult ComputeSimilarity([FromBody] SimilarityRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Text1) || string.IsNullOrWhiteSpace(request.Text2))
                return BadRequest("Input texts cannot be empty.");

            float similarityScore = ComputeTextSimilarity(request.Text1, request.Text2);
            return Ok(new { similarityScore });
        }
        private static float ComputeTextSimilarity(string text1, string text2)
        {
            var data = new[] { new TextData { Text = text1 }, new TextData { Text = text2 } };
            var dataView = mlContext.Data.LoadFromEnumerable(data);

            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(TextData.Text));
            var model = pipeline.Fit(dataView);
            var transformedData = model.Transform(dataView);
            var features = mlContext.Data.CreateEnumerable<TransformedTextData>(transformedData, reuseRowObject: false).ToArray();
            return ComputeCosineSimilarity(features[0].Features, features[1].Features);
        }
        private static float ComputeCosineSimilarity(float[] vector1, float[] vector2)
        {
            float dotProduct = vector1.Zip(vector2, (a, b) => a * b).Sum();
            float magnitude1 = (float)Math.Sqrt(vector1.Sum(x => x * x));
            float magnitude2 = (float)Math.Sqrt(vector2.Sum(x => x * x));

