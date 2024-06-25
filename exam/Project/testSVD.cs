/*
Program tests all the implemented features in SVD class in LinearAlgebra namespace.
Results of tests are saved in output file 'Out.testSVD.txt'. 

The specific tests performed are:
1: Compute SVD of k random nxn matrices A with n in [1, 50] randomly chosen in each case and check A = USV^T, U^TU = I and V^TV = VV^T = I.
2: Compute SVD of k random tall mxn matrices A with m > n in [1, 50] randomly chosen in each case and check A = USV^T, U^TU = I and V^TV = VV^T = I.
3: Compute SVD of k random tall mxn matrices A with m > n in [1, 50] randomly chosen in each case and check whether computed Pseudo-Inverse A^- satisfies AA^-A = A.
4: Compute least squares solution to overdetermined system of linear equations Ax = b for k random tall mxn matrices with m > n in [1, 50] randomly chosen in each case,
   using both QR-decomposition and singular value decomposition. Check that both methods agree. QR-decomposition was implemented and tested in a homework.
5: Compute SVD of k random tall mxn matrices A with m > n in [1, 50] randomly chosen in each case. Testing that the rank-r approximation minimizes the Frobenius norm of
   the difference between the matrix and all rank-r matrices is not easy. So I make a more basic test, checking in the limiting case of r = Rank(A)
   that the approximation is equal to A itself.
6: Compute SVD of k random symmetric nxn matrices A with n in [1, 50] randomly chosen in each case. To test the rank computation implemented in the SVD class, would require
   us to determine the number of linearly independent columns (ai) of our matrix, which in general would require us to determine the number of linearly independent
   non-trivial solutions to the eq.:
                                                b1*a1 + ... + bn*an = 0,       bi are real numbers.
   Instead of testing this general complicated case, I make a more basic test by assuming that A is a square symmetric matrix. In this case we can use
   the Jacobi eigenvalue algorithm to compute EVD of A. In the basis of eigenvectors the rank is particularly easy to compute as the number of non-zero eigenvalues. These
   two methods are then compared.
*/

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

public class Program
{
    public static void Main(string[] args)
    {
        // Variables.
        int k = 100;
        int seed = 1;
        int testNumber = 0;
        double absoluteError = 1e-9, relativeError = 1e-9;

        // Process command line input arguments.
        foreach (var arg in args)
        {
            var words = arg.Split(":");
            if (words[0] == "-testNumber")
            {
                testNumber = int.Parse(words[1]);
            }
        }

        // Run desired test k number of times and check results within preset absolute/relative errors.
        if (testNumber == 1)
        {
            RunTestNumberOne(k, seed, absoluteError, relativeError);
        }
        else if (testNumber == 2)
        {
            RunTestNumberTwo(k, seed, absoluteError, relativeError);
        }
        else if (testNumber == 3)
        {
            RunTestNumberThree(k, seed, absoluteError, relativeError);
        }
        else if (testNumber == 4)
        {
            RunTestNumberFour(k, seed, absoluteError, relativeError);
        }
        else if (testNumber == 5)
        {
            RunTestNumberFive(k, seed, absoluteError, relativeError);
        }
        else if (testNumber == 6)
        {
            RunTestNumberSix(k, seed, absoluteError, relativeError);
        }
    }

