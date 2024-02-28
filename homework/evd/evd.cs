/* Class EVD in namespace LinearAlgebra contains implementation of eigenvalue
decomposition of real symmetric matrices using the Jacobi eigenvalue algorithm,
with cyclic sweeps. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace LinearAlgebra
{
    public class EVD
    {
        // ----- Fields -----
        public Vector w;
        public Matrix V;
        public double absoluteError = 1e-12, relativeError = 1e-12;

        // ----- Constructor -----
        public EVD(Matrix M)
        {
            if (M.NumRows != M.NumCols || !Matrix.Approx(M, M.T()))
            {
                throw new ArgumentException("Input matrix should both be square and symmetric for Jacobi eigenvalue algorithm to work");
            }
            {
                Matrix A = M.Copy();
                V = Matrix.Identity(M.NumRows);
                w = new Vector(M.NumRows);
                // Run Jacobi rotations on A and update V.
                // Copy diagonal elements into w.
                bool changed;
                do 
                {
                    changed = false;
                    for (int p = 0; p < A.NumRows - 1; p++)
                    {
                        for (int q = p + 1; q < A.NumRows; q++)
                        {
                            double Apq = A[p, q], App = A[p, p], Aqq = A[q, q];
                            double theta = 0.5 * Atan2(2 * Apq, Aqq - App);
                            double s = Sin(theta), c = Cos(theta);
                            double updatedApp = c * c * App - 2 * s * c * Apq + s * s * Aqq;
                            double updatedAqq = s * s * App + 2 * s * c * Apq + c * c * Aqq;
                            if (!Matrix.Approx(updatedApp, App, absoluteError, relativeError) || !Matrix.Approx(updatedAqq, Aqq, absoluteError, relativeError))
                            {
                                changed = true;
                                TimesJ(A, p, q, theta);
                                JTimes(A, p, q, -theta);
                                TimesJ(V, p, q, theta);
                            }
                        }
                    }
                    for (int i = 0; i < A.NumRows; i++)
                    {
                        w[i] = A[i, i];
                    }
                } while (changed);
            }
        }

        // ----- Jacobi rotation methods -----
        public static void TimesJ(Matrix A, int p, int q, double theta) 
        {
            double s = Sin(theta), c = Cos(theta);
            for (int i = 0; i < A.NumRows; i++)
            {
                double Aip = A[i, p], Aiq = A[i, q];
                A[i, p] = c * Aip - s * Aiq;
                A[i, q] = s * Aip + c * Aiq;
            }
        }
        public static void JTimes(Matrix A, int p, int q, double theta) 
        {
            double s = Sin(theta), c = Cos(theta);
            for (int i = 0; i < A.NumRows; i++)
            {
                double Api = A[p, i], Aqi = A[q, i];
                A[p, i] = c * Api + s * Aqi;
                A[q, i] = -s * Api + c * Aqi;
            }
        }
    }
}