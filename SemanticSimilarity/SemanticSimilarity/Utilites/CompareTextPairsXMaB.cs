using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticSimilarity.Utilites
{
    internal class CompareTextPairsXMaB
    {

        static async Task CompareTextPairs(DocumentProcessor processor, List<string> texts, string category, List<(string, string, double)> results)
        {
            Console.WriteLine($"\n==== {category} Semantic Similarity ====");

            for (int i = 0; i < texts.Count; i++)
            {
                for (int j = i + 1; j < texts.Count; j++)
                {
                }
}
