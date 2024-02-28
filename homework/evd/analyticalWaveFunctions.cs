// Program constructs data corresponding to the analytical to s-wave radial Schrödinger eq. for the hydrogen atom.

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
    {
        static void Main(string[] args)
        {
            // Variables.
            double rMax = 10, rDelta = 0.05; // Default values if none were provided from command line. 
            string outFile = "";

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
                else if (words[0] == "-outFile")
                {
                    outFile = words[1];
                }
            }

            // Construct used r-values.
            int nPoints = (int) (rMax / rDelta) - 1;
            Vector r = new Vector(nPoints);
            for (int i = 0; i < nPoints; i++)
            {
                r[i] = rDelta * (i + 1);
            }

            /* Save analytical wave-functions corresponding to lowest lying eigenvalues for later plotting. 
            Each column in data file, corresponds the k'th wave vector ordered from left to right. */
            using (var outStream = new System.IO.StreamWriter(outFile, append:false))
            {   
                for (int i = 0; i < r.Length; i++)
                {
                    double x = r[i];
                    outStream.WriteLine($"{r[i]} {2 * x * Exp(-x)} {(1/Sqrt(2)) * x * (1 - x/2) * Exp(-x/2)}");
                }
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