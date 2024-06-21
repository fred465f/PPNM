/* Program tests resources used by the one-sided Jacobi algorithm for Singular
Value Decomposition, by testing runtime on random square matrices of increasing size.
Results are saved in Out.resourcesSVD.data.txt. */

using System;
using LinearAlgebra;
using System.Diagnostics;
using static System.Math;
using static System.Console;

public class Program
{
    public static void Main(string[] args)
    {
        // Variables.
        int n = 0;
        int seed = 1;
        var rnd = new Random(seed);

        // Process command line input arguments.
        foreach (var arg in args)
        {
            var words = arg.Split(":");
            if (words[0] == "-size")
            {
                n = int.Parse(words[1]);
            }
        }

        // Construct random nxn square matrix.
        Matrix A = new Matrix(n);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                A[i, j] = rnd.NextDouble();
            }
        }

        // Create and start clock.
        Stopwatch watch = new Stopwatch();
        watch.Start();

        // Compute SVD.
        SVD svd = new SVD(A);

        // Stop timer and output result to main output stream.
        watch.Stop();
        double elapsedMilliseconds = (double)watch.ElapsedMilliseconds;
        double elapsedSeconds = elapsedMilliseconds / 1000.0;
        WriteLine($"{n} {elapsedSeconds}");
    }
}