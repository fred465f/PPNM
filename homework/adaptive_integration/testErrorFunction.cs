/* Program tests adaptive integration methods implemented in AdaptiveIntegrator class
on on the integral representation of the error function. */

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
        double acc = 0.001;
        double eps = 0.001;

        // Compute error function in range [-3, 3] and output results to standard output stream.
        WriteLine("#x,errorFunction");
        double[] xRange = new double[] {-3, 3};
        double dx = 0.01;
        int numberXValues = (int)((xRange[1] - xRange[0])/dx);
        for (int i = 0; i < numberXValues; i++)
        {
            double x = xRange[0] + i * dx;
            WriteLine($"{x},{ErrorFunction(x, acc, eps)}");
        }
    }

    // Static method implements error function using its integral representation.
    public static double ErrorFunction(double z, double acc, double eps)
    {
        // Compute error function.
        if (z < 0)
        {
            return -ErrorFunction(-z, acc, eps);
        }
        else if (z <= 1)
        {
            (double integral, double error, int numSteps) = AdaptiveIntegrator.Integrate(delegate(double x) {return Exp(-x*x);}, 0, z, acc, eps);
            return 2/Sqrt(PI) * integral;
        }
        else
        {
            (double integral, double error, int numSteps) = AdaptiveIntegrator.Integrate(delegate(double x) {return Exp(-Pow(z+(1-x)/x, 2))/(x*x);}, 0, 1, acc, eps);
            return 1 - 2/Sqrt(PI) * integral;
        }
    }
}