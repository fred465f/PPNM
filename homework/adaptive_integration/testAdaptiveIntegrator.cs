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
        (double firstIntegralA, double error1A, int numStepsFirstA) = AdaptiveIntegrator.Integrate(delegate(double x) {return Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(firstIntegralA - 2.0/3.0) < acc)
        {
            WriteLine($"First integral was computed successfully within specified accuracy of {acc} in {numStepsFirstA} steps");
        }
        else
        {
            WriteLine($"First integral was not computed successfully within specified accuracy of {acc} in {numStepsFirstA} steps");
        }
        (double secondIntegralA, double error2A, int numStepsSecondA) = AdaptiveIntegrator.Integrate(delegate(double x) {return 1/Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(secondIntegralA - 2.0) < acc)
        {
            WriteLine($"Second integral was computed successfully within specified accuracy of {acc} in {numStepsSecondA} steps");
        }
        else
        {
            WriteLine($"Second integral was not computed successfully within specified accuracy of {acc} in {numStepsSecondA} steps");
        }
        (double thirdIntegralA, double error3A, int numStepsThirdA) = AdaptiveIntegrator.Integrate(delegate(double x) {return 4*Sqrt(1 - x*x);}, 0, 1, acc, eps);
        if (Abs(thirdIntegralA - PI) < acc)
        {
            WriteLine($"Third integral was computed successfully within specified accuracy of {acc} in {numStepsThirdA} steps");
        }
        else
        {
            WriteLine($"Third integral was not computed successfully within specified accuracy of {acc} in {numStepsThirdA} steps");
        }
        (double fourthIntegralA, double error4A, int numStepsFourthA) = AdaptiveIntegrator.Integrate(delegate(double x) {return Log(x)/Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(fourthIntegralA + 4) < acc)
        {
            WriteLine($"Fourth integral was computed successfully within specified accuracy of {acc} in {numStepsFourthA} steps");
        }
        else
        {
            WriteLine($"Fourth integral was not computed successfully within specified accuracy of {acc} in {numStepsFourthA} steps");
        }

        /* Test open quadrature adaptive integration using Clenshaw-Curtis variable transformation
        on functions appearing in part B. */
        /*
        (1) ∫_0^1 dx 1/√(x) = 2,
        (2) ∫_0^1 dx ln(x)/√(x) = -4.
        */
        WriteLine("\n----- Part B -----\n");
        (double firstIntegralB, double error1B, int numStepsFirstB) = AdaptiveIntegrator.IntegrateCCTransform(delegate(double x) {return 1/Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(firstIntegralB - 2.0) < acc)
        {
            WriteLine($"First integral was computed successfully within specified accuracy of {acc} using Clenshaw-Curtis variable transform in {numStepsFirstB} steps");
        }
        else
        {
            WriteLine($"First integral was not computed successfully within specified accuracy of {acc} using Clenshaw-Curtis variable transform in {numStepsFirstB} steps");
        }
        (double secondIntegralB, double error2B, int numStepsSecondB) = AdaptiveIntegrator.IntegrateCCTransform(delegate(double x) {return Log(x)/Sqrt(x);}, 0, 1, acc, eps);
        if (Abs(secondIntegralB + 4.0) < acc)
        {
            WriteLine($"Second integral was computed successfully within specified accuracy of {acc} using Clenshaw-Curtis variable transform in {numStepsSecondB} steps");
        }
        else
        {
            WriteLine($"Second integral was not computed successfully within specified accuracy of {acc} using Clenshaw-Curtis variable transform in {numStepsSecondB} steps");
        }

        /* Test whether variable transforms allowed computation of improper integrals works as intended as part of part C. */
        /*
        (1) ∫_-inf^+inf dx exp(-x^2) = π,
        (2) ∫_1^+inf dx 1/x^3 = 1/2.
        */
        WriteLine("\n----- Part C -----\n");
        (double firstIntegralC, double error1C, int numStepsFirstC) = AdaptiveIntegrator.Integrate(delegate(double x) {return Exp(-x*x);}, double.NegativeInfinity, double.PositiveInfinity, acc, eps);
        if (Abs(firstIntegralC - Sqrt(PI)) < acc)
        {
            WriteLine($"First improper integral was computed successfully within specified accuracy of {acc} in {numStepsFirstC} steps");
        }
        else
        {
            WriteLine($"First improper integral was not computed successfully within specified accuracy of {acc} in {numStepsFirstC} steps");
        }
        (double secondIntegralC, double error2C, int numStepsSecondC) = AdaptiveIntegrator.Integrate(delegate(double x) {return 1/Pow(x, 4);}, 1, double.PositiveInfinity, acc, eps);
        if (Abs(secondIntegralC - 1.0/3.0) < acc)
        {
            WriteLine($"Second improper integral was computed successfully within specified accuracy of {acc} in {numStepsSecondC} steps");
        }
        else
        {
            WriteLine($"Second improper integral was not computed successfully within specified accuracy of {acc} in {numStepsSecondC} steps");
        }
    }
}