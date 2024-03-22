/* Class RootFinder in Calculus namespace implements Newton's method with numerical 
Jacobian and back-tracking linesearch for solving the root finding problem. */

using System;
using Calculus;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace Calculus
{
    public static class RootFinder
    {
        // Newton's method.
        public static Vector Newton(Func<Vector, Vector> f, Vector x, double eps)
        {
            // Variables.
            int n = x.Length;
            int numSteps = 0;
            double lambda = 1.0;
            Vector dx = x.Apply(Abs) * Pow(2, -26);
            Vector xDisplaced = new Vector(n);
            Vector xDelta = new Vector(n);

            // Compute root.
            do
            {
                // Compute the Jacobi matrix.
                Matrix Jf = new Matrix(n);
                Vector fx = f(x);
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        xDisplaced = x.Copy();
                        xDisplaced[j] += dx[j];
                        Jf[i, j] = (f(xDisplaced)[i] - fx[i])/dx[j];
                    }
                }

                // Solve linear eq. Jf * xDelta = -f(x) for dx.
                QRGS qrgs = new QRGS(Jf);
                xDelta = qrgs.SolveLinearEq(-fx);

                // Find optimal lambda in x --> x + xDelta*lambda.
                lambda = 1.0;
                Vector xUpdated = x + xDelta * lambda;
                while (Vector.Norm(f(xUpdated)) > (1 - lambda/2.0) * Vector.Norm(fx) && lambda >= 1.0/1024.0)
                {
                    // Update lambda and xUpdated
                    lambda /= 2.0;
                    xUpdated = x + xDelta * lambda;
                }

                // Update x according to x --> x + xDelta*lambda.
                x = xUpdated;

                // Increase number of steps performed.
                numSteps += 1;
            } while (Vector.Norm(f(x)) > eps && numSteps < 10000 && Vector.Norm(xDelta) > Vector.Norm(dx));

            // Return result.
            return x;
        }
        public static double Newton(Func<double, double> f, double x, double eps) // Overload for one-dimensional case.
        {
            // Restate problem in terms of vectors with one entry.
            Func<Vector, Vector> fUpdated = delegate (Vector z) {return new Vector($"{f(z[0])}");};
            Vector xUpdated = new Vector($"{x}");

            // Call multi-dimensional Newton method.
            Vector x0 = Newton(fUpdated, xUpdated, eps);

            // Return result.
            return x0[0];
        }
    }
}