/* Program debugs ODE class in LinearAlgebra namespace by solving 
diff. eq. u'' = -u and saving results to a file for later plotting. */

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
        double finalParameterValue = 2 * PI;
        Vector initialSolutionValue = new Vector("0\n1");
        Func<double, Vector, Vector> function = (double x, Vector y) => new Vector($"{y[1]}\n{-y[0]}");

        // Make instance of ODE class.
        ODE ode = new ODE(function, initialParameterValue, initialSolutionValue);

        // Call driver to solve ODE.
        GenericList<double> xList = new GenericList<double>();
        GenericList<Vector> yList = new GenericList<Vector>();
        Vector yAtFinalParameterValue = ode.Driver(finalParameterValue, xList, yList);

        // Output results to standard output stream for later plotting.
        for (int i = 0; i < yList.Length; i++)
        {
            WriteLine($"{xList[i]},{yList[i][0]}");
        } 
    }
}