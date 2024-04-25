/* Program tests Newton method in RootFinder class in Calculus namespace, by testing it
on root finding problems with analytically known results. */

using System;
using Calculus;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main()
    {
        // Variables.
        double eps = 0.0001;
        Func<double, double> f1D = delegate(double z) {return Pow(z - 1, 2);};
        double x1D = 1.1;
        double analyticRoot1D = 1.0;
        Func<Vector, Vector> f2D = delegate(Vector z) {return new Vector($"{Pow(z[0], 2) + Pow(z[1] - 2, 2)}\n{Pow(z[0], 2)}");};
        Vector x2D = new Vector("0.1\n2.1");
        Vector analyticRoot2D = new Vector("0\n2");

        // Test RootFinder class on the two functions above.
        WriteLine("----- Part A -----\n");
        double root1D = RootFinder.Newton(f1D, x1D, eps);
        if (Abs(root1D - analyticRoot1D) < 100*eps)
        {
            WriteLine($"Analytic root to 1D function was found within an accuracy of {100*eps}");
        }
        else
        {
            WriteLine($"Analytic root to 1D function was not found within an accuracy of {100*eps}");
        }
        Vector root2D = RootFinder.Newton(f2D, x2D, eps);
        if (Vector.Norm(root2D - analyticRoot2D) < 100*eps)
        {
            WriteLine($"Analytic root to 2D function was found within an accuracy of {100*eps}");
        }
        else
        {
            WriteLine($"Analytic root to 2D function was not found within an accuracy of {100*eps}");
        }

        /* Find the extremum(s) of Rosenbrock's valley function, 
        f(x,y) = (1-x)^2+100(y-x^2)^2 
        by finding roots of its gradient. Its gradient is found analytically to be:
        Grad(f) = (-2(1-x) - 400(y - x^2)x, 200(y - x^2)).
        Easy to see that Grad(f)(1, 1) = (0, 0). So we know the existence of one analytic root.
        If Grad(f)(x, y) = (0, 0) => y = x^2 => -2 + 2x = 0, so x needs to be 1 => y = 1. So there
        only exist one root of Grad(f), namely (1, 1). Lets find it numerically. */
        Vector xRVF = new Vector("1.5\n0.5");
        Vector analyticalRootRVF = new Vector("1.0\n1.0");
        Func<Vector, Vector> gradOfRVF = delegate(Vector z) {return new Vector($"{-2*(1-z[0]) - 400.0*(z[1] - Pow(z[0], 2))*z[0]}\n{200.0*(z[1] - Pow(z[0], 2))}");};
        Vector rootRVF = RootFinder.Newton(gradOfRVF, xRVF, eps);
        if (Vector.Norm(rootRVF - analyticalRootRVF) < eps)
        {
            WriteLine($"Analytic root to Rosenbrock's valley function was found within an accuracy of {eps}. This should due to analytical considerations be the only root.");
        }
        else
        {
            WriteLine($"Analytic root to Rosenbrock's valley function was not found within an accuracy of {eps}");
        }
    }
}