/* Program applies ODE class in LinearAlgebra namespace to the Lotka-
Volterra system and saves results to a file for later plotting. */

using System;
using Calculus;
using LinearAlgebra;
using DataStructures;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main()
    {
        // Variables.
        double initialParameterValue = 0;
        double finalParameterValue = 15;
        Vector initialSolutionValue = new Vector("10\n5"); // This and functions needs to be changed!
        Func<double, Vector, Vector> function = (double t, Vector z) => new Vector($"{1.5*t - z[0]*z[1]}\n{-3*z[1] + z[0]*z[1]}");

        // Make instance of ODE class.
        ODE ode = new ODE(function, initialParameterValue, initialSolutionValue);

        // Call driver to solve ODE.
        GenericList<double> tList = new GenericList<double>();
        GenericList<Vector> zList = new GenericList<Vector>();
        Vector yAtFinalParameterValue = ode.Driver(finalParameterValue, tList, zList);

        // Output results to standard output stream for later plotting.
        WriteLine("#t,x,y");
        for (int i = 0; i < zList.Length; i++)
        {
            WriteLine($"{tList[i]},{zList[i][0]},{zList[i][1]}");
        }
    }
}