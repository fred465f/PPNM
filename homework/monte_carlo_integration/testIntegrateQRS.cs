/* Program tests implementation of Quasi-Random sequence Monte-Carlo integrator in MonteCarloIntegrator class.
Estimated error is compared to actual error, and error scaling is compared to that of the Plain Monte-Carlo 
integrator. This is done by outputting data to standard output stream for later plotting. */

using System;
using Calculus;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main()
    {
        // Variables.
        int numOfNs = 1000;
        int dN = 50;
        int[] Ns = new int[numOfNs];
        for (int i = 1; i < numOfNs; i++)
        {
            Ns[i] = dN * i;
        }
        Vector a = new Vector("0\n0");
        Vector b = new Vector($"1\n{2*PI}");
        Func<Vector, double> f = delegate(Vector v) {return v[0];};

        /* Compute area of circle using Plain Monte-Carlo integrator for varying N and output,
        estimated error, actual error and N to the standard output stream. */
        WriteLine("#N,actualError,estimatedError");
        foreach (int N in Ns)
        {
            // Compute integral.
            (double estimatedAreaOfCircle, double estimatedError) = MonteCarloIntegrator.IntegrateQRS(f, a, b, N);

            // Compute actual error.
            double actualError = Abs(estimatedAreaOfCircle - PI);

            // Save results.
            WriteLine($"{N},{actualError},{estimatedError}");
        }
    }
}