using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticSimilarity.Utilites
{
    public class SimilarityResult
    {
        public string Document1 { get; set; }
        public string Document2 { get; set; }
        public double SimilarityScore { get; set; }
    }
}
