using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class OutputHelper
{
    // Method to write semantic similarity results to a CSV file
    public static void WriteToCsv(string filePath, List<SemanticSimilarityResult> results)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        // Write the header
        csv.WriteHeader<SemanticSimilarityResult>();
        csv.NextRecord();

        // Write the results
        csv.WriteRecords(results);
    }
}

// Class to represent the semantic similarity result
public class SemanticSimilarityResult
{
    public string Sentence1 { get; set; }
    public string Sentence2 { get; set; }
    public double SimilarityScore { get; set; }
}