    // Static method runs test # 1.
    public static void RunTestNumberOne(int k, int seed, double absoluteError, double relativeError)
    {
        // Variables.
        int passedCases = 0;
        int failedCases = 0;
        var rnd = new Random(seed);

        // Perform test.
        for (int l = 0; l < k; l++)
        {
            // Choose size of square matrix randomly.
            int n = rnd.Next(1, 51);

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
            if (!Matrix.Approx(UTtimesU, identity, absoluteError, relativeError))
            {
                failedCases += 1;
                continue;
            }
            else if (!Matrix.Approx(VTtimesV, identity, absoluteError, relativeError))
            {
                failedCases += 1;
                continue;
            }
            else if (!Matrix.Approx(VtimesVT, identity, absoluteError, relativeError))
            {
                failedCases += 1;
                continue;
            }
            else if (!Matrix.Approx(A, UtimesStimesVT, absoluteError, relativeError))
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
        WriteLine($"Performed SVD using implemented algorithm on {k} real square matrices A \neach of random size in [1, 50] and checked whether U and V are orthogonal and \nwhether A = USV^T.\n");
        WriteLine($"This resulted in {passedCases} passed cases and {failedCases} failed cases.\n\n");
    }

    // Static method runs test # 2.
    public static void RunTestNumberTwo(int k, int seed, double absoluteError, double relativeError)
    {
        // Variables.
        int passedCases = 0;
        int failedCases = 0;
        var rnd = new Random(seed);

        // Perform test.
        for (int l = 0; l < k; l++)
        {
            // Choose size of tall matrix randomly.
            int n = rnd.Next(1, 51);
            int m = rnd.Next(n, 51);

            // Construct random mxn tall matrix.
            Matrix A = new Matrix(m, n);
            for (int i = 0; i < m; i++)
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
            Matrix UtimesS = new Matrix(m, n);
            for (int i = 0; i < n; i++)
            {
                UtimesS[i] = svd.U[i] * svd.S[i];
            }
            Matrix UtimesStimesVT = UtimesS * svd.V.T();

            // Check whether desired properties of SVD holds. If one fails, continue to next run.
            if (!Matrix.Approx(UTtimesU, identity, absoluteError, relativeError))
            {
                failedCases += 1;
                continue;
            }
            else if (!Matrix.Approx(VTtimesV, identity, absoluteError, relativeError))
            {
                failedCases += 1;
                continue;
            }
            else if (!Matrix.Approx(VtimesVT, identity, absoluteError, relativeError))
            {
                failedCases += 1;
                continue;
            }
            else if (!Matrix.Approx(A, UtimesStimesVT, absoluteError, relativeError))
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
        WriteLine("---------------- Test # 2 ----------------\n");
        WriteLine($"Performed SVD using implemented algorithm on {k} real tall matrices A \neach of random size mxn for m > n in [1, 50] and checked whether U are semi-orthogonal, \nV are orthogonal and whether A = USV^T.\n");
        WriteLine($"This resulted in {passedCases} passed cases and {failedCases} failed cases.\n\n");
    }

    // Static method runs test # 3.
    public static void RunTestNumberThree(int k, int seed, double absoluteError, double relativeError)
    {
        // Variables.
        int passedCases = 0;
        int failedCases = 0;
        var rnd = new Random(seed);

        // Perform test.
        for (int l = 0; l < k; l++)
        {
            // Choose size of tall matrix randomly.
            int n = rnd.Next(1, 51);
            int m = rnd.Next(n, 51);

            // Construct random mxn tall matrix.
            Matrix A = new Matrix(m, n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = rnd.NextDouble();
                }
            }

            // Compute SVD.
            SVD svd = new SVD(A);

            // Make necessary computations for test.
            Matrix AAminusA = A * svd.PseudoInverse() * A;

            // Check whether AA^-A = A.
            if (Matrix.Approx(AAminusA, A, absoluteError, relativeError))
            {
                passedCases += 1;
            }
            else
            {
                failedCases += 1;
            }
        }

        // Print result of text to command-line.
        WriteLine("---------------- Test # 3 ----------------\n");
        WriteLine($"Performed SVD using implemented algorithm on {k} real tall matrices A \neach of random size mxn for m > n in [1, 50] and checked whether the pseudo-inverse \ncomputed using SVD class satisfies AA^-A = A.\n");
        WriteLine($"This resulted in {passedCases} passed cases and {failedCases} failed cases.\n\n");
    }

    // Static method runs test # 4.
    public static void RunTestNumberFour(int k, int seed, double absoluteError, double relativeError)
    {
        // Variables.
        int passedCases = 0;
        int failedCases = 0;
        var rnd = new Random(seed);

        // Perform test.
        for (int l = 0; l < k; l++)
        {
            // Choose size of tall matrix randomly.
            int n = rnd.Next(1, 51);
            int m = rnd.Next(n, 51);

            // Construct random mxn tall matrix and vector of length m.
            Matrix A = new Matrix(m, n);
            Vector b = new Vector(m);
            for (int i = 0; i < m; i++)
            {
                b[i] = rnd.NextDouble();
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = rnd.NextDouble();
                }
            }

            // Compute SVD.
            SVD svd = new SVD(A);

            // Compute QRD
            QRGS qrgs = new QRGS(A);

            // Compute least squares solution to overdetermined system of linear equations Ax = b using both QRD and SVD.
            Vector solutionSVD = svd.LeastSquaresSVD(b);
            Vector solutionQRD = qrgs.SolveLinearEq(b);

            // Compare least squares solutions.
            if (Vector.Approx(solutionSVD, solutionQRD, absoluteError, relativeError))
            {
                passedCases += 1;
            }
            else
            {
                failedCases += 1;
            }
        }

        // Print result of text to command-line.
        WriteLine("---------------- Test # 4 ----------------\n");
        WriteLine($"Performed SVD using implemented algorithm on {k} real tall matrices A \neach of random size mxn for m > n in [1, 50] and checked whether the least squares \nsolution to the overdetermined system of linear equations Ax = b matched the solution \nobtained using the QR-decomposition algorithm implemented in the homework.\n");
        WriteLine($"This resulted in {passedCases} passed cases and {failedCases} failed cases.\n\n");
    }

