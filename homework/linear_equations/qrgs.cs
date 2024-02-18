/* Class QRGS in namespace LinearAlgebra solves linear equations,
computes determinants and inverses using the QR-decomp. of a matrix
given doing construction of class instance. QR-decomp. is computed
using modified Gram-Schmidt orthogonalization. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace LinearAlgebra
{
    public class QRGS
    {
        // Fields
        public Matrix Q, R; 

        /* Constructor - it computes QR-decomp. of input matrix doing
        creation of new instance of QRGS. */
        public QRGS(Matrix a)
        {
            if (a.NumRows < a.NumCols)
            {
                throw new ArgumentException("Input matrix should either be tall or square for algorithms to work", $"({a.NumRows}, {a.NumCols})");
            }
            else
            {
                Q = a.Copy();
                R = new Matrix(a.NumCols, a.NumCols);
                for (int i = 0; i < a.NumCols; i++)
                {
                    R[i, i] = Vector.Norm(Q[i]);
                    Q[i] /= R[i, i];
                    for (int j = i + 1; j < a.NumCols; j++)
                    {
                        R[i, j] = Vector.InnerProduct(Q[i], Q[j]);
                        Q[j] -= Q[i] * R[i, j];
                    }
                }
            }
        }

        /* Method solves A * x = b using decomp. A = Q * R, by rewriting 
        R * x = Q^T * b and then using backsubstitution. */
        public Vector SolveLinearEq(Vector b)
        {
            Vector x = Q.T() * b;
            for (int i = x.Length - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < x.Length; j++)
                {
                    sum += R[i, j] * x[j];
                }
                x[i] = (x[i] - sum) / R[i, i];
            }
            return x;
        }

        /* Method computes det. of A using decomp A = Q * R as product of 
        diagonal entries of R. Throws an exception if A is not a square matrix. */
        public double Determinant()
        {
            double determinant = 1;
            for (int i = 0; i < R.NumRows; i++)
            {
                for (int j = 0; j < R.NumCols; j++)
                {
                    if (i == j)
                    {
                        determinant *= R[i, j];
                    }
                }
            }
            return determinant;
        }

        // Method computes inverse of A. Throws an exception if det(A) = 0.
        public Matrix Inverse()
        {
            return this.Q;
        }
    }
}