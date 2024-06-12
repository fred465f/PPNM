/* FourierTransform class in Calculus namespace implements the Cooley-Tukey algorithm for
the fast Fourier transform as well as the classical discrete Fourier transform as well as
their respective inverses. (NOT TESTED YET!)*/

using System;
using System.Numerics;
using static System.Math;
using static System.Console;

namespace Calculus
{
    public static class FourierTransform
    {
        /* Computes FFT of input data and returns result in new complex array. */
        public static Complex[] FFT(Complex[] x)
        {
            // Variables.
            int N = x.Length;
            Complex[] c = new Complex[N];

            // Do FFT.
            FFT(-1, N, x, 0, 1, c, 0);

            // Return result.
            return c;
        }

        /* Computes inverse FFT of input data and returns result in new complex array. */
        public static Complex[] InverseFFT(Complex[] c)
        {
            // Variables.
            int N = c.Length;
            Complex[] x = new Complex[N];

            // Do inverse FFT.
            FFT(1, N, c, 0, 1, x, 0);
            for (int i = 0; i < N; i++)
            {
                x[i] /= N;
            }

            // Return result.
            return x;
        }

        /* Computes DFT of input data and returns result in new complex array. */
        public static Complex[] DFT(Complex[] x)
        {
            // Variables.
            int N = x.Length;
            Complex[] c = new Complex[N];

            // Do DFT.
            DFT(-1, N, x, 0, 1, c, 0);

            // Return result.
            return c;
        }

        /* Computes inverse DFT of input data and returns result in new complex array. */
        public static Complex[] InverseDFT(Complex[] c)
        {
            // Variables.
            int N = c.Length;
            Complex[] x = new Complex[N];

            // Do inverse DFT.
            DFT(1, N, c, 0, 1, x, 0);
            for (int i = 0; i < N; i++)
            {
                x[i] /= N;
            }

            // Return result.
            return x;
        }
        
        /* Computes the classical discrete Fourier transform of input data and stores
        result in input complex array. */
        private static void DFT(int sign, int N, Complex[] x, int ix, int stride, Complex[] c, int ic)
        {
            for (int k = 0; k < N; k++)
            {
                c[ic + k] = 0;
                for (int n = 0; n < N; n++)
                {
                    c[ic + k] += x[ix + n*stride] * Complex.Exp(sign*2*PI*Complex.ImaginaryOne*n*k/N);
                }
            }
        }

        /* Computes fast Fourier transform using Cooley-Tukey algorithm on input data
        and stores result in input complex array. It will be computed recursively, with
        the base case N = 1 being trivial. */
        private static void FFT(int sign, int N, Complex[] x, int ix, int stride, Complex[] c, int ic)
        {
            if (N == 1)
            {
                c[ic] = x[ix];
            }
            else if (N%2 == 0)
            {
                FFT(sign, N/2, x, ix, 2*stride, c, ic);
                FFT(sign, N/2, x, ix + stride, 2*stride, c, ix + N/2);
                for (int k = 0; k < N/2; k++)
                {
                    Complex p = c[ic + k];
                    Complex q = Complex.Exp(sign*2*PI*Complex.ImaginaryOne*k/N) * c[ic + k + N/2];
                    c[ic + k] = p + q;
                    c[ic + k + N/2] = p - q;
                }
            }
            else
            {
                DFT(sign, N, x, ix, stride, c, ic);
            }
        }
    }
}