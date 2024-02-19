/* Program tests elements of LinearAlgebra namespace. */

using System;
using LinearAlgebra;
using static System.Console;
using static System.Math;

class Program
{
	static void Main()
	{
		WriteLine("----- Check QR-decomp. -----\n");
		CheckQRDecomp(100, 80);

		WriteLine("\n\n----- Check linear eq. solver -----\n");
		CheckQRLinearEqSolver(100);

		WriteLine("\n\n----- Check inverse solver -----\n");
		CheckQRInverse(100);
	}

	// Method to check whether QR-decomp works for random modest size tall matrix.
	public static void CheckQRDecomp(int numRows, int numCols)
	{
		// To generate pseudo-random numbers.
		var rnd = new Random(1);

		// Generate random tall matrix of given input size.
		Matrix a = new Matrix(numRows, numCols);
		for (int i = 0; i < numRows; i++)
		{
			for (int j = 0; j < numCols; j++)
			{
				a[i, j] = rnd.NextDouble();
			}
		}

		// Create instance of QR-decomp class.
		QRGS qrgs = new QRGS(a);

		// Make output.
		if (Matrix.Approx(a, qrgs.Q * qrgs.R))
		{
			WriteLine("A = QR");
		}
		else
		{
			WriteLine("A != QR");
		}
		if (CheckUpperTriangular(qrgs.R))
		{
			WriteLine("R in QR-decomp. was indeed upper triangular.");
		}
		else
		{
			WriteLine("R in QR-decomp. was not upper triangular.");
		}
		if (CheckOrthogonal(qrgs.Q))
		{
			WriteLine("Q in QR-decomp. was indeed orthogonal.");
		}
		else
		{
			WriteLine("Q in QR-decomp. was not orthogonal.");
		}
	}

	// Method to check whether QR-decomp. class solves linear eq. correctly.
	public static void CheckQRLinearEqSolver(int m)
	{
		// To generate pseudo-random numbers.
		var rnd = new Random(1);

		// Generate random square matrix and vector.
		Matrix a = new Matrix(m);
		Vector v = new Vector(m);
		for (int i = 0; i < m; i++)
		{
			for (int j = 0; j < m; j++)
			{
				a[i, j] = rnd.NextDouble();
			}
			v[i] = rnd.NextDouble();
		}

		// Create instance of QR-decomp. class.
		QRGS qrgs = new QRGS(a);

		// Solve linear eq.
		Vector x = qrgs.SolveLinearEq(v);
		Write("A possible solution to Ax = b was found. ");

		// Make output.
		if (Vector.Approx(a * x, v))
		{
			WriteLine("Result was indeed a solution to linear eq.");
		}
		else
		{
			WriteLine("Result was not a solution to linear eq.");
		}
	}
	
	// Method to check whether QR-decomp. class computes inverse of random modest size square matrix correctly.
	public static void CheckQRInverse(int m)
	{
		// To generate pseudo-random numbers.
		var rnd = new Random(1);

		// Generate random square matrix.
		Matrix a = new Matrix(m);
		for (int i = 0; i < m; i++)
		{
			for (int j = 0; j < m; j++)
			{
				a[i, j] = rnd.NextDouble();
			}
		}

		// Create instance of QR-decomp. class.
		QRGS qrgs = new QRGS(a);

		// Compute inverse.
		Matrix b = qrgs.Inverse();
		Write("Possible inverse of matrix were found. ");

		// Make output.
		if (Matrix.Approx(a * b, Matrix.Identity(m)))
		{
			WriteLine("Result was indeed the inverse.");
		}
		else
		{
			WriteLine("Result was not the inverse.");
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