    // Static method runs test # 5.
    public static void RunTestNumberFive(int k, int seed, double absoluteError, double relativeError)
    {
        // Variables.
        int passedCases = 0;
        int failedCases = 0;
        var rnd = new Random(seed);

        // Perform test.
        for (int l = 0; l < k; l++)
        {
            // Choose size of tall matrix randomly.
            int n = rnd.Next(1, 51);
            int m = rnd.Next(n, 51);

            // Construct random mxn tall matrix and vector of length m.
            Matrix A = new Matrix(m, n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = rnd.NextDouble();
                }
            }

            // Compute SVD.
            SVD svd = new SVD(A);

            // Compute rank-r approximation of A where r = Rank(A).
            Matrix approximationRankR = svd.LowerRankApprox(svd.GetRank());

            // Compare least squares solutions.
            if (Matrix.Approx(approximationRankR, A, absoluteError, relativeError))
            {
                passedCases += 1;
            }
            else
            {
                failedCases += 1;
            }
        }

        // Print result of text to command-line.
        WriteLine("---------------- Test # 5 ----------------\n");
        WriteLine($"Performed SVD using implemented algorithm on {k} real tall matrices A \neach of random size mxn for m > n in [1, 50] and checked that in the limiting case \nwhere r = Rank(A), that the rank-r approximation of A is equal to A itself.\n");
        WriteLine($"This resulted in {passedCases} passed cases and {failedCases} failed cases.\n\n");
    }

    // Static method runs test # 6.
    public static void RunTestNumberSix(int k, int seed, double absoluteError, double relativeError)
    {
        // Variables.
        int passedCases = 0;
        int failedCases = 0;
        var rnd = new Random(seed);

        // Perform test.
        for (int l = 0; l < k; l++)
        {
            // Choose size of square matrix randomly.
            int n = rnd.Next(1, 51);

            // Construct random symmetric nxn square matrix.
            Matrix A = new Matrix(n);
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (j == i)
                    {
                        A[i, i] = rnd.NextDouble();
                    }
                    else
                    {
                        double entry = rnd.NextDouble();
                        A[i, j] = entry;
                        A[j, i] = entry;
                    }
                }
            }

            // Compute SVD.
            SVD svd = new SVD(A);

            // Compute EVD.
            EVD evd = new EVD(A);

            // Compare rank of A both using SVD and EVD.
            int rankSVD = svd.GetRank();
            int rankEVD = 0;
            Vector eigenValues = evd.w;
            for (int i = 0; i < eigenValues.Length; i++)
            {
                if (!Matrix.Approx(eigenValues[i], 0))
                {
                    rankEVD += 1;
                }     
            }

            // Compare rank computations.
            if (rankEVD == rankSVD)
            {
                passedCases += 1;
            }
            else
            {
                failedCases += 1;
            }
        }

        // Print result of text to command-line.
        WriteLine("---------------- Test # 6 ----------------\n");
        WriteLine($"Performed SVD using implemented algorithm on {k} real symmetric square matrices A \neach of random size in [1, 50] and checked whether the computed rank agreed with the \none obtained using EVD of A.\n");
        WriteLine($"This resulted in {passedCases} passed cases and {failedCases} failed cases.\n\n");
    }
}