/* Program tests elements of LinearAlgebra namespace. */

using System;
using LinearAlgebra;
using static System.Console;
using static System.Math;

class Program
{
	static void Main()
	{
		// To generate pseudo-random numbers.
		var rnd = new Random(10);

		// Check that QR-decomp. works for random tall (m, n) matrix.
		int m = 200, n = 150;
		Matrix a = new Matrix(m, n);
		for (int i = 0; i < m; i++)
		{
			for (int j = 0; j < n; j++)
			{
				a[i, j] = rnd.NextDouble();
			}
		}
		QRGS aDecomp = new QRGS(a);
		if (Matrix.Approx(a, aDecomp.Q * aDecomp.R))
		{
			WriteLine("A = QR");
		}
		else
		{
			WriteLine("A != QR");
		}
		if (CheckUpperTriangular(aDecomp.R))
		{
			WriteLine("R in QR-decomp. was indeed upper triangular.");
		}
		else
		{
			WriteLine("R in QR-decomp. was not upper triangular.");
		}
		if (CheckOrthogonal(aDecomp.Q))
		{
			WriteLine("Q in QR-decomp. was indeed orthogonal.");
		}
		else
		{
			WriteLine("Q in QR-decomp. was not orthogonal.");
		}

		// Solve A * x = b using QR-decomp. A = Q * R for square matrix.
		a = new Matrix(m);
		Vector b = new Vector(m);
		for (int i = 0; i < m; i++)
		{
			for (int j = 0; j < m; j++)
			{
				a[i, j] = rnd.NextDouble();
			}
			b[i] = rnd.NextDouble();
		}
		aDecomp = new QRGS(a);
		Vector x = aDecomp.SolveLinearEq(b);
		if (Vector.Approx(a * x, b))
		{
			WriteLine("It is a solution.");
		}
		else
		{
			WriteLine("It is not a solution.");
		}
	}

	// Method checks whether input matrix is upper triangular.
	public static bool CheckUpperTriangular(Matrix a)
	{
		for (int i = 0; i < a.NumRows; i++)
		{
			for (int j = 0; j < a.NumCols; j++)
			{
				if (i > j && !Matrix.Approx(a[i, j], 0))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Methods checks whether input matrix is orthogonal.
	public static bool CheckOrthogonal(Matrix a)
	{
		Matrix product = a.T() * a;
		Matrix identity = Matrix.Identity(product.NumRows);
		if (Matrix.Approx(product, identity))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
