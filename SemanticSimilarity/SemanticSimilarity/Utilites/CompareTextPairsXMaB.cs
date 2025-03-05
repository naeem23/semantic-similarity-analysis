//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SemanticSimilarity.Utilites
//{
//    internal class CompareTextPairsXMaB
//    {

//        static async Task CompareTextPairs(DocumentProcessor processor, List<string> texts, string category, List<(string, string, double)> results)
//        {
//            Console.WriteLine($"\n==== {category} Semantic Similarity ===="); 

//            for (int i = 0; i < texts.Count; i++)
//            {
//                for (int j = i + 1; j < texts.Count; j++)
//                {
//                    string text1 = texts[i];
//                    string text2 = texts[j];

//                    List<double> vectorA = await processor.GetEmbeddingAsync(text1);
//                    List<double> vectorB = await processor.GetEmbeddingAsync(text2);
//                    double similarity = processor.CalculateCosineSimilarity(vectorA, vectorB);
//                    Console.WriteLine($"Comparing: \"{text1}\" ↔ \"{text2}\" → Similarity Score: {similarity:F4}");

//                    // ✅ Store the result in the results list here
//                    results.Add((text1, text2, similarity));
//                }

            
//        }
//    }
//}
