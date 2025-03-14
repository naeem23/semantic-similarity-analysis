using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;

namespace SemanticSimilarity.Utilites
{
   public class CustomCsvWriter
    {
        public static void SaveResultsToCsv(string fileName, List<(string, string, double)> results)
        //List<(string, string, double)> results = new List<(string, string, double)>(); // for me Note

        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine("Text1,Text2,SimilarityScore");

                    foreach (var (text1, text2, similarity) in results)
                    {
                        writer.WriteLine($"\"{text1}\",\"{text2}\",{similarity:F4}");
                    }
                }

                Console.WriteLine($"\n Congratulation!!! Results successfully saved to {fileName}"); //save file 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing CSV file: {ex.Message}");
            }
        }
    }

}
