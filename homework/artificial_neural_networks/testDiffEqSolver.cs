/* Program tests implementation of second order ODE solver using ANN's on

y''(x) + w^2y(x) = 0.

which is the well known diff. eq. of a harmonic oscillator. The chosen interval is [0,10],
and initial data c = 0, y(0) = 5 and y'(0) = 0. Should result in cosine solution
with amplitude equal to 5. The initial data will be weighted equally so:

α = β = 1. */

using System;
using Calculus;
using LinearAlgebra;
using MachineLearning;
using static System.Math;
using static System.Console;

public class Program
{
    public static void Main()
    {
        // Variables.
        int numOfHiddenNeurons = 20;
        int numOfPoints = 1000;
        double a = 0;
        double b = 5;
        double c = a;
        double yc = 0;
        double ycPrime = 0;

        // Setting up diff. eq.
        Func<Func<double, double>, Func<double, double>, Func<double, double>, double, double> diffEq = (y, yPrime, yDoublePrime, x) => y(x);

        // Solve diff. eq.
        ODESolver solver = new ODESolver(diffEq, a, b, c, yc, ycPrime, numOfHiddenNeurons);
        solver.Train(1, 1);

        // Save predictions for later plotting.
        WriteLine($"# x, y(x)");
        for (int i = 0; i < numOfPoints; i++)
        {
            double x = a + i*(b - a)/numOfPoints;
            WriteLine($"{x},{solver.Predict(x)}");
        }
    }
}