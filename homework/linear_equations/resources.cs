/* Program tests resource usage of QR-decomp. class in LinearAlgebra namespace. */

using System;
using LinearAlgebra;
using System.Diagnostics;
using static System.Console;
using static System.Math;

class Program
{
	static void Main(string[] args)
	{
		// Variables.
        int size = 100;

		// Process command line input arguments.
		foreach (var arg in args)
		{
            var words = arg.Split(":");
            if (words[0] == "-size")
            {
			    size = int.Parse(words[1]);
            }
		}

		// Create and start clock.
        Stopwatch watch = new Stopwatch();
        watch.Start();

		// Do QR-decomposition.
		DoQRDecomp(size);

		// Stop timer and output result to main output stream.
        watch.Stop();
        double elapsedMilliseconds = (double)watch.ElapsedMilliseconds;
        double elapsedSeconds = elapsedMilliseconds / 1000.0;
        WriteLine($"{size} {elapsedSeconds}");
	}

	// Method to do QR-decomp on random input size matrix.
	public static void DoQRDecomp(int m)
	{
		// To generate pseudo-random numbers.
		var rnd = new Random(1);

		// Generate random tall matrix of given input size.
		Matrix a = new Matrix(m);
		for (int i = 0; i < m; i++)
		{
			for (int j = 0; j < m; j++)
			{
				a[i, j] = rnd.NextDouble();
			}
		}

		// Create instance of QR-decomp class.
		QRGS qrgs = new QRGS(a);
	}
}