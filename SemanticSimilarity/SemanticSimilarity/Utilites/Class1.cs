using System;
using System.Collections.Generic;
using System.Linq;

public static class TFIDFHelper
{
    public static Dictionary<string, double> ComputeTF(string text)
    {
        var words = text.Split(' ');
        var wordCount = words.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());
        var totalWords = words.Length;
        return wordCount.ToDictionary(k => k.Key, v => (double)v.Value / totalWords);
    }

    public static Dictionary<string, double> ComputeIDF(List<string> documents)
    {
        var wordDocumentCount = new Dictionary<string, int>();
        int totalDocs = documents.Count;

        foreach (var doc in documents)
        {
            var words = doc.Split(' ').Distinct();
            foreach (var word in words)
            {
                if (wordDocumentCount.ContainsKey(word))
                    wordDocumentCount[word]++;
                else
                    wordDocumentCount[word] = 1;
            }
        }

        return wordDocumentCount.ToDictionary(k => k.Key, v => Math.Log((double)totalDocs / (1 + v.Value)));
    }

    public static double ComputeTFIDFSimilarity(string text1, string text2, List<string> corpus)
    {
        var tf1 = ComputeTF(text1);
        var tf2 = ComputeTF(text2);
        var idf = ComputeIDF(corpus);

        double score = 0;
        foreach (var word in tf1.Keys.Intersect(tf2.Keys))
        {
            score += (tf1[word] * idf[word]) * (tf2[word] * idf[word]);
        }

        return score;
    }
}
