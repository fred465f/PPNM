/* Program uses ODE class in LinearAlgebra namespace to solve the problem of
equatorial motion and saves results to a file for later plotting. */

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
        double initialParameterValue = 0.0;
        double finalParameterValue = 500.0;
        Vector initialSolutionValue1 = new Vector("1\n0.01"); // Circular motion
        Vector initialSolutionValue2 = new Vector("1\n-0.5"); // Elliptical motion
        Vector initialSolutionValue3 = new Vector("1\n-0.5"); // Relativistic precession motion

        // Make instances of ODE class.
        ODE ode1 = new ODE(CreateFirstOrderDiffEq(0.0), initialParameterValue, initialSolutionValue1);
        ODE ode2 = new ODE(CreateFirstOrderDiffEq(0.0), initialParameterValue, initialSolutionValue2);
        ODE ode3 = new ODE(CreateFirstOrderDiffEq(0.01), initialParameterValue, initialSolutionValue3);

        // Call driver to solve ODE for circular motion.
        GenericList<double> xList1 = new GenericList<double>();
        GenericList<Vector> yList1 = new GenericList<Vector>();
        Vector yAtFinalParameterValue1 = ode1.Driver(finalParameterValue, xList1, yList1);

        // Call driver to solve ODE for elliptical motion.
        GenericList<double> xList2 = new GenericList<double>();
        GenericList<Vector> yList2 = new GenericList<Vector>();
        Vector yAtFinalParameterValue2 = ode2.Driver(finalParameterValue, xList2, yList2);

        // Call driver to solve ODE for relativistic precession motion.
        GenericList<double> xList3 = new GenericList<double>();
        GenericList<Vector> yList3 = new GenericList<Vector>();
        Vector yAtFinalParameterValue3 = ode3.Driver(finalParameterValue, xList3, yList3);

        // Output results to standard output stream for later plotting.
        for (int i = 0; i < Min(Min(xList1.Length, xList2.Length), xList3.Length); i++)
        {
            WriteLine($"{xList1[i]},{yList1[i][0]},{xList2[i]},{yList2[i][0]},{xList3[i]},{yList3[i][0]}");
        }
    }

    // Method that for given relativistic parameter epsilon, creates system of first order diff. eq.'s.
    public static Func<double, Vector, Vector> CreateFirstOrderDiffEq(double epsilon)
    {
        Func<double, Vector, Vector> function = (double x, Vector y) => new Vector($"{y[1]}\n{1 - y[0] + epsilon*y[0]*y[0]}");
        return function;
    }
}