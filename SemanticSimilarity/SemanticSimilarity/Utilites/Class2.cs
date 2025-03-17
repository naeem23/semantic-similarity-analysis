using System;
using System.Collections.Generic;
using System.Linq;

public static class EuclideanSimilarity
{
    public static double ComputeEuclideanDistance(List<double> vector1, List<double> vector2)
    {
        if (vector1.Count != vector2.Count)
            throw new ArgumentException("Vectors must be of the same length");

        double sum = vector1.Zip(vector2, (v1, v2) => Math.Pow(v1 - v2, 2)).Sum();
        return Math.Sqrt(sum);
    }
}
