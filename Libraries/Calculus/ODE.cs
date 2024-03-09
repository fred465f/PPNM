/* Implements embedded Runge-Kutta stepper with error estimate and an adaptive step-size
driver for solving Ordinary Differential Equation (ODE) Initial Value Problems (IVP). */

using System;
using LinearAlgebra;
using DataStructures;
using static System.Math;
using static System.Console;

namespace Calculus
{
    public class ODE
    {
        // Fields.
        public Func<double, Vector, Vector> f; // From dy/dx = f(x,y).
        public double a; // Initial parameter value.
        public Vector ya; // Initial value of solution y at a.

        // Constructor.
        public ODE(Func<double, Vector, Vector> function, double initialParameterValue, Vector initialSolutionValue)
        {
            f = function;
            a = initialParameterValue;
            ya = initialSolutionValue;
        }

        // Driver.
        public Vector Driver(double b, GenericList<double> xList = null, GenericList<Vector> yList = null, double h = 0.01, double absoluteError = 0.01, double relativeError = 0.01)
        {
            // If initial parameter value a is less than final parameter value b, throw an exception.
            if (b < a)
            {
                throw new ArgumentException("Initial parameter value a is less than final parameter value b", $"a = {a} and b = {b}");
            }
            else
            {
                // Prepare variables.
                double x = a;
                Vector y = ya.Copy();
                if (xList != null && yList != null)
                {
                    xList.Add(x);
                    yList.Add(y);
                }

                // Driver.
                do
                {
                    // Check if we are done or one step away from being done.
                    if (x >= b)
                    {
                        return ya;
                    }
                    else if (x + h >= b)
                    {
                        h = b - x;
                    }

                    // Make step.
                    var (yh, stepError) = Stepper(x, y, h);

                    // Compute tolerances and check the step acceptance condition.
                    Vector tolerances = new Vector(yh.Length);
                    for (int i = 0; i < yh.Length; i++)
                    {
                        tolerances[i] = (absoluteError + relativeError * Abs(yh[i])) * Sqrt(h/(b-a));
                    }
                    bool condition = true;
                    for (int i = 0; i < y.Length; i++)
                    {
                        if (stepError[i] > tolerances[i])
                        {
                            condition = false;
                        }
                    }

                    // If this condition holds true, save step.
                    if (condition)
                    {
                        x += h;
                        y = yh;
                        if (xList != null && yList != null)
                        {
                            xList.Add(x);
                            yList.Add(y);
                        }
                    }

                    // Choose smallest ratio of tolerance/error for change of step size h.
                    Vector absStepError = stepError.Apply(Abs);
                    double factor = tolerances[0] / absStepError[0];
                    for (int i = 0; i < y.Length; i++)
                    {
                        factor = Min(factor, tolerances[i] / absStepError[i]);
                    }

                    // Change step size and try again.
                    h *= Min(Pow(factor, 0.25) * 0.95, 2);
                } while (true);
            }
        }

        // Stepper with error estimate using embedded midpoint/Euler method.
        public (Vector, Vector) Stepper(double x, Vector y, double h)
        {
            // Embedded lower order Euler method.
            Vector k0 = f(x, y);

            // Higher order midpoint method.
            Vector k1 = f(x + h/2, y + k0* (h/2));

            // Estimate of y(x + h).
            Vector yh = y + k1 * h;

            // Estimate of error.
            Vector stepError = (k1 - k0) * h;

            // Return results.
            return (yh, stepError);
        }
    }
}