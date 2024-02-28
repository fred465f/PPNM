/* Program tests resources used in diagonalizing a random symmetric (n, n) matrix using
the Jacobi eigenvalue algorithm, implemented in the EVD class in the LinearAlgebra namespace. 
Test are performing in two cases: without optimization and with optimizations utilizing that
Jacobi rotations only needs to be performed in upper triangular part of input matrix due to symmetry. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

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

            // Diagonalize random symmetric (n, n) matrix.
            DiagonalizeRandomMatrix(size);
        }

        // Static method to check diagonalize random symmetric (n, n) matrix.
        static void DiagonalizeRandomMatrix(int n)
        {
            // Create random symmetric square matrix of size (n, n).
            var rnd = new System.Random(1);
            Matrix M = new Matrix(n);
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    double randomDouble = rnd.NextDouble();
                    M[i, j] = randomDouble;
                    M[j, i] = randomDouble;
                }
            }

            // Create instance of EVD class. Eigenvalue-decomposition occurs doing construction of instance.
            EVD evd = new EVD(M);
        }
    }