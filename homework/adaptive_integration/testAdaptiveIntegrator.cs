/* Program tests adaptive integration methods implemented in AdaptiveIntegrator class
on several known integrals. */

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
        double acc = 0.0001;
        double eps = 0.0001;

        // Test functions appearing in part A of homework.
        /*
        (1) ∫_0^1 dx √(x) = 2/3,
        (2) ∫_0^1 dx 1/√(x) = 2,
        (3) ∫_0^1 dx 4√(1-x²) = π,
        (4) ∫_0^1 dx ln(x)/√(x) = -4.
        */
        WriteLine("----- Part A -----\n");
        (double firstIntegralA, double error1A) = AdaptiveIntegrator.Integrate(delegate(double x) {return Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(firstIntegralA - 2.0/3.0) < 0.01)
        {
            WriteLine($"First integral was computed successfully within specified accuracy of {acc}");
        }
        else
        {
            WriteLine($"First integral was not computed successfully within specified accuracy of {acc}");
        }
        (double secondIntegralA, double error2A) = AdaptiveIntegrator.Integrate(delegate(double x) {return 1/Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(secondIntegralA - 2.0) < 0.01)
        {
            WriteLine($"Second integral was computed successfully within specified accuracy of {acc}");
        }
        else
        {
            WriteLine($"Second integral was not computed successfully within specified accuracy of {acc}");
        }
        (double thirdIntegralA, double error3A) = AdaptiveIntegrator.Integrate(delegate(double x) {return 4*Sqrt(1 - x*x);}, 0, 1, acc, eps);
        if (Abs(thirdIntegralA - PI) < 0.01)
        {
            WriteLine($"Third integral was computed successfully within specified accuracy of {acc}");
        }
        else
        {
            WriteLine($"Third integral was not computed successfully within specified accuracy of {acc}");
        }
        (double fourthIntegralA, double error4A) = AdaptiveIntegrator.Integrate(delegate(double x) {return Log(x)/Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(fourthIntegralA + 4) < 0.01)
        {
            WriteLine($"Fourth integral was computed successfully within specified accuracy of {acc}");
        }
        else
        {
            WriteLine($"Fourth integral was not computed successfully within specified accuracy of {acc}");
        }

        /* Test open quadrature adaptive integration using Clenshaw-Curtis variable transformation
        on functions appearing in part B. */
        /*
        (1) ∫_0^1 dx 1/√(x) = 2,
        (2) ∫_0^1 dx ln(x)/√(x) = -4.
        */
        WriteLine("\n----- Part B -----\n");
        (double firstIntegralB, double error1B) = AdaptiveIntegrator.IntegrateCCTransform(delegate(double x) {return 1/Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(firstIntegralB - 2.0) < 0.01)
        {
            WriteLine($"First integral was computed successfully within specified accuracy of {acc} using Clenshaw-Curtis variable transform");
        }
        else
        {
            WriteLine($"First integral was not computed successfully within specified accuracy of {acc} using Clenshaw-Curtis variable transform");
        }
        (double secondIntegralB, double error2B) = AdaptiveIntegrator.IntegrateCCTransform(delegate(double x) {return Log(x)/Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(secondIntegralB + 4.0) < 0.01)
        {
            WriteLine($"Second integral was computed successfully within specified accuracy of {acc} using Clenshaw-Curtis variable transform");
        }
        else
        {
            WriteLine($"Second integral was not computed successfully within specified accuracy of {acc} using Clenshaw-Curtis variable transform");
        }
    }
}