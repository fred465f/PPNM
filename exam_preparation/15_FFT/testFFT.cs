/* Program tests implementation of Cooley-Tukey FFT algorithm in FFT class in
Calculus namespace. It does so by applying it to analytic know cases as well as
checking that it is unitary. */

using System;
using Calculus;
using System.Numerics;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main()
    {
        // Variables.
        int N = 10000;

        /* Check FFT and DFT on known cases such as, f = A*cos(a*t) + B*cos(b*t) + C*cos(c*t), which in frequency 
        space should have a distinct peak at a, b and c with distinct amplitudes. */
        Func<double, double> f = t => 10*Cos(3*t) + 2*Cos(13*t) + 7*Cos(23*t);
        Complex[] x = new Complex[N];
        for (int i = 0; i < N; i++)
        {
            x[i] = new Complex(f(0.02*i), 0);
        }
        Complex[] c = FourierTransform.FFT(x);
        for (int i = 0; i < N; i++)
        {
            WriteLine($"{c[i].Real}");
        }
    }
}