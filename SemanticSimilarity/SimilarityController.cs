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
