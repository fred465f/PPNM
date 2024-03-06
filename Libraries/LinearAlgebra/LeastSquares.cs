using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace LinearAlgebra
{
    /* Class utilizes QR-decomposition to implement least squares method for fitting */
    public static class LeastSquares
    {
        public static (Vector coefficients, Matrix covarianceMatrix) Fit(Func<double, double>[] functions, Vector xValues, Vector yValues, Vector yErrors)
        {
            /* Problem is transformed into linear eq. A * c = b, the number of columns (m) in A
            is equal to the number of functions in linear combination, i.e functions.Length,
            and number of rows (n) equal to the number of data points, i.e xValues.Length. */
            int n = xValues.Length;
            int m = functions.Length;

            // Functions coefficients in least squares fit to be returned.
            Vector coefficients = new Vector(functions.Length);

            /* Covariance matrix to be returned.
            Using QR-decomposition we write A = Q * R with R square matrix of size (m, m).
            Now the covariance matrix is computed as (R^T * R)^(-1), so must have equal size to R. 
            Can equivalently be computed as A^(-1) * A^(-1)^T, with A^(-1) the pseudo-inverse of A. */
            Matrix covarianceMatrix = new Matrix(functions.Length);

            // Throw exception if y-errors are zero.
            for (int i = 0; i < n; i++)
            {
                if (Matrix.Approx(yErrors[i], 0))
                {
                    throw new ArgumentException("Uncertainties needs to be none-zero to avoid division by zero exception.", $"Zero uncertainty found at index {i}.");
                }
            }

            // Construct appropriate vector and matrix for solving least-squares problem.
            Matrix A = new Matrix(n, m);
            Vector b = new Vector(n);
            for (int i = 0; i < n; i++)
            {
                b[i] = yValues[i] / yErrors[i];
                for (int j = 0; j < m; j++)
                {
                    A[i, j] = functions[j](xValues[i]) / yErrors[i];
                }
            }

            // Do QR-decomposition.
            QRGS qrgs = new QRGS(A);

            // Solve linear equation A * c = b for coefficients c.
            coefficients = qrgs.SolveLinearEq(b);
            
            // Get covariance matrix.
            Matrix pseudoInverseA = qrgs.PseudoInverse();
            covarianceMatrix = pseudoInverseA * pseudoInverseA.T();

            // Return results.
            return (coefficients, covarianceMatrix);
        }
    }
}