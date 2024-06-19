/* Class SVD in LinearAlgebra namespace implements the one-sided Jacobi algorithm
for Singular Value Decomposition. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace LinearAlgebra
{
    public class SVD
    {
        // Fields.
        public Matrix U, V;
        public Vector S;
        public int inputNumRows;
        public int inputNumCols;
        public double absoluteError = 1e-14, relativeError = 1e-14;

        // Constructor.
        public SVD(Matrix A)
        {
            // Save relevant properties of A.
            inputNumRows = A.NumRows;
            inputNumCols = A.NumCols;
            
            // Copy A into U and perform Jacobi rotations inplace. To get U in the end, columns will need to be normalized. This is done after convergence is reached.
            U = A.Copy();

            // Initialize V as the identity and update during each Jacobi rotation.
            V = Matrix.Identity(inputNumCols);

            // Initialize S as a vector to save memory.
            S = new Vector(inputNumCols);

            // Do Jacobi rotations until convergence or until number of sweeps exceeds set limit, indicating that method wont converge.
            bool allColumnsOrthogonal = false;
            int numOfSweeps = 0;
            while (!allColumnsOrthogonal && numOfSweeps < 100000)
            {
                // Flip 'allColumnsOrthogonal' during sweep if non-orthogonal columns are found.
                allColumnsOrthogonal = true;

                // Do a sweep of Jacobi rotations.
                for (int p = 0; p < inputNumCols; p++)
                {
                    for (int q = p + 1; q < inputNumCols; q++)
                    {
                        // Get desired columns of matrix A' = AJ resulting from all Jacobi rotations up until now.
                        Vector aPrimeP = U[p];
                        Vector aPrimeQ = U[q];

                        // If corresponding entries are already orthogonal, go on to next index.
                        double aPrimePQ = Vector.InnerProduct(aPrimeP, aPrimeQ);
                        if (Matrix.Approx(aPrimePQ, 0, absoluteError, relativeError))
                        {
                            continue;
                        }
                        else
                        {
                            allColumnsOrthogonal = false;
                        }

                        // Compute angle theta in J(p, q, theta) making A[p] and A[q] orthogonal.
                        double aPrimePP = Vector.InnerProduct(aPrimeP, aPrimeP);
                        double aPrimeQQ = Vector.InnerProduct(aPrimeQ, aPrimeQ);
                        double theta = 0.5 * Atan2(2 * aPrimePQ, aPrimeQQ - aPrimePP);

                        // Do Jacobi rotation inplace on both A' and V.
                        double s = Sin(theta), c = Cos(theta);
                        U[p] = c * aPrimeP - s * aPrimeQ;
                        U[q] = s * aPrimeP + c * aPrimeQ;
                        for (int i = 0; i < inputNumCols; i++)
                        {
                            double Vip = V[i, p], Viq = V[i, q];
                            V[i, p] = c * Vip - s * Viq;
                            V[i, q] = s * Vip + c * Viq;
                        }
                    }
                }

                // Increase number of sweeps performed.
                numOfSweeps += 1;
            }

            // Compute U and S and save them.
            for (int i = 0; i < inputNumCols; i++)
            {
                Vector ui = U[i];
                double si = Vector.Norm(ui);
                if (Matrix.Approx(si, 0, absoluteError, relativeError))
                {
                    U[i] = ui.Apply(x => 0);
                    S[i] = si;
                }
                else
                {
                    U[i] = ui/si;
                    S[i] = si;
                }
            }
        }

        // Method computes pseudo-inverse of input matrix and returns it.
        public Matrix PseudoInverse()
        {
            // Initialize memory for pseudo-inverse B (of size nxm) of initial matrix A of size (mxn).
            Matrix B = new Matrix(inputNumCols, inputNumRows);

            // Compute pseudo-inverse as VS^-U^T. Call C := VS^- for simplicity.
            Matrix C = V.Copy();
            for (int i = 0; i < inputNumCols; i++)
            {
                double si = S[i];
                if (Matrix.Approx(si, 0, absoluteError, relativeError))
                {
                    C[i] = C[i] * 0;
                }
                else
                {
                    C[i] = C[i] / S[i];
                }
            }
            B = C * U.T();

            // Return result.
            return B;
        }

        // Method solves Least-Squares problem Ax = b, if system is overdetermined, using pseudo-inverse of A.
        public Vector LeastSquaresSVD(Vector b)
        {
            // Check whether system is overdetermined, otherwise throw an error.
            if (inputNumRows < inputNumCols)
            {
                throw new ArgumentException("For least squares method to work, system must be overdetermined.", $"Size of input matrix ({inputNumRows}, {inputNumCols})");
            }
            
            // If A is a tall mxn matrix, then b should be a vector of length m. Throw error if this is not the case.
            if (b.Length != inputNumRows)
            {
                throw new ArgumentException("Wrong size of input vector. Should have length equal to number of rows of input matrix.", $"Length of input vector {b.Length}");
            }

            // Compute and return least squares solution.
            return PseudoInverse() * b;
        }

        // Method returns lower rank approximation of input matrix, assuming r <= Rank(A).
        public Matrix LowerRankApprox(int r)
        {
            // Check that r <= Rank(A), otherwise throw an exception.
            if (r > GetRank())
            {
                throw new ArgumentException("The chosen rank r of the approximation should be less than the rank of the input matrix.", $"Input r = {r}, rank of input matrix = {GetRank()}");
            }

            // Initialize memory for rank-r approximation matrix.
            Matrix lowerRankApprox = Matrix.Zero(inputNumRows, inputNumCols);

            // Compute rank-r approximation.
            double previousLargestSV = 2*Vector.Norm(S); // Largest possible singular value is || S ||.
            for (int l = 0; l < r; l++)
            {
                int currentLargestSVIndex = 0;
                double currentLargestSV = 0;
                for (int i = 0; i < inputNumCols; i++)
                {
                    double si = S[i];
                    if (si > currentLargestSV && si < previousLargestSV)
                    {
                        currentLargestSV = si;
                        currentLargestSVIndex = i;
                    }
                }
                previousLargestSV = currentLargestSV;
                lowerRankApprox += currentLargestSV * Vector.OuterProduct(U[currentLargestSVIndex], V[currentLargestSVIndex]);
            }

            // Return approximation.
            return lowerRankApprox;
        }

        // Method returns rank of input matrix.
        public int GetRank()
        {
            // Initialize variable.
            int rank = 0;

            // Find number of non-zero singular values.
            for (int i = 0; i < S.Length; i++)
            {
                if (!Matrix.Approx(S[i], 0))
                {
                    rank += 1;
                }
            }

            // Return result.
            return rank;
        }
    }
}