/* Program uses the shooting method to compute ground state energy of s-wave
solutions to the Schrödinger eq. for the Hydrogen atom for some given input
parameters: absolute error of ODE integrator (accODE), relative error of ODE
integrator (epsODE), and integration limits of ODE integrator rMin and rMax. */

using System;
using Calculus;
using LinearAlgebra;
using DataStructures;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main(string[] args)
    {
        // Variables. (default values of input parameters)
        double eps = 0.001;
        double accODE = 0.001;
        double epsODE = 0.001;
        double rMax = 14;
        double rMin = 0.001;
        double energyGuess = - 0.5;
        int numPoints = 5;

        // Add heading of file explaining file structure.
        WriteLine("# This file contains data regarding convergence of GSE of radial s-wave solution to SE of H-atom.");
        WriteLine("# Parameters varied are absolute error of ODE integrator (accODE), relative error of ODE \n# integrator (epsODE), and integration limits of ODE integrator rMin and rMax.");
        WriteLine($"# Their default values are: accODE = {accODE}, epsODE = {epsODE}, rMin = {rMin} and rMax = {rMax}.");
        WriteLine("# File is structured such that every two columns fit together, with first one containing values of parameters varied and second one GSE.\n#");

        // Vary accODE and save results.
        double[] accODEs = new double[] {0.01, 0.008, 0.006, 0.004, 0.002};
        double[] accODEsGSE = new double[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            double numericalGSE = RootFinder.Newton(delegate(double E) {return M(E, rMin, rMax, accODEs[i], epsODE);}, energyGuess, eps);
            accODEsGSE[i] = numericalGSE;
        }

        // Vary epsODE and save results.
        double[] epsODEs = new double[] {0.01, 0.008, 0.006, 0.004, 0.002};
        double[] epsODEsGSE = new double[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            double numericalGSE = RootFinder.Newton(delegate(double E) {return M(E, rMin, rMax, accODE, epsODEs[i]);}, energyGuess, eps);
            epsODEsGSE[i] = numericalGSE;
        }

        // Vary rMin and save results.
        double[] rMins = new double[] {0.01, 0.008, 0.006, 0.004, 0.002};
        double[] rMinsGSE = new double[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            double numericalGSE = RootFinder.Newton(delegate(double E) {return M(E, rMins[i], rMax, accODE, epsODE);}, energyGuess, eps);
            rMinsGSE[i] = numericalGSE;
        }

        // Vary rMax and save results.
        double[] rMaxs = new double[] {10, 11, 12, 13, 14};
        double[] rMaxsGSE = new double[numPoints];
        for (int i = 0; i < numPoints; i++)
        {
            double numericalGSE = RootFinder.Newton(delegate(double E) {return M(E, rMin, rMaxs[i], accODE, epsODE);}, energyGuess, eps);
            rMaxsGSE[i] = numericalGSE;
        }

        // Output results to standard output stream.
        WriteLine("#accODE,GSE,epsODE,GSE,rMin,GSE,rMax,GSE");
        for (int i = 0; i < numPoints; i++)
        {
            WriteLine($"{accODEs[i]},{accODEsGSE[i]},{epsODEs[i]},{epsODEsGSE[i]},{rMins[i]},{rMinsGSE[i]},{rMaxs[i]},{rMaxsGSE[i]}");
        }
    }

    // Auxillary function M(E).
    public static double M(double E, double rMin, double rMax, double accODE, double epsODE)
    {
        /* Rewrite second order ODE to first order ODE's. */
        /*
        We have
                  -(1/2)f'' -(1/r)f = Ef     and     f(rMin) = rMin - rMin^2.
        Let y0 = f and y1 = f'. Then we get the following equations,
                        y0' = y1     and     y1' = -2*(1/r + E)*y0,
        and initial data,
                   y0(rMin) = rMin - rMin^2     and     y1(rMin) = 1 - 2rMin.
        */

        // Variables.
        Vector initialData = new Vector($"{rMin - Pow(rMin, 2)}\n{1 - 2*rMin}");
        Func<double, Vector, Vector> function = delegate(double r, Vector y) {return new Vector($"{y[1]}\n{-2*(1/r + E)*y[0]}");};

        // Make instance of ODE class.
        ODE ode = new ODE(function, rMin, initialData);

        // Call driver to solve ODE.
        Vector yAtrMax = ode.Driver(rMax, absoluteError: accODE, relativeError: epsODE);

        // Return result.
        return yAtrMax[0];
    }
}