/* Several versions of Monte-Carlo integration is implemented for computing
multi-dimensional integrals, using different sampling methods. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace Calculus
{
    public static class MonteCarloIntegrator
    {
        // Implements plain Monte-Carlo integration.
        public static (double, double) IntegratePlain(Func<Vector, double> f, Vector a, Vector b, int N)
        {
            // If input vectors a and b representing lower/upper limits of multi-dimensional array were not equal throw an exception.
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Input vectors a and b representing lower/upper limits of multi-dimensional array were not equal", $"Length(a) = {a.Length} and Length(b) = {b.Length}");
            }

            // Variables.
            int dim = a.Length;
            double V = 1;
            double sum = 0, sumOfSquares = 0;

            // Compute auxiliary rectangular volume V.
            for (int i = 0; i < dim; i++)
            {
                V *= b[i] - a[i];
            }

            // Prepare memory for vector containing random sampling points.
            Vector x = new Vector(dim);
            var rnd = new Random();

            // Perform random sampling.
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    x[j] = a[j] + rnd.NextDouble() * (b[j] - a[j]);
                }
                double fx = f(x);
                sum += fx;
                sumOfSquares += fx * fx;
            }

            // Compute and return integral.
            double mean = sum/N;
            double sigma = Sqrt(sumOfSquares/N - mean*mean);
            return (mean*V, sigma*V/Sqrt(N));
        }

        // Implements Monte-Carlo integration using Quasi-random sequences.
        public static (double, double) IntegrateQRS(Func<Vector, double> f, Vector a, Vector b, int N)
        {
            // If input vectors a and b representing lower/upper limits of multi-dimensional array were not equal throw an exception.
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Input vectors a and b representing lower/upper limits of multi-dimensional array were not equal", $"Length(a) = {a.Length} and Length(b) = {b.Length}");
            }

            // Variables.
            int dim = a.Length;
            double V = 1;
            double sumHalton = 0, sumLatticeRule = 0;
            Vector alpha = new Vector(dim);
            for (int i = 0; i < dim; i++)
            {
                alpha[i] = Sqrt(PI + i) - Truncate(Sqrt(PI + i));
            }

            // Compute auxiliary rectangular volume V.
            for (int i = 0; i < dim; i++)
            {
                V *= b[i] - a[i];
            }

            // Prepare memory for vectors containing sampling points using Quasi-Random sequences.
            Vector xHalton = new Vector(dim);
            Vector xLatticeRule = new Vector(dim);

            // Perform random sampling.
            for (int n = 1; n <= N; n++)
            {
                // First for Halton Quasi-Random sequences.
                xHalton = QuasiRandomSequenceHalton(n, dim);
                for (int i = 0; i < dim; i++)
                {
                    xHalton[i] = xHalton[i] * (b[i] - a[i]) + a[i];
                }
                sumHalton += f(xHalton);

                // Secondly for Lattice rule Quasi-Random sequences.
                xLatticeRule = QuasiRandomSequenceLatticeRule(n, dim, alpha);
                for (int i = 0; i < dim; i++)
                {
                    xLatticeRule[i] = xLatticeRule[i] * (b[i] - a[i]) + a[i];
                }
                sumLatticeRule += f(xLatticeRule);
            }

            // Compute and return integral.
            double meanHalton = sumHalton/N;
            double meanLatticeRule = sumLatticeRule/N;
            double error = Abs(meanHalton - meanLatticeRule);
            return ((meanHalton + meanLatticeRule)*0.5*V, error);
        }

        // Implements Monte-Carlo integration using recursive stratified sampling.
        public static (double, double) IntegrateRSS(Func<Vector, double> f, Vector a, Vector b, int N)
        {
            // If input vectors a and b representing lower/upper limits of multi-dimensional array were not equal throw an exception.
            if (a.Length != b.Length)
            {
                throw new ArgumentException("Input vectors a and b representing lower/upper limits of multi-dimensional array were not equal", $"Length(a) = {a.Length} and Length(b) = {b.Length}");
            }

            // Variables.
            int nMin = 200;
            int d = a.Length;

            // Check if current number of points N is less than specified nMin.
            if (N < nMin)
            {
                return MonteCarloIntegrator.IntegratePlain(f, a, b, N);
            }
            else
            {
                // Find dimension with largest sub-variance.
                double[] largestSubVariances = new double[2] {0.0, 0.0};
                int index = 0;
                Vector aUpdated = new Vector(d);
                Vector bUpdated = new Vector(d);
                for (int i = 0; i < d; i++)
                {
                    // Compute sub-variance for "right" sub-dimension.
                    aUpdated = a.Copy();
                    aUpdated[i] = (b[i] + a[i])/2;
                    (double subIntegralRight, double subVarianceRight) = MonteCarloIntegrator.IntegratePlain(f, aUpdated, b, nMin);

                    // Compute sub-variance for "left" sub-dimension.
                    bUpdated = b.Copy();
                    bUpdated[i] = (b[i] + a[i])/2;
                    (double subIntegralLeft, double subVarianceLeft) = MonteCarloIntegrator.IntegratePlain(f, a, bUpdated, nMin);

                    // Update values of sub-variance if this dimension has larger sub-variance than any previous one. Update the index as well.
                    if (Max(subVarianceLeft, subVarianceRight) > largestSubVariances[0] && Max(subVarianceLeft, subVarianceRight) > largestSubVariances[1])
                    {
                        largestSubVariances[0] = subVarianceLeft;
                        largestSubVariances[1] = subVarianceRight;
                        index = i;
                    }
                }

                // Compute proportionality of subdivision along this dimension.
                int nLeft = (int) Round(N * largestSubVariances[0]/(largestSubVariances[0] + largestSubVariances[1]));
                int nRight = N - nLeft;

                // Compute integrals.
                (double integralLeft, double varianceLeft) = IntegrateRSS(f, a, bUpdated, nLeft);
                (double integralRight, double varianceRight) = IntegrateRSS(f, aUpdated, b, nRight);

                // Return sum of integrals and corresponding error.
                return (integralLeft + integralRight, Sqrt(Pow(varianceLeft, 2) + Pow(varianceRight, 2)));
            }
        }

        // Implementation of some Quasi-Random sequences.
        private static Vector QuasiRandomSequenceHalton(int n, int dim)
        {
            // Coprime basis.
            int[] coprimeBasis = new int[18] {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61};

            // Throw exception if there are fewer basis elements "coprimeBasis.Length" than the dimension "dim" of them multi-dimensional integral.
            if (coprimeBasis.Length < dim)
            {
                throw new ArgumentException("There must be more coprime basis elements than the dimension of the multi-dimensional integral", $"coprimeBasis.Length = {coprimeBasis.Length} and dim = {dim}");
            }

            // Initialize memory.
            Vector x = new Vector(dim);

            // Construct Quasi-Random sequence x.
            for (int i = 0; i < dim; i++)
            {
                x[i] = CorputSequence(n, coprimeBasis[i]);
            }

            // Return result.
            return x;
        }
        private static double CorputSequence(int n, int b)
        {
            double q = 0;
            double bk = 1.0/((double)b);
            while (n > 0)
            {
                q += (n % b) * bk;
                n /= b;
                bk /= b;
            }
            return q;
        }
        private static Vector QuasiRandomSequenceLatticeRule(int n, int dim, Vector alpha)
        {
            // Initialize memory.
            Vector x = new Vector(dim);

            // Construct Quasi-Random vector.
            for (int i = 0; i < dim; i++)
            {
                x[i] = n*alpha[i] - Truncate(n*alpha[i]);
            }

            // Return result.
            return x;
        }
    }
}