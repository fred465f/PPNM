using System;
using static System.Console;
using static System.Math;

public static class main
{
	public static void Main()
	{
		int Size = 2;
		Matrix<double> Unity = new Matrix<double>(Size, Size);

		for (int i = 0; i < Size; i++)
		{
			for (int j = 0; j < Size; j++)
			{
				if (i == j)
				{
					Unity[i][j] = 1.0;
				}
				else
				{
					Unity[i][j] = 0.0;
				}
			}
		}

		for (int i = 0; i < Size; i++)
        {
        	for (int j = 0; j < Size; j++)
            {
                Write($"{Unity[i][j]} ");
            }
            WriteLine("");
        }
	}
}
