/* Program tests all the implemented features in SVD class in LinearAlgebra namespace.
Results of tests are saved in output file 'Out.testSVD.txt'. 

The specific tests performed are:
1: Compute SVD of k random nxn matrices A with n in [1, 100] randomly chosen in each case and check A = USV^T, U^TU = I and V^TV = VV^T = I.
2: ...
*/

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

public class Program
{
    public static void Main()
    {
        // ----------------------- Test # 1 -----------------------

        // Variables.
        int k = 100;
        int seed = 1;
        int passedCases = 0;
        int failedCases = 0;
        var rnd = new Random(seed);

        // Perform test.
        for (int l = 0; l < k; l++)
        {
            // Choose size of square matrix randomly.
            int n = rnd.Next(1, 100);

            // Construct random nxn square matrix.
            Matrix A = new Matrix(n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = rnd.NextDouble();
                }
            }

            // Compute SVD.
            SVD svd = new SVD(A);

            // Make necessary computations for test.
            Matrix identity = Matrix.Identity(n);
            Matrix UTtimesU = svd.U.T() * svd.U;
            Matrix VTtimesV = svd.V.T() * svd.V;
            Matrix VtimesVT = svd.V * svd.V.T();
            Matrix UtimesS = new Matrix(n);
            for (int i = 0; i < n; i++)
            {
                UtimesS[i] = svd.U[i] * svd.S[i];
            }
            Matrix UtimesStimesVT = UtimesS * svd.V.T();

            // Check whether desired properties of SVD holds. If one fails, continue to next run.
            if (!Matrix.Approx(UTtimesU, identity))
            {
                failedCases += 1;
                continue;
            }
            else if (!Matrix.Approx(VTtimesV, identity))
            {
                failedCases += 1;
                continue;
            }
            else if (!Matrix.Approx(VtimesVT, identity))
            {
                failedCases += 1;
                continue;
            }
            else if (!Matrix.Approx(A, UtimesStimesVT))
            {
                failedCases += 1;
                continue;
            }
            else
            {
                passedCases += 1;
            }
        }

        // Print result of text to command-line.
        WriteLine("---------------- Test # 1 ----------------\n");
        WriteLine($"Performed SVD using implemented algorithm on {k} real square matrices A \neach of random size in [1, 100] and checked whether U and V are orthogonal and \nwhether A = USV^T.\n");
        WriteLine($"This resulted in {passedCases} passed cases and {failedCases} failed cases.");
    }
}