using System;
using static System.Console;
using static System.Math;

public static class main
{
	public static void Main()
	{
		int size = 2;
		Matrix unity = new Matrix(size, size);
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				if (i == j)
				{
					unity[i, j] = 1.0;
				}
				else
				{
					unity[i, j] = 0.0;
				}
			}
		}
		unity.PrintMatrix();
        WriteLine("");

        Matrix hadamard = new Matrix(2, 2);
		hadamard.DataFromString("0,1\n1,0");
		hadamard.PrintMatrix();
		WriteLine("");

		Matrix sum = unity + hadamard;
		sum.PrintMatrix();
		WriteLine("");

		Matrix diff = unity - hadamard;
		diff.PrintMatrix();
		WriteLine("");

		Matrix negUnity = -unity;
		negUnity.PrintMatrix();
		WriteLine("");

		Matrix negNegUnity = (-1) * negUnity;
		negNegUnity.PrintMatrix();
		WriteLine($"Claim -(-I) = I: {Matrix.Approx(unity, negNegUnity)}");
		WriteLine("");

		Matrix hadamardSquared = hadamard * hadamard;
		hadamardSquared.PrintMatrix();
		WriteLine("");

		Matrix halfUnity = unity / 2;
		halfUnity.PrintMatrix();
		WriteLine("");
	}
}
