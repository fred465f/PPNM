using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace LinearAlgebra
{
    /* Class QRGS in solves linear equations, computes determinants and inverses 
    using the QR-decomp. of a matrix given doing construction of class instance. 
    QR-decomp. is computed using modified Gram-Schmidt orthogonalization. */
    public class QRGS
    {
        // Fields
        public Matrix Q, R; 
        public bool isSquare;
        public int inputNumRows;
        public int inputNumCols;

        /* Constructor - it computes QR-decomp. of input matrix doing
        creation of new instance of QRGS. */
        public QRGS(Matrix A)
        {
            if (A.NumRows < A.NumCols)
            {
                throw new ArgumentException("Input matrix should either be tall or square for algorithms to work", $"({A.NumRows}, {A.NumCols})");
            }
            else
            {
                Q = A.Copy();
                R = new Matrix(A.NumCols, A.NumCols);
                for (int i = 0; i < A.NumCols; i++)
                {
                    R[i, i] = Vector.Norm(Q[i]);
                    Q[i] /= R[i, i];
                    for (int j = i + 1; j < A.NumCols; j++)
                    {
                        R[i, j] = Vector.InnerProduct(Q[i], Q[j]);
                        Q[j] -= Q[i] * R[i, j];
                    }
                }
                if (A.NumRows == A.NumCols)
                {
                    isSquare = true;
                }
                else
                {
                    isSquare = false;
                }
                inputNumRows = A.NumRows;
                inputNumCols = A.NumCols;
            }
        }

        /* Method solves A * x = b using decomp. A = Q * R, by rewriting 
        R * x = Q^T * b and then using back substitution. In case where A is tall
        this gives you the least squares solution to the overdetermined system
        of linear equations. */
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
            if (!isSquare)
            {
                throw new ArgumentException("The determinant is only defined for square matrices", $"({inputNumRows}, {inputNumCols})");
            }
            else
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
        }

        // Method computes inverse of A. Throws an exception if det(A) = 0.
        public Matrix Inverse()
        {
            double determinant = this.Determinant();
            if (!isSquare || Matrix.Approx(determinant, 0))
            {
                throw new ArgumentException("To be invertible a matrix must both be a square matrix and have non-trivial determinant", $"Square = {isSquare}, Determinant = {determinant}");
            }
            else
            {
                Matrix B = new Matrix(inputNumRows, inputNumCols);
                for (int i = 0; i < inputNumCols; i++)
                {
                    Vector ei = new Vector(inputNumRows);
                    for (int j = 0; j < inputNumRows; j++)
                    {
                        if (i == j)
                        {
                            ei[j] = 1.0;
                        }
                        else
                        {
                            ei[j] = 0.0;
                        }
                    }
                    Vector bi = SolveLinearEq(ei);
                    B[i] = bi;
                }
                return B;
            }
        }

        // Method computes pseudo-inverse of tall matrix A.
        public Matrix PseudoInverse()
        {
            Matrix B = new Matrix(inputNumCols, inputNumRows);
            for (int i = 0; i < inputNumCols; i++)
            {
                Vector ei = new Vector(inputNumRows);
                for (int j = 0; j < inputNumRows; j++)
                {
                    if (i == j)
                    {
                        ei[j] = 1.0;
                    }
                    else
                    {
                        ei[j] = 0.0;
                    }
                }
                Vector bi = SolveLinearEq(ei);
                B[i] = bi;
            }
            return B;
        }
    }
}