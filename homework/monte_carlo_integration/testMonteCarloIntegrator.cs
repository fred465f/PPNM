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
        int N = 2000000;
        double acc = 0.01;

        // Test IntegratePlain method in MonteCarloIntegrator class of several integrals with known analytic solutions.
        /* 
        (1) ∫_0^2π  dθ ∫_0^1  dr r*sin(θ),
        (2) ∫_0^π  dx/π ∫_0^π  dy/π ∫_0^π  dz/π [1-cos(x)cos(y)cos(z)]^(-1) = Γ(1/4)^4/(4π^3) ≈ 1.3932039296856768591842462603255.
        */
        WriteLine("----- Part A -----\n");
        Vector aFirstA = new Vector("0\n0");
        Vector bFirstA = new Vector($"1\n{2*PI}");
        (double firstIntegralA, double firstErrorA) = MonteCarloIntegrator.IntegratePlain(delegate(Vector v) {return v[0];}, aFirstA, bFirstA, N);
        if (Abs(firstIntegralA - PI) < acc)
        {
            WriteLine($"Plain Monte-Carlo integrator evaluated the first integral correctly within an accuracy of {acc} in N = {N} steps");
        }
        else
        {
            WriteLine($"Plain Monte-Carlo integrator did not evaluate the first integral correctly within an accuracy of {acc} in N = {N} steps");
        }
        Vector aSecondA = new Vector("0\n0\n0");
        Vector bSecondA = new Vector($"{PI}\n{PI}\n{PI}");
        (double secondIntegralA, double secondErrorA) = MonteCarloIntegrator.IntegratePlain(delegate(Vector v) {return 1.0/(1-Cos(v[0])*Cos(v[1])*Cos(v[2]))*1/Pow(PI, 3);}, aSecondA, bSecondA, N);
        if (Abs(secondIntegralA - 1.3932039296856768591842462603255) < acc)
        {
            WriteLine($"Plain Monte-Carlo integrator evaluated the second integral correctly within an accuracy of {acc} in N = {N} steps");
        }
        else
        {
            WriteLine($"Plain Monte-Carlo integrator did not evaluate the second integral correctly within an accuracy of {acc} in N = {N} steps");
        }
    }
}