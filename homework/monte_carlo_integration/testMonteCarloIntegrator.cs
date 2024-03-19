/* Program tests implementation of the three different Monte-Carlo integrators in MonteCarloIntegrator class.
Several multi-dimensional integrals are computed and compared with analytical values.  */

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
        int N = 500000;
        double acc = 0.05;
        Vector aFirst = new Vector("0\n0");
        Vector bFirst = new Vector($"1\n{2*PI}");
        Vector aSecond = new Vector("0\n0\n0");
        Vector bSecond = new Vector($"{PI}\n{PI}\n{PI}");

        // Test IntegratePlain, IntegrateQRS and IntegrateRSS method in MonteCarloIntegrator class of several integrals with known analytic solutions.
        /* 
        (1) ∫_0^2π  dθ ∫_0^1  dr r*sin(θ),
        (2) ∫_0^π  dx/π ∫_0^π  dy/π ∫_0^π  dz/π [1-cos(x)cos(y)cos(z)]^(-1) = Γ(1/4)^4/(4π^3) ≈ 1.393203929685.
        */
        WriteLine("----- Part A -----\n");
        (double firstIntegralA, double firstErrorA) = MonteCarloIntegrator.IntegratePlain(delegate(Vector v) {return v[0];}, aFirst, bFirst, N);
        if (Abs(firstIntegralA - PI) < acc)
        {
            WriteLine($"Plain Monte-Carlo integrator evaluated the first integral correctly within an accuracy of {acc} in N = {N} steps with and actual error of {Abs(firstIntegralA - PI)}");
        }
        else
        {
            WriteLine($"Plain Monte-Carlo integrator did not evaluate the first integral correctly within an accuracy of {acc} in N = {N} steps with and actual error of {Abs(firstIntegralA - PI)}");
        }
        (double secondIntegralA, double secondErrorA) = MonteCarloIntegrator.IntegratePlain(delegate(Vector v) {return 1.0/(1-Cos(v[0])*Cos(v[1])*Cos(v[2]))*1/Pow(PI, 3);}, aSecond, bSecond, N);
        if (Abs(secondIntegralA - 1.393203929685) < acc)
        {
            WriteLine($"Plain Monte-Carlo integrator evaluated the second integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(secondIntegralA - 1.393203929685)}");
        }
        else
        {
            WriteLine($"Plain Monte-Carlo integrator did not evaluate the second integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(secondIntegralA - 1.393203929685)}");
        }
        WriteLine("\n----- Part B -----\n");
        (double firstIntegralB, double firstErrorB) = MonteCarloIntegrator.IntegrateQRS(delegate(Vector v) {return v[0];}, aFirst, bFirst, N);
        if (Abs(firstIntegralB - PI) < acc)
        {
            WriteLine($"QRS Monte-Carlo integrator evaluated the first integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(firstIntegralB - PI)}");
        }
        else
        {
            WriteLine($"QRS Monte-Carlo integrator did not evaluate the first integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(firstIntegralB - PI)}");
        }
        (double secondIntegralB, double secondErrorB) = MonteCarloIntegrator.IntegrateQRS(delegate(Vector v) {return 1.0/(1-Cos(v[0])*Cos(v[1])*Cos(v[2]))*1/Pow(PI, 3);}, aSecond, bSecond, N);
        if (Abs(secondIntegralB - 1.393203929685) < acc)
        {
            WriteLine($"QRS Monte-Carlo integrator evaluated the second integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(secondIntegralB - 1.393203929685)}");
        }
        else
        {
            WriteLine($"QRS Monte-Carlo integrator did not evaluate the second integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(secondIntegralB - 1.393203929685)}");
        }
        WriteLine("\n----- Part C -----\n");
        (double firstIntegralC, double firstErrorC) = MonteCarloIntegrator.IntegrateRSS(delegate(Vector v) {return v[0];}, aFirst, bFirst, N);
        if (Abs(firstIntegralC - PI) < acc)
        {
            WriteLine($"RSS Monte-Carlo integrator evaluated the first integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(firstIntegralC - PI)}");
        }
        else
        {
            WriteLine($"RSS Monte-Carlo integrator did not evaluate the first integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(firstIntegralC - PI)}");
        }
        (double secondIntegralC, double secondErrorC) = MonteCarloIntegrator.IntegrateRSS(delegate(Vector v) {return 1.0/(1-Cos(v[0])*Cos(v[1])*Cos(v[2]))*1/Pow(PI, 3);}, aSecond, bSecond, N);
        if (Abs(secondIntegralC - 1.393203929685) < acc)
        {
            WriteLine($"RRS Monte-Carlo integrator evaluated the second integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(secondIntegralC - 1.393203929685)}");
        }
        else
        {
            WriteLine($"RSS Monte-Carlo integrator did not evaluate the second integral correctly within an accuracy of {acc} in N = {N} steps with an actual error of {Abs(secondIntegralC - 1.393203929685)}");
        }
    }
}