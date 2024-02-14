using System;
using static System.Console;
using static System.Math;

public static class main
{
	public static void Main()
	{
		int Size = 2;
		Matrix Unity = new Matrix(Size, Size);
		for (int i = 0; i < Size; i++)
		{
			for (int j = 0; j < Size; j++)
			{
				if (i == j)
				{
					Unity[i, j] = 1.0;
				}
				else
				{
					Unity[i, j] = 0.0;
				}
			}
		}
		Unity.PrintMatrix();
        WriteLine("");

        Matrix Hadamard = new Matrix(2, 2);
		Hadamard.DataFromString("0,1\n1,0");
		Hadamard.PrintMatrix();
		WriteLine("");

		Matrix Sum = Unity + Hadamard;
		Sum.PrintMatrix();
		WriteLine("");

		Matrix Diff = Unity - Hadamard;
		Diff.PrintMatrix();
		WriteLine("");

		Matrix NegUnity = -Unity;
		NegUnity.PrintMatrix();
		WriteLine("");

		Matrix NegNegUnity = (-1) * NegUnity;
		NegNegUnity.PrintMatrix();
		WriteLine("");

		Matrix HadamardSquared = Hadamard * Hadamard;
		HadamardSquared.PrintMatrix();
		WriteLine("");
	}
}
