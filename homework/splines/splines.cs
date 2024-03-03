using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

// Class implementing linear spline interpolation for chosen set of discrete data points.
public class LinearSpline
{
    // Fields.
    public Vector x, y;

    // Constructor.
    public LinearSpline(Vector xValues, Vector yValues)
    {
        x = xValues.Copy();
        y = yValues.Copy();
    }

    // Do interpolation using linear splines.
    public double Interpolate(double z) 
    {
        if (!(z >= x[0] && z <= x[x.Length - 1]))
        {
            throw new ArgumentException("Chosen z-value must lie in interval [xValues[0], xValues[Length - 1]].", $"z-value = {z} does not lie in interval [{x[0]}, {x[x.Length - 1]}].");
        }
        else
        {
            int i = BinarySearch(x, z);
            double dx = x[i + 1] - x[i];
            if (!(dx > 0))
            {
                throw new ArgumentException("Either dx = 0 or dx < 0, either case should not occur and is a result of faulty input values.", $"dx = {dx}");
            }
            double dy = y[i + 1] - y[i];
            return y[i] + dy/dx * (z - x[i]);
        }
    }

    // Perform integration from first x-value x[0] to chosen z > x[0].
    public double Integrate(double z)
    {
        if (!(z >= x[0] && z <= x[x.Length - 1]))
        {
            throw new ArgumentException("Chosen z-value must lie in interval [xValues[0], xValues[Length - 1]].", $"z-value = {z} does not lie in interval [{x[0]}, {x[x.Length - 1]}].");
        }
        else
        {
            int i = BinarySearch(x, z);
            double integral = 0.0;
            for (int j = 0; j <= i; j++)
            {
                double dx = x[j + 1] - x[j];
                if (!(dx > 0))
                {
                    throw new ArgumentException("Either dx = 0 or dx < 0, either case should not occur and is a result of faulty input values.", $"dx = {dx}");
                }
                double dy = y[j + 1] - y[j];
                if (j == i)
                {
                    integral += (0.5) * (dy/dx) * z*z + (y[j] - (dy/dx) * x[j]) * z;
                    integral -= (0.5) * (dy/dx) * x[j]*x[j] + (y[j] - (dy/dx) * x[j]) * x[j];
                }
                else
                {
                    integral += (0.5) * (dy/dx) * x[j + 1]*x[j + 1] + (y[j] - (dy/dx) * x[j]) * x[j + 1];
                    integral -= (0.5) * (dy/dx) * x[j]*x[j] + (y[j] - (dy/dx) * x[j]) * x[j];
                }
            }
            return integral;
        }
    }

    // Perform binary search.
    public static int BinarySearch(Vector xValues, double z)
    {
        if (!(z >= xValues[0] && z <= xValues[xValues.Length - 1]))
        {
            throw new ArgumentException("Chosen z-value must lie in interval [xValues[0], xValues[Length - 1]].", $"z-value = {z} does not lie in interval [{xValues[0]}, {xValues[xValues.Length - 1]}].");
        }
        else
        {
            int i = 0;
            int j = xValues.Length - 1;
            while (j - i > 1)
            {
                int middle = (i + j) / 2;
                if (z > xValues[middle])
                {
                    i = middle;
                }
                else
                {
                    j = middle;
                }
            }
            return i;
        }
    }
}

// Class implementing quadratic spline interpolation for chosen set of discrete data points.
public class QuadraticSpline
{
    // Fields.

    // Constructor.

    // Do interpolation using quadratic splines.

    // Perform integration from first x-value x[0] to chosen z > x[0].

    // Perform differentiation from first x-value x[0] to chosen z > x[0].

    // Perform binary search.
}