/* Program tests resource usage of QR-decomp. class in LinearAlgebra namespace. */

using System;
using LinearAlgebra;
using static System.Console;
using static System.Math;

class Program
{
	static void Main(string[] args)
	{
		foreach (var arg in args)
		{
            var words = arg.Split(":");
            if (words[0] == "-size")
            {
			    int m = int.Parse(words[1]);
			    DoQRDecomp(m);
            }
		}
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