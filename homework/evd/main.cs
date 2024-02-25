/* Program tests functionality of EVD class in LinearAlgebra namespace. Seems as if the convergence criteria
is chosen to be the convergence of diagonal elements, then the output matrices
satisfies the desired relations only within 6 significant digits for input matrices of size (100, 100). 
Even though it is slower, it might be useful to use as a convergence criteria that the largest off-diagonal
element is smaller than some desired absolute/relative error.
Program also uses EVD class to solve the s-wave radial Schrödinger eq. for the hydrogen atom. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
    {
        static void Main(string[] args)
        {
            // Variables.
            double absoluteError = 1e-6, relativeError = 1e-6;
            int n = 100;
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

            // Check functionality of EVD class.
            WriteLine("----- Part A -----");
            CheckEVD(n, absoluteError, relativeError);
            WriteLine("");
            
            /* Solve s-wave radial Schrödinger eq. for Hydrogen atom using manually chosen 
            parameters of r_max and dr for which it seemed that the ground state energy converged. 
            This is checked using convergence plots named:
            Out.drConvergence.gnuplot.png and Out.rMaxConvergence.gnuplot.png. */
            WriteLine("----- Part B -----");
            (Vector w, Matrix V) = SolveRadialEq(rMax, rDelta);
            WriteLine("S-wave radial Schrödinger eq. for the Hydrogen atom successfully solved numerically using EVD class.");

            /* Save wave-functions corresponding to lowest lying eigenvalues for later plotting. 
            Each column in data file, corresponds the k'th wave vector ordered from left to right. */
            using (var outStream = new System.IO.StreamWriter(outFile, append:false))
            {   
                Vector firstWaveVector = V[0] / Sqrt(rDelta);
                Vector secondWaveVector = V[1] / Sqrt(rDelta);
                Vector thirdWaveVector = V[2] / Sqrt(rDelta);
                int nPoints = (int) (rMax / rDelta) - 1;
                Vector r = new Vector(nPoints);
                for (int i = 0; i < nPoints; i++)
                {
                    r[i] = rDelta * (i + 1);
                }
                for (int i = 0; i < w.Length; i++)
                {
                    outStream.WriteLine($"{r[i]} {firstWaveVector[i]} {secondWaveVector[i]} {thirdWaveVector[i]}");
                }
            }
            WriteLine($"Saved wave vectors corresponding to radial wave numbers 1, 2 and 3 for s-waves in {outFile}.");
        }

        // Static method to check functionality of EVD class.
        static void CheckEVD(int n, double absoluteError, double relativeError)
        {
            // Create random symmetric square matrix of size (n, n).
            var rnd = new System.Random(1);
            Matrix M = new Matrix(n);
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    double randomDouble = rnd.NextDouble();
                    M[i, j] = randomDouble;
                    M[j, i] = randomDouble;
                }
            }

            // Create instance of EVD class. Eigenvalue-decomposition occurs doing construction of instance.
            EVD evd = new EVD(M);

            // Compute relevant matrices for functionality checks.
            Matrix D = Matrix.Diag(evd.w);
            Matrix I = Matrix.Identity(D.NumRows);
            
            // Perform functionality checks.
            if (Matrix.Approx(D, evd.V.T() * M * evd.V, absoluteError, relativeError))
            {
                WriteLine("V^T * M * V = D");
            }
            if (Matrix.Approx(evd.V * D * evd.V.T(), M, absoluteError, relativeError))
            {
                WriteLine("V * D * V^T = M");
            }
            if (Matrix.Approx(I, evd.V.T() * evd.V) && Matrix.Approx(I, evd.V * evd.V.T()))
            {
                WriteLine("V^T * V = V * V^T = I");
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