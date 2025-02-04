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
