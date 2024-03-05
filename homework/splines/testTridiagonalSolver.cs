/* Program test method to solve tri-diagonal systems of linear equations. 
After functionality has been checked, the method will be implemented in 
the CubicSpline class. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main(string[] args)
    {
        // Create random tri-diagonal matrix as well as a random vector.
        var rnd = new Random(1);
        int size = 100;
        Matrix A = new Matrix(size, size);
        Vector b = new Vector(size);
        for (int i = 0; i < size; i++)
        {
            if (i == 0)
            {
                A[i, i] = rnd.NextDouble();
                A[i, i + 1] = rnd.NextDouble();
            }
            else if (i == size - 1)
            {
                A[i, i - 1] = rnd.NextDouble();
                A[i, i] = rnd.NextDouble();
            }
            else
            {
                A[i, i - 1] = rnd.NextDouble();
                A[i, i] = rnd.NextDouble();
                A[i, i + 1] = rnd.NextDouble();
            }
            b[i] = rnd.NextDouble();
        }

        // Solve tri-diagonal system.
        Vector solution = TriDiagonalSolver(A, b);

        // Test solution.
        if (Vector.Approx(A * solution, b))
        {
            WriteLine($"Method solved random ({size}, {size}) system of tri-diagonal equations.");
        }
        else
        {
            WriteLine($"Method failed to solve random ({size}, {size}) system of tri-diagonal equations.");
        }
    }

    // Method solves tri-diagonal system of linear equations.
    public static Vector TriDiagonalSolver(Matrix inputMatrix, Vector inputVector)
    {
        // Throw exception if matrix is not square.
        if (inputMatrix.NumRows != inputMatrix.NumCols)
        {
            throw new ArgumentException("Input matrix needs to be square", $"Input matrix size ({inputMatrix.NumRows}, {inputMatrix.NumCols})");
        }

        // Throw exception if matrix and vector are of incompatible sizes.
        int n = inputMatrix.NumRows;
        if (inputVector.Length != n)
        {
            throw new ArgumentException("Input vector and matrix needs to be of compatible sizes", $"Matrix size = ({n}, {n}), Vector size = {inputVector.Length}");
        }

        // Delete this and change "inputMatrix" => A and "inputVector" => b, if changes should be done in-place instead.
        Matrix A = inputMatrix.Copy();
        Vector b = inputVector.Copy();

        // Do first run of Gauss elimination.
        for (int i = 0; i < n - 1; i++)
        {
            double entry = A[i + 1, i];
            if (!Matrix.Approx(entry, 0.0))
            {
                A[i + 1, i] = 0;
                A[i + 1, i + 1] -= A[i, i + 1] * (entry / A[i, i]);
                b[i + 1] -= b[i] * (entry / A[i, i]);
            }
        }

        // Solve system using back-substitution.
        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += A[i, j] * b[j];
            }
            b[i] = (b[i] - sum) / A[i, i];
        }

        // Return solution to system.
        return b;
    }
}