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
    public class FileProcessor
    {
        public static List<string> LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File not found - {filePath}");
                return new List<string>();
            }

            return new List<string>(File.ReadAllLines(filePath));
        }
    }

}
