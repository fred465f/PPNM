/* Program tests functionality of EVD class in LinearAlgebra namespace */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
    {
        static void Main()
        {
            var rnd = new System.Random(1);
            Matrix A = new Matrix(100, 100);
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    A[i, j] = rnd.NextDouble();
                }
            }
            EVD evd = new EVD(A);
        }
    }