/* Program computes ground state energy of the s-wave Hamiltonian for the Hydrogen atom
using EVD class in LinearAlgebra namespace. These computations are meant to be done for
varying r_max and dr values to check convergence of ground state energy. Makefile is 
optimized as to ensure that convergence computations are performed in parallel. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
    {
        static void Main(string[] args)
        {
            // Variables.
            double rMax = 10, rDelta = 0.5; 
            bool rMaxConvergenceTest = false;
            bool drConvergenceTest = false;

            // Process command line input arguments.
            foreach (var arg in args)
            {
                var words = arg.Split(":");
                if (words[0] == "-rMax")
                {
                    rMax = double.Parse(words[1]);
                }
                else if (words[0] == "-dr")
                {
                    rDelta = double.Parse(words[1]);
                }
                else if (words[0] == "-test")
                {
                    if (words[1] == "rMax")
                    {
                        rMaxConvergenceTest = true;
                    }
                    else if (words[1] == "dr")
                    {
                        drConvergenceTest = true;
                    }
                }
            }

            // Compute ground state energy with respect to r_max and r_delta.
            (Vector w, Matrix V) = SolveRadialEq(rMax, rDelta);
            if (rMaxConvergenceTest)
            {
                WriteLine($"{rMax} {w[0]}");
            }
            else if (drConvergenceTest)
            {
                WriteLine($"{rDelta} {w[0]}");
            }
        }

        // Method to solve s-wave radial Schrödinger eq. for the Hydrogen atom in units of Bohr radius for length and Hartree for energy.
        static (Vector w, Matrix V) SolveRadialEq(double rMax, double rDelta)
        {
            // Number of r-points on grid.
            int nPoints = (int) (rMax / rDelta) - 1;

            // Construct r-vector.
            Vector r = new Vector(nPoints);
            for (int i = 0; i < nPoints; i++)
            {
                r[i] = rDelta * (i + 1);
            }

            // Construct Hamiltonian.
            Matrix H = new Matrix(nPoints);
            for (int i = 0; i < nPoints; i++) // Kinetic term.
            {
                for (int j = 0; j < nPoints; j++)
                {
                    if (i == j)
                    {
                        H[i, i] = 1 / (rDelta * rDelta);
                    }
                    else if (i == j + 1)
                    {
                        H[i, j] = -1 / (2 * rDelta * rDelta);
                    }
                    else if (i == j - 1)
                    {
                        H[i, j] = -1 / (2 * rDelta * rDelta);
                    }
                    else
                    {
                        H[i, j] = 0;
                    }
                }
            }
            for (int i = 0; i < nPoints; i++) // Potential term.
            {
                H[i, i] += -1 / r[i];
            }

            // Create instance of EVD class performing EVD in process.
            EVD evd = new EVD(H);

            // Return results.
            return (evd.w, evd.V);
        }
    }