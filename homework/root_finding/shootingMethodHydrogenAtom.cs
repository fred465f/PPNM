/* Program uses the shooting method to compute ground state energy of s-wave
solutions to the Schrödinger eq. for the Hydrogen atom. Convergence is checked
with respect to rMax, rMin and acc, eps. Results are saved for later plotting */

/* 
------------------------------- Outline of the problem at hand ------------------------------------

The s-wave radial Schrödinger equation for the Hydrogen atom reads (in units "Bohr radius" and "Hartree"),

                                    -(1/2)f'' -(1/r)f = Ef. 

Solutions satisfy two boundary conditions, 

                                      f(r --> 0) = r - r^2.
                                         f(r --> ∞) = 0.

To avoid problems with numerically integration to infinity, we instead impose

                                           f(rMax) = 0,

for rMax way larger than typical atomic sizes (could be 10 Bohr radii). Similarly the Coloumb potential diverges
at r = 0, so if rMin is way smaller than typical atomic sizes (could be 0.1 Bohr radii), then we instead impose

                                     f(rMin) = rMin - rMin^2.

The idea is then to use the ODE solver in the Calculus namespace to solve the following IVP:

                     -(1/2)f'' -(1/r)f = Ef     and     f(rMin) = rMin - rMin^2.

Let F_E(r) be a solution. For many choices of E, F_E(r) will not satisfy:

                                          F_E(rMax) = 0,

only for certain discrete choices of E. Define the auxillary function:

                                         M(E) = F_E(rMax).

The shooting method is then using root finding algorithms to find roots of M(E). We will try to find the lowest negative root,
corresponding to the ground state energy E_0 and the corresponding wave-function. 
This will be compared to the analytic solution with,

                              E_0 = -1/2     and     f_0(r) = r exp(-r)

These solutions will be checked for varying rMax, rMin as well as varying acc, eps.
*/

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
        double eps = 0.05;
        double rMax = 8;
        double rMin = 0.1;
        double analyticGSE = -0.5;
        Func<double, double> analyticWF = delegate(double r) {return r * Exp(-r);};
        GenericList<double> rList = new GenericList<double>();
        GenericList<Vector> yList = new GenericList<Vector>();

        // Find ground state energy.
        double currentEnergyGuess = - 0.6;
        double numericalGSE = RootFinder.Newton(delegate(double E) {return M(E, rMin, rMax);}, currentEnergyGuess, eps);
        WriteLine($"#Numerical GSE = {numericalGSE}\n#");

        // If numericalGSE is close to analyticGSE save results.
        if (Abs(numericalGSE - analyticGSE) < eps)
        {
            // Compute corresponding wave function.
            Vector initialData = new Vector($"{rMin - Pow(rMin, 2)}\n{1 - 2*rMin}");
            Func<double, Vector, Vector> function = delegate(double r, Vector y) {return new Vector($"{y[1]}\n{-0.5*(1/r + numericalGSE)*y[0]}");};
            ODE ode = new ODE(function, rMin, initialData);
            Vector yAtrMax = ode.Driver(rMax, rList, yList);

            // Output results to standard output stream.
            WriteLine("#r,numericalGSE,analyticGSE");
            for (int i = 0; i < rList.Length; i++)
            {
                WriteLine($"{rList[i]},{yList[i][0]},{analyticWF(rList[i])}");
            }
        }
    }

    // Auxillary function M(E).
    public static double M(double E, double rMin, double rMax)
    {
        /* Rewrite second order ODE to first order ODE's. */
        /*
        We have
                  -(1/2)f'' -(1/r)f = Ef     and     f(rMin) = rMin - rMin^2.
        Let y0 = f and y1 = f'. Then we get the following equations,
                        y0' = y1     and     y1' = -(1/2)*(1/r + E)*y0,
        and initial data,
                   y0(rMin) = rMin - rMin^2     and     y1(rMin) = 1 - 2rMin.
        */

        // Variables.
        Vector initialData = new Vector($"{rMin - Pow(rMin, 2)}\n{1 - 2*rMin}");
        Func<double, Vector, Vector> function = delegate(double r, Vector y) {return new Vector($"{y[1]}\n{-0.5*(1/r + E)*y[0]}");};

        // Make instance of ODE class.
        ODE ode = new ODE(function, rMin, initialData);

        // Call driver to solve ODE.
        Vector yAtrMax = ode.Driver(rMax);

        // Return result.
        return yAtrMax[0];
    }
}