using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace Calculus
{
    /* Recursive open-quadrature adaptive integrator using a higher order quadrature and 
    lower order embedded quadrature satisfying specified absolute and relative error goals.
    Open quadrature adaptive integration using Clenshaw-Curtis variable integration, integration
    error estimate and infinite limit integration is also implemented. */
    public static class AdaptiveIntegrator
    {
        // Implements above mentioned method.
        public static (double, double) Integrate(Func<double, double> f, double a, double b, double acc, double eps)
        {
            // Step counter.
            int numSteps = 0;

            // Check if either limit or both is infinity/-infinity and make appropriate variable transforms.
            if (double.IsPositiveInfinity(b) && double.IsNegativeInfinity(a))
            {
                Func<double, double> fTransformed = delegate(double t) {return (f((1-t)/t) + f(-(1-t)/t))/(t*t);};
                double f2 = fTransformed(2.0/6.0);
                double f3 = fTransformed(4.0/6.0);
                return IntegrateRecursiveStep(fTransformed, 0, 1, acc, eps, f2, f3, numSteps);
            }
            else if (double.IsPositiveInfinity(b))
            {
                Func<double, double> fTransformed = delegate(double t) {return (f(a + (1-t)/t))/(t*t);};
                double f2 = fTransformed(2.0/6.0);
                double f3 = fTransformed(4.0/6.0);
                return IntegrateRecursiveStep(fTransformed, 0, 1, acc, eps, f2, f3, numSteps);
            }
            else if (double.IsNegativeInfinity(a))
            {
                Func<double, double> fTransformed = delegate(double t) {return (f(b - (1-t)/t))/(t*t);};
                double f2 = fTransformed(2.0/6.0);
                double f3 = fTransformed(4.0/6.0);
                return IntegrateRecursiveStep(fTransformed, 0, 1, acc, eps, f2, f3, numSteps);
            }
            else
            {
                double f2 = f(a + 2*(b - a)/6);
                double f3 = f(a + 4*(b - a)/6);
                return IntegrateRecursiveStep(f, a, b, acc, eps, f2, f3, numSteps);
            }
        }

        // Recursive step.
        private static (double, double) IntegrateRecursiveStep(Func<double, double> f, double a, double b, double acc, double eps, double f2, double f3, int numSteps)
        {
            // Throw exception if numSteps gets too large, indicating no convergence of integral.
            if (numSteps > 1000000)
            {
                throw new ArgumentException("Number of recursive adaptive integral steps exceeded 1000000, indicating that the integral do not converge.");
            }

            // Compute f at specified nodes.
            double f1 = f(a + (b - a)/6);
            double f4 = f(a + 5*(b - a)/6);

            // Compute integral using higher order and lower order embedded method and estimate local error.
            double Q = (2*f1 + f2 + f3 + 2*f4)/6 * (b - a);
            double q = (f1 + f2 + f3 + f4)/4 * (b - a);
            double tolerance = acc + eps * Abs(Q);
            double error = Abs(Q - q);

            // Increase number of steps used by one

            // Check if the error performed is smaller than given tolerance.
            if (error < tolerance)
            {
                // Stop integral and return result.
                return (Q, Abs(Q - q));
            }
            else
            {
                // Perform recursive step.
                (double Q1, double error1) = IntegrateRecursiveStep(f, a, (a + b)/2, acc/Sqrt(2.0), eps, f1, f2, numSteps + 1);
                (double Q2, double error2) = IntegrateRecursiveStep(f, (a + b)/2, b, acc/Sqrt(2.0), eps, f3, f4, numSteps + 1);
                return (Q1 + Q2, error1 + error2);
            }
        }

        // Open quadrature adaptive integrator with Clenshaw-Curtis variable transformation.
        public static (double, double) IntegrateCCTransform(Func<double, double> f, double a, double b, double acc, double eps)
        {
            Func<double, double> fTransformed = delegate(double theta) {return f((a+b)/2 + (b-a)/2*Cos(theta)) * Sin(theta)*(b-a)/2;};
            return Integrate(fTransformed, 0, PI, acc, eps);
        }
    }
}